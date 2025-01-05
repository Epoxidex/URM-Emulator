namespace URM_Emulator.Managers
{
    public static class RenderManager
    {
        private const string INSTRUCTION_SEPARATOR = "-------------------";
        public static void ColoredWriteLine(string message, ConsoleColor color)
        {
            ChangeConsoleColor(color, () => Console.WriteLine(message));
        }

        public static void ColoredWrite(string message, ConsoleColor color)
        {
            ChangeConsoleColor(color, () => Console.Write(message));
        }

        public static void PrintExecutingInstructions(List<string> instructions, int currentInstructionIndex)
        {
            Console.WriteLine("Program Code:");
            Console.WriteLine(INSTRUCTION_SEPARATOR);

            for (int i = 0; i < instructions.Count; i++)
            {
                string pointer = i == currentInstructionIndex ? "->" : "  ";
                var color = i == currentInstructionIndex ? ConsoleColor.Yellow : ConsoleColor.DarkYellow;
                ColoredWriteLine($"{pointer} {i + 1}: {instructions[i]}", color);
            }
            Console.WriteLine(INSTRUCTION_SEPARATOR);

        }
        public static void PrintProgram(Dictionary<int, int> registers, List<string> instructions)
        {
            PrintRegisters(registers);
            ColoredWriteLine("Use instruction number [0] to terminate the program.", ConsoleColor.DarkYellow);
            Console.WriteLine("Current instructions:");
            PrintInstructions(instructions);
            Console.WriteLine(INSTRUCTION_SEPARATOR);
        }
        public static void PrintInstructions(List<string> instructions)
        {
            for (int i = 0; i < instructions.Count; i++)
            {
                Console.WriteLine($"[{i + 1}] {instructions[i]}");
            }
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

        private static string GenerateSeparator(int columnWidth, int columnCount)
        {
            return new string('-', (columnWidth + 3) * columnCount + 1);
        }
        private static void ChangeConsoleColor(ConsoleColor color, Action action)
        {
            var previousColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            action.Invoke();
            Console.ForegroundColor = previousColor;
        }

        private static int CalculateColumnWidth(Dictionary<int, int> registers)
        {
            return Math.Max(registers.Values.Max().ToString().Length, registers.Keys.Max().ToString().Length) + 2;
        }
    }
}
