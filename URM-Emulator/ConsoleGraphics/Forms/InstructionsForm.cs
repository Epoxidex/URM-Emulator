
namespace URM_Emulator.ConsoleGraphics.Forms
{
    public class InstructionsForm : Form
    {
        public int CurrentInstructionIndex { get; set; } = -1;

        public InstructionsForm(int x, int y, string title, string[] instructions, ConsoleColor color = ConsoleColor.Yellow)
            : base(x, y, title, instructions, true, color)
        {

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
                Console.SetCursorPosition(X + 1, Y + 1 + i);
                if (i == CurrentInstructionIndex)
                {
                    Console.BackgroundColor = Color;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                Console.Write(Rows[i]);

                Console.ResetColor();
                Console.ForegroundColor = Color;
            }
        }
    }
}
