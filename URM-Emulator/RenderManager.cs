namespace URM_Emulator
{
    public static class RenderManager
    {
        public static void ColoredWriteLine(string message, ConsoleColor color)
        {
            ChangeConsoleColor(color, () => Console.WriteLine(message));
        }

        public static void ColoredWrite(string message, ConsoleColor color)
        {
            ChangeConsoleColor(color, () => Console.Write(message));
        }

        public static void PrintRegisters(Dictionary<int, int> registers, HashSet<int> changedValues = null)
        {
            if (registers.Count == 0)
            {
                Console.WriteLine("No registers to display.");
                return;
            }

            int columnWidth = CalculateColumnWidth(registers);
            string separator = GenerateSeparator(columnWidth, registers.Count);

            Console.WriteLine(separator);
            PrintRow(registers, columnWidth, changedValues, isKeyRow: false);
            Console.WriteLine(separator);
            PrintRow(registers, columnWidth, changedValues, isKeyRow: true);
            Console.WriteLine(separator);
        }

        private static void PrintRow(Dictionary<int, int> registers, int columnWidth, HashSet<int> changedValues, bool isKeyRow)
        {
            Console.Write("|");
            changedValues ??= new HashSet<int>();
            foreach (var key in registers.Keys.OrderBy(k => k))
            {
                string cellContent = isKeyRow ? $"R{key}".PadLeft(columnWidth) : registers[key].ToString().PadLeft(columnWidth);
                if (changedValues.Contains(key))
                {
                    ColoredWrite($" {cellContent} ", ConsoleColor.Blue);
                }
                else
                {
                    Console.Write($" {cellContent} ");
                }
                Console.Write("|");
            }
            Console.WriteLine();
        }

        private static void ChangeConsoleColor(ConsoleColor color, Action action)
        {
            var previousColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            action.Invoke();
            Console.ForegroundColor = previousColor;
        }

        private static string GenerateSeparator(int columnWidth, int columnCount)
        {
            return new string('-', (columnWidth + 3) * columnCount + 1);
        }

        private static int CalculateColumnWidth(Dictionary<int, int> registers)
        {
            return Math.Max(registers.Values.Max().ToString().Length, registers.Keys.Max().ToString().Length) + 2;
        }
    }
}
