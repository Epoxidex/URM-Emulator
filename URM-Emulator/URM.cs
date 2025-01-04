using System;

namespace URM_Emulator
{
    public class URM
    {
        private const int TERMINATION_INSTRUCTION_ID = -1;
        private const int STARTING_NUMBER_OF_REGISTERS = 10;
        public Dictionary<int, int> Registers { get; private set; } = new Dictionary<int, int>();
        public List<string> Instructions { get; private set; } = new List<string>();

        public int CurrentInstructionId { get; private set; } = 0;

        public URM()
        {
            FillStartRegisters();
        }

        public void DeleteAllInstructions()
        {
            Instructions = new List<string>();
        }
        public void ResetRegisters()
        {
            Registers.Clear();
            FillStartRegisters();
        }

        private void FillStartRegisters()
        {
            for (int i = 1; i <= STARTING_NUMBER_OF_REGISTERS; i++)
                Registers[i] = 0;
        }
        public int GetRegisterValue(int index)
        {
            if (!Registers.ContainsKey(index)) 
                return 0;
            return Registers[index];
        }

        public void SetRegisterValue(int index, int value)
        {
            if (index < 1)
                throw new Exception("The register number is less than 1");
            if (value < 0)
                throw new Exception("The register value is less than 0");
            else 
                Registers[index] = value;
        }

        public void SetInstructions(string instructions)
        {
            Instructions = InstructionHelper.ValidateInstructions(instructions);
        }
        private void GoToNextInstuction()
        {
            CurrentInstructionId++;
        }

        public void ExecuteInstructions()
        {
            while (CurrentInstructionId != TERMINATION_INSTRUCTION_ID && CurrentInstructionId < Instructions.Count)
            {
                ExecuteInstruction(Instructions[CurrentInstructionId]);
            }
        }
        public void ExecuteInstruction(string instruction)
        {
            var terms = instruction.Split(' ');

            switch (terms[0].ToUpper())
            {
                case "Z": 
                    Z(int.Parse(terms[1]));
                    GoToNextInstuction();
                    break;

                case "S": 
                    S(int.Parse(terms[1]));
                    GoToNextInstuction();
                    break;

                case "M": 
                    M(int.Parse(terms[1]), int.Parse(terms[2]));
                    GoToNextInstuction();
                    break;

                case "J":
                    J(int.Parse(terms[1]), int.Parse(terms[2]), int.Parse(terms[3]));
                    break;
            }
        }
        public void Z(int index)
        {
            Registers[index] = 0;
        }

        public void S(int index)
        {
            if (!Registers.ContainsKey(index))
                Registers[index] = 1;
            else
                Registers[index]++;
        }

        public void M(int fromIndex, int toIndex)
        {
            if (!Registers.ContainsKey(fromIndex))
                Registers[fromIndex] = 0;
            if (!Registers.ContainsKey(toIndex))
                Registers[toIndex] = 0;
            
            Registers[toIndex] = Registers[fromIndex];
        }

        public void J(int index1, int index2, int instructionId)
        {
            if (!Registers.ContainsKey(index1))
                Registers[index1] = 0;
            if (!Registers.ContainsKey(index2))
                Registers[index2] = 0;

            if (Registers[index1] == Registers[index2])
                CurrentInstructionId = instructionId-1;
            else
                GoToNextInstuction();
        }
    }
}
