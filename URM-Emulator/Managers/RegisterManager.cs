using System.Text.RegularExpressions;

namespace URM_Emulator.Managers
{
    public class RegisterManager
    {

        private readonly URM _urm;

        public RegisterManager(URM urm)
        {
            _urm = urm;
        }

        public List<int> LoadRegisterFromString(string registerData, char sep = ':')
        {
            if (string.IsNullOrEmpty(registerData))
            {
                throw new ArgumentNullException(nameof(registerData), "Input string cannot be null or empty.");
            }

            string[] parts = registerData.Split(sep);
            if (parts.Length != 2)
            {
                throw new FormatException("Invalid input string format. Expected format is 'register_number:value'.");
            }

            if (!int.TryParse(parts[0].Trim(), out int registerNumber))
            {
                throw new FormatException("Invalid register number format. Expected a positive integer.");
            }

            if (!int.TryParse(parts[1].Trim(), out int registerValue))
            {
                throw new FormatException("Invalid register value format. Expected a non-negative integer.");
            }

            _urm.SetRegisterValue(registerNumber, registerValue);
            return new List<int>([registerNumber, registerValue]);
        }

        public List<List<int>> LoadRegistersFromString(string input, char sep = ':')
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentNullException(nameof(input), "Input string cannot be null or empty.");
            }

            string normalizedInput = Regex.Replace(input, @"\s+", " ").Trim();

            var result = new List<List<int>>();
            foreach (string registerData in normalizedInput.Split(' '))
            {
                result.Add(LoadRegisterFromString(registerData, sep: sep));
            }
            return result;
        }

        public string EnterRegisters()
        {
            Console.Write("\nEnter register number and value in the following format: ");
            Console.WriteLine("register_number:value");
            Console.WriteLine("For example: 1:42, to set the value 42 in register R1.");
            Console.Write(" > ");
            try
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                var regData = LoadRegisterFromString(Console.ReadLine());
                return $"The register R{regData[0]} is set to a value '{regData[1]}'";
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
