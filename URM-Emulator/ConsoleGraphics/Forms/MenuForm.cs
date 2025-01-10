﻿namespace URM_Emulator.ConsoleGraphics.Forms
{
    public class MenuForm : Form
    {
        public int SelectedIndex { get; set; } = 0;
        public int MaxRowLength  { get; private set; }

        public MenuForm(int x, int y, string title, string[] rows, bool border, ConsoleColor color = ConsoleColor.White)
            : base(x, y, title, rows, border, color)
        {
            MaxRowLength = rows.Max(x => x.Length);
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
                Console.SetCursorPosition(X + 1, Y + 1 + i);

                if (i == SelectedIndex)
                {
                    Console.BackgroundColor = Color;
                    Console.ForegroundColor = ConsoleColor.Black;
                }

                Console.Write(Rows[i].PadRight(MaxRowLength + 2));

                // Reset color
                Console.ResetColor();
                Console.ForegroundColor = Color;
            }
        }
    }
}
