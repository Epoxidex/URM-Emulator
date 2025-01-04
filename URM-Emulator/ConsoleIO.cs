using System.ComponentModel;
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
                        ExecuteInstructions();
                        break;
                    case 3:
                        StepByStepExecution();
                        break;
                    case 4:
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
                PrintProgram();

                ColoredWriteLine(message, ConsoleColor.Cyan);
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
        private void ExecuteInstructions()
        {
            Console.Clear();
            PrintProgram();

            var oldRegisters = new Dictionary<int, int>(_urm.Registers);
            _urm.ExecuteInstructions();
            Console.WriteLine("Registers after execution: ");
            PrintRegisters(_urm.Registers);

            string input;
            do
            {
                Console.WriteLine("Save new values for registers? [y/n]");
                input = Console.ReadLine().Trim().ToLower();
            }
            while (input != "n" && input != "y");

            if (input == "n")
                _urm.SetRegistersValues(oldRegisters);
        }

        private void StepByStepExecution()
        {
            Console.Clear();
            Console.WriteLine("Step-by-step execution started. Press 'Enter' to execute next instruction, 'q' to quit.\n");

            var oldRegisters = new Dictionary<int, int>(_urm.Registers);

            while (_urm.CurrentInstructionId < _urm.Instructions.Count)
            {
                Console.Clear();

                Console.WriteLine("Program Code:\n-------------------");
                for (int i = 0; i < _urm.Instructions.Count; i++)
                {
                    string pointer = i == _urm.CurrentInstructionId ? "->" : "  ";
                    var color = i == _urm.CurrentInstructionId ? ConsoleColor.Yellow : ConsoleColor.DarkYellow;
                    ColoredWriteLine($"{pointer} {i + 1}: {_urm.Instructions[i]}", color);
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                Console.WriteLine("\n-------------------\n");

                if (_urm.CurrentInstructionId >= _urm.Instructions.Count || _urm.CurrentInstructionId < 0)
                {
                    Console.WriteLine("Program completed\n");
                    break;
                }

                Console.WriteLine($"Current Instruction (#{_urm.CurrentInstructionId + 1}): {_urm.Instructions[_urm.CurrentInstructionId]}\n");

                Console.WriteLine("Registers before execution:");
                PrintRegisters(_urm.Registers);

                _urm.ExecuteInstruction(_urm.Instructions[_urm.CurrentInstructionId]);

                Console.WriteLine("\nRegisters after execution:");
                PrintRegisters(_urm.Registers);

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
            PrintRegisters(oldRegisters);
            Console.WriteLine("New registers:");
            PrintRegisters(_urm.Registers);

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
                Console.WriteLine("2. Execute instructions");
                Console.WriteLine("3. Step-by-step execution");
                Console.WriteLine("4. Exit");
                Console.WriteLine();
                Console.Write("Choose an option: ");
            }
            while (!int.TryParse(Console.ReadLine(), out option));

            return option;
        }

        private void PrintProgram()
        {
            PrintRegisters(_urm.Registers);
            ColoredWriteLine("Use instruction number [0] to terminate the program.", ConsoleColor.DarkYellow);
            Console.WriteLine("Current instructions:");
            PrintInstructions();
            Console.WriteLine("--------------------");
        }
        private void PrintInstructions()
        {
            var instructions = _urm.Instructions;
            for (int i = 0; i < _urm.Instructions.Count; i++)
            {
                Console.WriteLine($"[{i + 1}] {instructions[i]}");
            }
        }

        private void PrintRegisters(Dictionary<int, int> registers)
        {
            if (registers.Count == 0)
            {
                Console.WriteLine("No registers to display.");
                return;
            }

            int columnWidth = CalculateColumnWidth(registers);
            string separator = GenerateSeparator(columnWidth, registers.Count);

            var builder = new StringBuilder();

            builder.AppendLine(separator);
            builder.AppendLine(GenerateValuesRow(registers, columnWidth));
            builder.AppendLine(separator);
            builder.AppendLine(GenerateKeysRow(registers, columnWidth));
            builder.AppendLine(separator);

            Console.WriteLine(builder.ToString());
        }

        private int CalculateColumnWidth(Dictionary<int, int> registers)
        {
            return Math.Max(registers.Values.Max(), registers.Keys.Max()).ToString().Length + 2;
        }

        private string GenerateSeparator(int columnWidth, int columnCount)
        {
            return new string('-', (columnWidth + 3) * columnCount + 1);
        }

        private string GenerateValuesRow(Dictionary<int, int> registers, int columnWidth)
        {
            var builder = new StringBuilder();
            builder.Append("|");
            foreach (var value in registers.Keys.OrderBy(k => k))
            {
                builder.Append($" {registers[value].ToString().PadLeft(columnWidth)} |");
            }
            return builder.ToString();
        }

        private string GenerateKeysRow(Dictionary<int, int> registers, int columnWidth)
        {
            var builder = new StringBuilder();
            builder.Append("|");
            foreach (var key in registers.Keys.OrderBy(k => k))
            {
                builder.Append($" {('R' + key.ToString()).PadLeft(columnWidth)} |");
            }
            return builder.ToString();
        }

        private void ColoredWriteLine(string message, ConsoleColor color)
        {
            var previousColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = previousColor;
        }
    }
}