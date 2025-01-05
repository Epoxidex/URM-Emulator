namespace URM_Emulator.Managers
{
    public class InstructionManager
    {

        private readonly URM _urm;

        public InstructionManager(URM urm)
        {
            _urm = urm;
        }

        public void LoadInstructionsFromString(string instructions) => _urm.SetInstructions(instructions);

        //TODO remove
        public string GetInstructionsFromFile(string path)
        {
            if (path.Trim().ToLower() == "x") return "";

            string text = File.ReadAllText(path);
            try
            {
                _urm.SetInstructions(text);
                return "Instructions added.";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

    }
}
