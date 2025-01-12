namespace URM_Emulator.ConsoleGraphics.Forms
{
    public class Form
    {
        public int X {  get; set; }
        public int Y { get; set; }
        public string Title { get; set; }
        public string[] Rows { get; set; }
        public bool Border { get; set; }
        public bool PrintTitle { get; set; } = true;
        public ConsoleColor Color { get; set; }

        public Form(int x, int y, string title, string[] rows, bool border = true, ConsoleColor color = ConsoleColor.White)
        {
            X = x;
            Y = y;
            Title = title;
            Rows = rows;
            Border = border;
            Color = color;
        }

        public void Show()
        {
            Console.ForegroundColor = Color;

            if (Border)
            {
                Graphics.DrawBorder(X, Y, Rows, Title);
            }

            for (int i = 0; i < Rows.Length; i++)
            {
                Console.SetCursorPosition(X + 1, Y + 2 + i);
                Console.Write(Rows[i]);
            }
            if (PrintTitle)
            {
                Graphics.PrintTitle(Title, X + 2, Y);
            }
        }
    }
}
