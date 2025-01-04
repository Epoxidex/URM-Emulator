namespace URM_Emulator
{
    public static class InstructionHelper
    {

        public static string ValidateInstruction(string instruction)
        {
            instruction = instruction.Trim().ToUpper();
            var args = instruction.Split(' ').Skip(1);

            while (instruction.Contains("  "))
                instruction = instruction.Replace("  ", " ");

            switch (instruction[0])
            {
                case 'Z':
                    if (args.Count() != 1) throw new Exception("Too many arguments for Z");
                    break;

                case 'S':
                    if (args.Count() != 1) throw new Exception("Too many arguments for S");
                    break;

                case 'M':
                    if (args.Count() != 2) throw new Exception("Too many arguments for M");
                    break;

                case 'J':
                    if (args.Count() != 3) throw new Exception("Too many arguments for J");
                    break;

                default:
                    throw new Exception("Unknown instruction");
            }

            try
            {
                var intArgs = args.Select(x => int.Parse(x));
                if (intArgs.Any(x => x < 0)) 
                    throw new Exception("Invalid arguments");
            }
            catch
            {
                throw new Exception("Invalid arguments");
            }

            return instruction;
        }

        public static List<string> ValidateInstructions(string instructions)
        {
            while (instructions.Contains("\n\n"))
                instructions = instructions.Replace("\n\n", "\n");

            return instructions.Split("\n").Select(x => ValidateInstruction(x)).ToList();
        }
    }
}
