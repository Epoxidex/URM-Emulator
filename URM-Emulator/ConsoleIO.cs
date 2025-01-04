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


            var builder = new StringBuilder();
            int columnWidth = Math.Max(registers.Values.Max(), registers.Keys.Max()).ToString().Length + 2;

            builder.AppendLine(new string('-', (columnWidth + 3) * registers.Count + 1));

            builder.Append("|");
            foreach (var key in registers.Keys.OrderBy(k => k))
            {
                builder.Append($" {registers[key].ToString().PadLeft(columnWidth)} |");
            }
            builder.AppendLine();

            builder.AppendLine(new string('-', (columnWidth + 3) * registers.Count + 1));

            builder.Append("|");
            foreach (var key in registers.Keys.OrderBy(k => k))
            {
                builder.Append($" {('R'+key.ToString()).PadLeft(columnWidth)} |");
            }
            builder.AppendLine();

            builder.AppendLine(new string('-', (columnWidth + 3) * registers.Count + 1));

            Console.WriteLine(builder.ToString());

        }
    }
}
