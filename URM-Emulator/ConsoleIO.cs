using System.ComponentModel;
using System.Drawing;
using System.Text;

namespace URM_Emulator
{
    public class ConsoleIO
    {
        private readonly URM _urm;

        public ConsoleIO(URM urm)
        {
            _urm = urm;
        }

        public void Run()
        {
            while (true)
            {
                int option = ShowMenu();

                switch (option)
                {
                    case 1:
                        RunProgramEditor();
                        break;
                    case 2:
                        StepByStepExecution();
                        break;
                    case 3:
                        Console.WriteLine("Exiting...");
                        return;
                }
            }
        }

        private string EnterRegisters()
        {
            Console.Write("\nEnter register number and value in the following format: ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("register_number:value");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("For example: 1:42, to set the value 42 in register R1.");
            Console.Write(" > ");
            try
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                var input = Console.ReadLine().Split(':').Select(s => int.Parse(s.Trim())).ToList();
                _urm.SetRegisterValue(input[0], input[1]);
                return $"The register R{input[0]} is set to a value '{input[1]}'";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }

        private void RunProgramEditor()
        {
            string message = string.Empty;

            while (true)
            {

                Console.Clear();
                RenderManager.PrintProgram(_urm.Registers, _urm.Instructions);

                RenderManager.ColoredWriteLine(message, ConsoleColor.Cyan);
                Console.WriteLine();

                Console.WriteLine("Options:");
                Console.WriteLine("1. Set register value");
                Console.WriteLine("2. Reset all registers");
                Console.WriteLine("3. Choose instructions from file");
                Console.WriteLine("'x' to return to main menu");
                Console.WriteLine();
                Console.Write("Choose an option: ");

                string input = Console.ReadLine()?.Trim().ToLower();
                if (input.ToLower() == "x") break;

                switch (input)
                {
                    case "1":
                        message = EnterRegisters();
                        break;
                    case "2":
                        _urm.ResetRegisters();
                        message = "Registers reset.";
                        break;
                    case "3":
                        Console.Write("Enter path to file ('x' for cancelling): ");
                        message = GetInstructionsFromFile(Console.ReadLine());
                        break;
                    default:
                        message = "Invalid option. Try again.";
                        break;
                }
            }

        }

        private string GetInstructionsFromFile(string path)
        {
            if (path.Trim().ToLower() == "x") return "";

            string text = File.ReadAllText(path);
            try
            {
                _urm.SetInstructions(text);
                return "Instructions added.";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private void StepByStepExecution()
        {
            var oldRegisters = new Dictionary<int, int>(_urm.Registers);

            while (_urm.CurrentInstructionId < _urm.Instructions.Count)
            {
                Console.Clear();
                Console.Write("\x1b[3J");

                RenderManager.PrintExecutingInstructions(_urm.Instructions, _urm.CurrentInstructionId);

                if (_urm.CurrentInstructionId >= _urm.Instructions.Count || _urm.CurrentInstructionId < 0)
                {
                    Console.WriteLine("Program completed\n");
                    break;
                }

                Console.Write($"Current Instruction (#{_urm.CurrentInstructionId + 1}): ");
                RenderManager.ColoredWriteLine($"{_urm.Instructions[_urm.CurrentInstructionId]}", ConsoleColor.Yellow);

                var previousRegisters = new Dictionary<int, int>(_urm.Registers);
                _urm.ExecuteInstruction(_urm.Instructions[_urm.CurrentInstructionId]);
                var changedRegisters = GetChangedRegisters(previousRegisters, _urm.Registers);

                if (changedRegisters != null)
                {
                    Console.WriteLine("Changed registers: ");
                    foreach (var reg in changedRegisters)
                    {
                        RenderManager.ColoredWriteLine($"R{reg}: {(previousRegisters.ContainsKey(reg) ? previousRegisters[reg] : 0)} -> {_urm.Registers[reg]}", ConsoleColor.Blue);
                    }
                }
                Console.WriteLine();

                Console.WriteLine("Registers before execution:");

                RenderManager.PrintRegisters(previousRegisters, changedRegisters);

                Console.WriteLine("\nRegisters after execution:");
                RenderManager.PrintRegisters(_urm.Registers, changedRegisters);

                Console.WriteLine("\nPress 'Enter' to continue, 'q' to quit step-by-step mode and complete program.");
                var key = Console.ReadKey();

                if (key.Key == ConsoleKey.Q)
                {
                    _urm.ExecuteInstructions();
                    break;
                }
            }

            _urm.GoToFirstInstruction();

            Console.Clear();
            Console.WriteLine("Program completed");
            Console.WriteLine("Old registers:");
            RenderManager.PrintRegisters(oldRegisters);
            Console.WriteLine("New registers:");
            RenderManager.PrintRegisters(_urm.Registers);

            string input;
            do
            {
                Console.WriteLine("\nSave new register values? [y/n]");
                input = Console.ReadLine().Trim().ToLower();
            } while (input != "y" && input != "n");


            if (input == "n")
                _urm.SetRegistersValues(oldRegisters);
        }



        private int ShowMenu()
        {
            int option;

            do
            {
                Console.Clear();
                Console.WriteLine("Menu:");
                Console.WriteLine("1. Edit program");
                Console.WriteLine("2. Execute program");
                Console.WriteLine("3. Exit");
                Console.WriteLine();
                Console.Write("Choose an option: ");
            }
            while (!int.TryParse(Console.ReadLine(), out option));

            return option;
        }

        private HashSet<int> GetChangedRegisters(Dictionary<int, int> regs1, Dictionary<int, int> regs2)
        {
            HashSet<int> changedKeys = new HashSet<int>();

            foreach (var keyValuePair in regs1)
            {
                int key = keyValuePair.Key;
                if (!regs2.TryGetValue(key, out int value2) || keyValuePair.Value != value2)
                {
                    changedKeys.Add(key);
                }
            }

            foreach (var keyValuePair in regs2)
            {
                int key = keyValuePair.Key;
                if (!regs1.ContainsKey(key))
                {
                    changedKeys.Add(key);
                }
            }

            return changedKeys;
        }
    }
}