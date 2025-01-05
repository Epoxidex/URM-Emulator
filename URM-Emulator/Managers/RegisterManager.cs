namespace URM_Emulator.Managers
{
    public class RegisterManager
    {

        private readonly URM _urm;

        public RegisterManager(URM urm)
        {
            _urm = urm;
        }

        public string EnterRegisters()
        {
            Console.Write("\nEnter register number and value in the following format: ");
            RenderManager.ColoredWriteLine("register_number:value", ConsoleColor.Yellow);
            Console.WriteLine("For example: 1:42, to set the value 42 in register R1.");
            Console.Write(" > ");
            try
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                var input = Console.ReadLine().Split(':').Select(s => int.Parse(s.Trim())).ToList();
                _urm.SetRegisterValue(input[0], input[1]);
                return $"The register R{input[0]} is set to a value '{input[1]}'";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }

        public HashSet<int> GetChangedRegisters(Dictionary<int, int> regs1, Dictionary<int, int> regs2)
        {
            HashSet<int> changedKeys = new HashSet<int>();

            foreach (var keyValuePair in regs1)
            {
                int key = keyValuePair.Key;
                if (!regs2.TryGetValue(key, out int value2) || keyValuePair.Value != value2)
                {
                    changedKeys.Add(key);
                }
            }

            foreach (var keyValuePair in regs2)
            {
                int key = keyValuePair.Key;
                if (!regs1.ContainsKey(key))
                {
                    changedKeys.Add(key);
                }
            }

            return changedKeys;
        }

    }
}
