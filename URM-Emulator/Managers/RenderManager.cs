namespace URM_Emulator.Managers
{
    public static class RenderManager
    {
        private const string INSTRUCTION_SEPARATOR = "-------------------";

        /// <summary>
        /// Outputs a message with a specified console color.
        /// </summary>
        public static void ColoredWriteLine(string message, ConsoleColor color) =>
            ChangeConsoleColor(color, () => Console.WriteLine(message));

        /// <summary>
        /// Outputs a message without a newline using a specified console color.
        /// </summary>
        public static void ColoredWrite(string message, ConsoleColor color) =>
            ChangeConsoleColor(color, () => Console.Write(message));

        /// <summary>
        /// Displays the executing instructions with the current instruction highlighted.
        /// </summary>
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

        /// <summary>
        /// Displays the current state of the program, including registers and instructions.
        /// </summary>
        public static void PrintProgram(Dictionary<int, int> registers, List<string> instructions)
        {
            PrintRegisters(registers);
            ColoredWriteLine("Use instruction number [0] to terminate the program.", ConsoleColor.DarkYellow);
            Console.WriteLine("Current instructions:");
            PrintInstructions(instructions);
            Console.WriteLine(INSTRUCTION_SEPARATOR);
        }

        /// <summary>
        /// Outputs a list of instructions.
        /// </summary>
        public static void PrintInstructions(List<string> instructions)
        {
            if (instructions == null || instructions.Count == 0)
            {
                Console.WriteLine("No instructions to display.");
                return;
            }

            for (int i = 0; i < instructions.Count; i++)
            {
                Console.WriteLine($"[{i + 1}] {instructions[i]}");
            }
        }

        /// <summary>
        /// Outputs the state of the registers in a table format.
        /// </summary>
        public static void PrintRegisters(Dictionary<int, int> registers, HashSet<int> changedValues = null)
        {
            if (registers == null || registers.Count == 0)
            {
                Console.WriteLine("No registers to display.");
                return;
            }

            int columnWidth = CalculateColumnWidth(registers);
            string separator = GenerateSeparator(columnWidth, registers.Count);

            Console.WriteLine(separator);
            PrintRow(registers, columnWidth, changedValues, isKeyRow: true); // Keys (R1, R2...)
            Console.WriteLine(separator);
            PrintRow(registers, columnWidth, changedValues, isKeyRow: false); // Values
            Console.WriteLine(separator);
        }

        /// <summary>
        /// Changes the console text color and executes an action.
        /// </summary>
        private static void ChangeConsoleColor(ConsoleColor color, Action action)
        {
            var previousColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            action.Invoke();
            Console.ForegroundColor = previousColor;
        }

        /// <summary>
        /// Prints a row of register data.
        /// </summary>
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

        /// <summary>
        /// Generates a table separator line.
        /// </summary>
        private static string GenerateSeparator(int columnWidth, int columnCount) =>
            new string('-', (columnWidth + 3) * columnCount + 1);

        /// <summary>
        /// Calculates the width of each column based on the longest key or value in the registers.
        /// </summary>
        private static int CalculateColumnWidth(Dictionary<int, int> registers)
        {
            if (registers == null || registers.Count == 0) return 0;

            int maxKeyLength = registers.Keys.Max().ToString().Length;
            int maxValueLength = registers.Values.Max().ToString().Length;
            return Math.Max(maxKeyLength, maxValueLength) + 2;
        }
    }
}
