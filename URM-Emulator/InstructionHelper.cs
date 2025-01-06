namespace URM_Emulator
{
    public static class InstructionHelper
    {
        /// <summary>
        /// Validates a single URM instruction.
        /// </summary>
        /// <param name="instruction">The instruction to validate.</param>
        /// <returns>The validated instruction in uppercase.</returns>
        public static string ValidateInstruction(string instruction)
        {
            if (string.IsNullOrWhiteSpace(instruction))
                throw new ArgumentException("Instruction cannot be null or empty.");

            // Normalize instruction format
            instruction = string.Join(" ", instruction.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries)).ToUpper();
            var parts = instruction.Split(' ');
            var command = parts[0];
            var args = parts.Skip(1).ToArray();

            // Validate command and argument count
            int expectedArgs = command switch
            {
                "Z" => 1,
                "S" => 1,
                "M" => 2,
                "J" => 3,
                _ => throw new FormatException($"Unknown instruction: {command}")
            };

            if (args.Length != expectedArgs)
                throw new FormatException($"Instruction {command} expects {expectedArgs} arguments, but got {args.Length}.");

            // Validate arguments as non-negative integers
            if (!args.All(arg => int.TryParse(arg, out int value) && value >= 0))
                throw new FormatException($"Instruction {command} contains invalid or negative arguments.");

            return instruction;
        }

        /// <summary>
        /// Validates a series of URM instructions.
        /// </summary>
        /// <param name="instructions">The multiline string containing instructions.</param>
        /// <returns>A list of validated instructions.</returns>
        public static List<string> ValidateInstructions(string instructions)
        {
            if (string.IsNullOrWhiteSpace(instructions))
                throw new ArgumentException("Instructions cannot be null or empty.");

            return instructions
                .Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(ValidateInstruction)
                .ToList();
        }
    }
}
