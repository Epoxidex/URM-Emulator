namespace URM_Emulator.ConsoleGraphics.Forms
{
    public class RegistersForm : Form
    {
        public Dictionary<int, int> Registers { get; set; }
        public HashSet<int> ChangedValues { get; set; }

        public RegistersForm(int x, int y, string title, Dictionary<int, int> registers, HashSet<int> changedValues = null, ConsoleColor color = ConsoleColor.White)
            : base(x, y, title, new string[0], true, color)
        {
            Registers = registers;
            ChangedValues = changedValues ?? new HashSet<int>();
        }

        public void Show()
        {
            int columnWidth = CalculateColumnWidth();
            string[] rows = GenerateRegisterRows(columnWidth);

            Console.ForegroundColor = Color;

            if (Border)
            {
                Graphics.DrawBorder(X, Y, rows, Title);
            }

            for (int i = 0; i < rows.Length; i++)
            {
                Console.SetCursorPosition(X + 1, Y + 2 + i);
                Console.Write(rows[i]);
            }
            if (PrintTitle)
            {
                Graphics.PrintTitle(Title, X + 2, Y);
            }
        }

        private string[] GenerateRegisterRows(int columnWidth)
        {
            var rows = new List<string>();

            // Header Row
            var header = "|" + string.Join("|", Registers.Keys.OrderBy(k => k).Select(k => $"R{k}".PadLeft(columnWidth))) + "|";
            rows.Add(header);

            // Separator Row
            var separator = new string('-', header.Length);
            rows.Add(separator);

            // Values Row
            var values = "|" + string.Join("|", Registers.Keys.OrderBy(k => k).Select(k =>
            {
                var value = Registers[k].ToString().PadLeft(columnWidth);
                return ChangedValues.Contains(k) ? $"*{value}*" : value;
            })) + "|";
            rows.Add(values);

            return rows.ToArray();
        }

        private int CalculateColumnWidth()
        {
            if (Registers == null || Registers.Count == 0) return 0;

            int maxKeyLength = Registers.Keys.Max().ToString().Length;
            int maxValueLength = Registers.Values.Max().ToString().Length;
            return Math.Max(maxKeyLength, maxValueLength) + 2;
        }
    }
}
