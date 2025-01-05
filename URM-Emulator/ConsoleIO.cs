using System.ComponentModel;
using System.Drawing;
using System.Text;
using URM_Emulator.Managers;

namespace URM_Emulator
{
    public class ConsoleIO
    {
        private readonly URM _urm;
        private readonly RegisterManager _registerManager;
        private readonly InstructionManager _instructionManager;
        private readonly ProgramManager _programManager;

        public ConsoleIO(URM urm)
        {
            _urm = urm;
            _registerManager = new RegisterManager(urm);
            _instructionManager = new InstructionManager(urm);
            _programManager = new ProgramManager(urm, _registerManager, _instructionManager);
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
                        RunExampleLoader();
                        break;
                    case 4:
                        Console.WriteLine("Exiting...");
                        return;
                }
            }
        }
        private void RunExampleLoader()
        {
            Console.Clear();
            Console.WriteLine("Choose example");
            Console.WriteLine("1. Sum of two numbers");

            string input = Console.ReadLine()?.Trim().ToLower();
            if (input.ToLower() == "x")
                return;

            switch (input)
            {
                case "1":
                    _programManager.LoadProgramFromFile("Examples\\Sum of two numbers.txt");
                    RenderManager.ColoredWriteLine("Program Loaded!", ConsoleColor.Cyan);
                    Console.ReadKey();
                break;
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
                        message = _registerManager.EnterRegisters();
                        break;
                    case "2":
                        _urm.ResetRegisters();
                        message = "Registers reset.";
                        break;
                    case "3":
                        Console.Write("Enter path to file ('x' for cancelling): ");
                        message = _instructionManager.GetInstructionsFromFile(Console.ReadLine());
                        break;
                    default:
                        message = "Invalid option. Try again.";
                        break;
                }
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
                var changedRegisters = _registerManager.GetChangedRegisters(previousRegisters, _urm.Registers);

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
                Console.WriteLine("3. Load example of program");
                Console.WriteLine("4. Exit");
                Console.WriteLine();
                Console.Write("Choose an option: ");
            }
            while (!int.TryParse(Console.ReadLine(), out option));

            return option;
        }
    }
}