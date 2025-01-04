using System;

namespace URM_Emulator
{
    public class URM
    {
        private Dictionary<int, int> _registers = new Dictionary<int, int>();
        private List<string> _instructions = new List<string>();

        public int _currentInstructionId = 0;
        public void Reset()
        {
            _registers.Clear();
        }
        public int GetRegisterValue(int index)
        {
            if (!_registers.ContainsKey(index)) 
                return 0;
            return _registers[index];
        }

        public void SetRegisterValue(int index, int value)
        {
            if (value < 0)
                throw new Exception("The register value is less than 0");
            else 
                _registers[index] = value;
        }

        private void GoToNextInstuction()
        {
            _currentInstructionId++;
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
                
                default:
                    throw new Exception("Unknown instruction");
            }
        }
        public void Z(int index)
        {
            _registers[index] = 0;
        }

        public void S(int index)
        {
            if (!_registers.ContainsKey(index))
                _registers[index] = 1;
            else
                _registers[index]++;
        }

        public void M(int fromIndex, int toIndex)
        {
            if (!_registers.ContainsKey(fromIndex))
                _registers[fromIndex] = 0;
            if (!_registers.ContainsKey(toIndex))
                _registers[fromIndex] = 0;
            
            _registers[toIndex] = _registers[fromIndex];
        }

        public void J(int index1, int index2, int instructionId)
        {
            if (!_registers.ContainsKey(index1))
                _registers[index1] = 0;
            if (!_registers.ContainsKey(index2))
                _registers[index2] = 0;

            if (_registers[index1] == _registers[index2])
                _currentInstructionId = instructionId;
            else
                _currentInstructionId++;
        }

        public void PrintRegisters()
        {
            foreach (var value in _registers.Keys.ToList())
            {
                Console.Write(_registers[value].ToString()+" ");
            }
            Console.WriteLine();
        }
    }
}
