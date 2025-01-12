namespace URM_Emulator
{
    /// <summary>
    /// Represents a Unlimited Register Machine (URM).
    /// </summary>
    public class URM
    {
        private const int TerminationInstructionId = -1;
        private const int DefaultRegisterCount = 10;

        public Dictionary<int, int> Registers { get; private set; }
        public List<string> Instructions { get; private set; }
        public int CurrentInstructionId { get; private set; }

        public URM()
        {
            Registers = new Dictionary<int, int>();
            Instructions = new List<string>();
            ResetRegisters();
            GoToFirstInstruction();
        }

        /// <summary>
        /// Clears all loaded instructions.
        /// </summary>
        public void DeleteAllInstructions() => Instructions.Clear();

        /// <summary>
        /// Resets all registers to default values.
        /// </summary>
        public void ResetRegisters()
        {
            Registers.Clear();
            for (int i = 1; i <= DefaultRegisterCount; i++)
            {
                Registers[i] = 0;
            }
        }

        /// <summary>
        /// Gets the value of a specific register.
        /// </summary>
        public int GetRegisterValue(int index)
        {
            if (index < 1) throw new ArgumentOutOfRangeException(nameof(index), "Register number must be greater than 0.");
            return Registers.TryGetValue(index, out int value) ? value : 0;
        }

        /// <summary>
        /// Sets the value of a specific register.
        /// </summary>
        public void SetRegisterValue(int index, int value)
        {
            if (index < 1) throw new ArgumentOutOfRangeException(nameof(index), "Register number must be greater than 0.");
            if (value < 0) throw new ArgumentOutOfRangeException(nameof(value), "Register value must be non-negative.");

            Registers[index] = value;
        }

        /// <summary>
        /// Sets multiple register values at once.
        /// </summary>
        public void SetRegistersValues(Dictionary<int, int> values)
        {
            if (values.Keys.Any(key => key < 1)) throw new ArgumentException("All register numbers must be greater than 0.");
            if (values.Values.Any(value => value < 0)) throw new ArgumentException("All register values must be non-negative.");

            foreach (var pair in values)
            {
                Registers[pair.Key] = pair.Value;
            }
        }

        /// <summary>
        /// Loads and validates instructions.
        /// </summary>
        public void SetInstructions(string instructions)
        {
            if (string.IsNullOrWhiteSpace(instructions)) throw new ArgumentException("Instructions cannot be null or empty.");
            Instructions = InstructionHelper.ValidateInstructions(instructions);
        }

        /// <summary>
        /// Moves to the next instruction.
        /// </summary>
        public void GoToNextInstruction() => CurrentInstructionId++;

        /// <summary>
        /// Resets instruction pointer to the first instruction.
        /// </summary>
        public void GoToFirstInstruction() => CurrentInstructionId = 0;

        /// <summary>
        /// Executes all loaded instructions.
        /// </summary>
        public void ExecuteInstructions()
        {
            while (CurrentInstructionId != TerminationInstructionId && CurrentInstructionId < Instructions.Count)
            {
                ExecuteInstruction(Instructions[CurrentInstructionId]);
            }
            GoToFirstInstruction();
        }

        /// <summary>
        /// Executes a single instruction.
        /// </summary>
        public void ExecuteInstruction(string instruction)
        {
            var terms = instruction.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            switch (terms[0].ToUpper())
            {
                case "Z":
                    Z(ParseRegister(terms[1]));
                    GoToNextInstruction();
                    break;

                case "S":
                    S(ParseRegister(terms[1]));
                    GoToNextInstruction();
                    break;

                case "M":
                    M(ParseRegister(terms[1]), ParseRegister(terms[2]));
                    GoToNextInstruction();
                    break;

                case "J":
                    J(ParseRegister(terms[1]), ParseRegister(terms[2]), ParseInstructionId(terms[3]));
                    break;

                default:
                    throw new InvalidOperationException($"Unknown instruction: {terms[0]}");
            }
        }

        private int ParseRegister(string input)
        {
            if (!int.TryParse(input, out int index) || index < 1)
                throw new FormatException($"Invalid register number: {input}");
            return index;
        }

        private int ParseInstructionId(string input)
        {
            if (!int.TryParse(input, out int id) || id < 0)
                throw new FormatException($"Invalid instruction ID: {input}");
            return id;
        }

        private void Z(int index) => Registers[index] = 0;

        private void S(int index) => Registers[index] = Registers.GetValueOrDefault(index, 0) + 1;

        private void M(int fromIndex, int toIndex) => Registers[toIndex] = Registers.GetValueOrDefault(fromIndex, 0);

        private void J(int index1, int index2, int instructionId)
        {
            if (Registers.GetValueOrDefault(index1) == Registers.GetValueOrDefault(index2))
            {
                CurrentInstructionId = instructionId - 1;
            }
            else
            {
                GoToNextInstruction();
            }
        }
    }
}
