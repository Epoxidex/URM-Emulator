﻿
namespace URM_Emulator.ConsoleGraphics.Forms
{
    public class InstructionsForm : Form
    {
        public int CurrentInstructionIndex { get; set; } = -1;
        public int MaxRowLength { get; private set; } = 0;

        public InstructionsForm(int x, int y, string title, string[] instructions, ConsoleColor color = ConsoleColor.Yellow)
            : base(x, y, title, instructions, true, color)
        {
            if (instructions.Count() > 0)
                MaxRowLength = instructions.Max(x => x.Length);
        }

        public void Show()
        {
            Console.ForegroundColor = Color;

            if (string.IsNullOrWhiteSpace(string.Join("", Rows)))
            {
                Rows = [" There are no instructions "];
            }

            if (Border)
            {
                Graphics.DrawBorder(X, Y, Rows, Title);
            }

            for (int i = 0; i < Rows.Length; i++)
            {
                Console.SetCursorPosition(X + 1, Y + 2 + i);
                if (i == CurrentInstructionIndex)
                {
                    Console.BackgroundColor = Color;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                Console.Write(Rows[i].PadRight(MaxRowLength + 2));

                Console.ResetColor();
                Console.ForegroundColor = Color;
            }
            if (PrintTitle)
            {
                Graphics.PrintTitle(Title, X + 2, Y);
            }
        }
    }
}
