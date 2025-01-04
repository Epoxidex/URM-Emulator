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
            int option = ShowMenu();
            Console.Clear();

            switch (option)
            {
                case 1:
                    PrintRegisters();
                    break;
            }

        }

        private int ShowMenu()
        {
            int option;

            do
            {
                Console.Clear();
                Console.WriteLine("1. Enter registers");
                Console.WriteLine("2. Enter instructions");
                Console.WriteLine("3. Execute instructions");
                Console.WriteLine("4. Step-by-step execution");
                Console.WriteLine("5. Exit");
                Console.WriteLine();
            }
            while (!int.TryParse(Console.ReadLine(), out option));

            return option;
        }

        private void PrintRegisters()
        {
            var registers = _urm.Registers;

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
            foreach (var value in registers.Values.OrderBy(k => k))
            {
                builder.Append($" {value.ToString().PadLeft(columnWidth)} |");
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
