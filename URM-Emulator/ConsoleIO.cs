using Microsoft.Win32;
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
                        RunRegistersEditor();
                        break;
                    case 2:
                        EnterInstructions();
                        break;
                    case 3:
                        ExecuteInstructions();
                        break;
                    case 4:
                        StepByStepExecution();
                        break;
                    case 5:
                        Console.WriteLine("Exiting...");
                        return;
                }
            }
        }

        private void RunRegistersEditor()
        {
            string message = string.Empty;

            while (true)
            {

                Console.Clear();
                PrintRegisters();

                Console.WriteLine(message);
                Console.WriteLine();

                Console.WriteLine("Options:");
                Console.WriteLine("1. Set register value");
                Console.WriteLine("2. Reset all registers");
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
                    default:
                        message = "Invalid option. Try again.";
                        break;
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
                var input = Console.ReadLine().Split(':').Select(s => int.Parse(s.Trim())).ToList();
                _urm.SetRegisterValue(input[0], input[1]);
                return $"The register R{input[0]} is set to a value '{input[1]}'";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private void EnterInstructions()
        {
            Console.Clear();
            PrintRegisters();

            Console.WriteLine("Current instructions:");
            PrintInstructions();
            Console.ReadLine();
        }

        private void ExecuteInstructions()
        {

        }

        private void StepByStepExecution()
        {

        }

        private int ShowMenu()
        {
            int option;

            do
            {
                Console.Clear();
                Console.WriteLine("Menu:");
                Console.WriteLine("1. Enter or edit registers");
                Console.WriteLine("2. Enter instructions");
                Console.WriteLine("3. Execute instructions");
                Console.WriteLine("4. Step-by-step execution");
                Console.WriteLine("5. Exit");
                Console.WriteLine();
                Console.Write("Choose an option: ");
            }
            while (!int.TryParse(Console.ReadLine(), out option));

            return option;
        }

        private void PrintInstructions()
        {
            var instructions = _urm.Instructions;
            for (int i = 0; i < _urm.Instructions.Count; i++)
            {
                Console.WriteLine($"[{i+1}] {instructions[i]}");
            }
        }

        private void PrintRegisters()
        {
            var registers = _urm.Registers;

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

    }
}
