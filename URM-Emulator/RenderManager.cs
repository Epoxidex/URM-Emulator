namespace URM_Emulator
{
    public static class RenderManager
    {
        public static void ColoredWriteLine(string message, ConsoleColor color)
        {
            var previousColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = previousColor;
        }
        public static void ColoredWrite(string message, ConsoleColor color)
        {
            var previousColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(message);
            Console.ForegroundColor = previousColor;
        }
        public static void PrintRegisters(Dictionary<int, int> registers, HashSet<int> changedRegisters = null)
        {
            if (registers.Count == 0)
            {
                Console.WriteLine("No registers to display.");
                return;
            }

            int columnWidth = CalculateColumnWidth(registers);
            string separator = GenerateSeparator(columnWidth, registers.Count);

            Console.WriteLine(separator);
            PrintValuesRow(registers, columnWidth, changedRegisters);
            Console.WriteLine(separator);
            PrintKeysRow(registers, columnWidth, changedRegisters);
            Console.WriteLine(separator);
        }

        private static void PrintValuesRow(Dictionary<int, int> registers, int columnWidth, HashSet<int> changedValues = null)
        {
            Console.Write("|");
            foreach (var value in registers.Keys.OrderBy(k => k))
            {
                if (changedValues != null && changedValues.Contains(value))
                {
                    ColoredWrite($" {registers[value].ToString().PadLeft(columnWidth)} ", ConsoleColor.Blue);
                    Console.Write("|");
                }
                else
                {
                    Console.Write($" {registers[value].ToString().PadLeft(columnWidth)} |");
                }
            }
            Console.WriteLine();
        }

        private static void PrintKeysRow(Dictionary<int, int> registers, int columnWidth, HashSet<int> changedValues = null)
        {
            Console.Write("|");
            foreach (var key in registers.Keys.OrderBy(k => k))
            {
                if (changedValues != null && changedValues.Contains(key))
                {
                    ColoredWrite($" {('R' + key.ToString()).PadLeft(columnWidth)} ", ConsoleColor.Blue);
                    Console.Write("|");
                }
                else
                {
                    Console.Write($" {('R' + key.ToString()).PadLeft(columnWidth)} |");
                }
            }
            Console.WriteLine();
        }

        private static string GenerateSeparator(int columnWidth, int columnCount)
        {
            return new string('-', (columnWidth + 3) * columnCount + 1);
        }
        private static int CalculateColumnWidth(Dictionary<int, int> registers)
        {
            return Math.Max(registers.Values.Max(), registers.Keys.Max()).ToString().Length + 2;
        }

    }
}
