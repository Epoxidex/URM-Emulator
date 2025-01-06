namespace URM_Emulator.Managers
{
    public class ProgramManager
    {

        private readonly URM _urm;
        private readonly RegisterManager _registerManager;
        private readonly InstructionManager _instructionManager;

        public ProgramManager(URM urm, RegisterManager registerManager, InstructionManager instructionManager)
        {
            _urm = urm;
            _registerManager = registerManager;
            _instructionManager = instructionManager;
        }

        public void LoadProgramFromFile(string path)
        {
            string[] lines = File.ReadAllLines(path);

            if (lines.Length > 0)
            {
                string registers = lines[0].Trim();

                string instructions = string.Join(Environment.NewLine, lines, 1, lines.Length - 1).Trim();

                _registerManager.LoadRegistersFromString(registers);
                _instructionManager.LoadInstructionsFromString(instructions);
            }
            else
            {
                throw new FormatException("Invalid data format");
            }
        }
    }
}
