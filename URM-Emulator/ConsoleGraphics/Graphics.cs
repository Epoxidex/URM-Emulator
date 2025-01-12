﻿using URM_Emulator.ConsoleGraphics.Forms;

namespace URM_Emulator.ConsoleGraphics
{
    public static class Graphics
    {
        public static void DrawBorder(int x, int y, string[] rows, string title)
        {
            int width = GetMaxWidth(rows, title) + 4;
            int height = rows.Length + 2;

            Console.SetCursorPosition(x, y);
            Console.Write("╔" + new string('═', width - 2) + "╗");

            for (int i = 0; i < height - 2; i++)
            {
                Console.SetCursorPosition(x, y + 1 + i);
                Console.Write("║" + new string(' ', width - 2) + "║");
            }

            Console.SetCursorPosition(x, y + height - 1);
            Console.Write("╚" + new string('═', width - 2) + "╝");

            PrintTitle(title, x+2, y);

        }
        private static void PrintTitle(string title, int x, int y)
        {
            Console.SetCursorPosition(x, y);
            Console.Write($"╣{title}╠");
        }
        private static int GetMaxWidth(string[] rows, string title)
        {
            return Math.Max(rows.Max(row => row.Length), title.Length);
        }
    }

}