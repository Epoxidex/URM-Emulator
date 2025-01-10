using URM_Emulator.ConsoleGraphics;
using URM_Emulator.ConsoleGraphics.Forms;
using URM_Emulator.Managers;

namespace URM_Emulator
{
    public class ConsoleIO
    {
        private readonly URM _urm;
        private readonly RegisterManager _registerManager;
        private readonly InstructionManager _instructionManager;
        private readonly ProgramManager _programManager;

        public ConsoleIO(URM urm)
        {
            _urm = urm;
            _registerManager = new RegisterManager(urm);
            _instructionManager = new InstructionManager(urm);
            _programManager = new ProgramManager(urm, _registerManager, _instructionManager);

            Console.CursorVisible = false;
        }

        public void Run()
        {
            while (true)
            {
                Console.Clear();
                int option = ShowMainMenu();
                Console.Clear();
                switch (option)
                {
                    case 0:
                        RunProgramEditor();
                        break;
                    case 1:
                        StepByStepExecution();
                        break;
                    case 2:
                        RunExampleLoader();
                        break;
                    case 3:
                        return;
                }
            }
        }

        private int ShowMainMenu()
        {
            string[] menuItems =
            {
                " Edit program ",
                " Execute program ",
                " Load example of program ",
                " Exit "
            };

            return SelectMenuOption(menuItems, new MenuForm(2, 2, "Choose option", menuItems, border: true, ConsoleColor.Green));
        }

        private int SelectMenuOption(string[] options, MenuForm menuForm)
        {
            int selectedIndex = 0;
            ConsoleKey key;

            do
            {
                menuForm.Show();
                key = Console.ReadKey().Key;

                if (key == ConsoleKey.UpArrow)
                    selectedIndex = (selectedIndex - 1 + options.Length) % options.Length;
                else if (key == ConsoleKey.DownArrow)
                    selectedIndex = (selectedIndex + 1) % options.Length;

                menuForm.SelectedIndex = selectedIndex;

            } while (key != ConsoleKey.Enter);

            return selectedIndex;
        }

        private void RunExampleLoader()
        {
            string[] examples =
            {
                "Sum of two numbers | R1+R2",
                "Maximum of three numbers | max(R1, R2, R3)"
            };

            int selectedIndex = SelectMenuOption(examples, new MenuForm(2, 2, "Examples", examples, true, ConsoleColor.Cyan));

            try
            {
                string filePath = selectedIndex switch
                {
                    0 => "Examples\\Sum of two numbers.txt",
                    1 => "Examples\\Maximum of three numbers.txt",
                    _ => throw new InvalidOperationException()
                };

                _programManager.LoadProgramFromFile(filePath);
                ShowMessage("Program Loaded!", ConsoleColor.Green);
            }
            catch (Exception ex)
            {
                ShowMessage($"Error loading file: {ex.Message}", ConsoleColor.Red);
            }
        }

        private void RunProgramEditor()
        {
            string[] editorOptions =
            {
                " Set register value ",
                " Reset all registers ",
                " Choose instructions from file ",
                " Back "
            };

            while (true)
            {
                RenderCurrentState();
                int selectedIndex = SelectMenuOption(editorOptions, new MenuForm(2, 2, "Choouse option", editorOptions, true, ConsoleColor.Green));

                switch (selectedIndex)
                {
                    case 0:
                        ShowMessage(_registerManager.EnterRegisters(), ConsoleColor.Green);
                        break;
                    case 1:
                        _urm.ResetRegisters();
                        ShowMessage("Registers reset.", ConsoleColor.Green);
                        break;
                    case 2:
                        ShowMessage("Feature not implemented yet.", ConsoleColor.Yellow);
                        break;
                    case 3:
                        return;
                }
            }
        }

        private void StepByStepExecution()
        {
            var oldRegisters = new Dictionary<int, int>(_urm.Registers);

            var oldRegistersForm = new RegistersForm(30, 2, "Registers", _urm.Registers);
            var newRegistersForm = new RegistersForm(30, 8, "Registers", _urm.Registers);

            var k = 1;
            var instructionsForm = new InstructionsForm(2, 2, "Instructions", _urm.Instructions.Select(i => $" [{k++}] {i.ToString()} ").ToArray());
            instructionsForm.CurrentInstructionIndex = 0;

            while (_urm.CurrentInstructionId < _urm.Instructions.Count)
            {
                Console.Clear();
                Console.Write("\x1b[3J");

                if (_urm.CurrentInstructionId >= _urm.Instructions.Count || _urm.CurrentInstructionId < 0)
                {
                    Console.WriteLine("Program completed\n");
                    break;
                }

                instructionsForm.Show();

                //Console.Write($"Current Instruction (#{_urm.CurrentInstructionId + 1}): ");
                //RenderManager.ColoredWriteLine($"{_urm.Instructions[_urm.CurrentInstructionId]}", ConsoleColor.Yellow);

                var previousRegisters = new Dictionary<int, int>(_urm.Registers);
                _urm.ExecuteInstruction(_urm.Instructions[_urm.CurrentInstructionId]);
                var changedRegisters = _registerManager.GetChangedRegisters(previousRegisters, _urm.Registers);

                //if (changedRegisters != null)
                //{
                //    Console.WriteLine("Changed registers: ");
                //    foreach (var reg in changedRegisters)
                //    {
                //        RenderManager.ColoredWriteLine($"R{reg}: {(previousRegisters.ContainsKey(reg) ? previousRegisters[reg] : 0)} -> {_urm.Registers[reg]}", ConsoleColor.Blue);
                //    }
                //}
                //Console.WriteLine();

                oldRegistersForm.Registers = previousRegisters;
                oldRegistersForm.ChangedValues = changedRegisters;
                oldRegistersForm.Show();
                newRegistersForm.ChangedValues = changedRegisters;
                newRegistersForm.Show();

                //Console.WriteLine("\nPress 'Enter' to continue, 'q' to quit step-by-step mode and complete program.");
                var key = Console.ReadKey();

                if (key.Key == ConsoleKey.Q)
                {
                    _urm.ExecuteInstructions();
                    break;
                }
                instructionsForm.CurrentInstructionIndex = _urm.CurrentInstructionId;
            }

            _urm.GoToFirstInstruction();

            //Console.Clear();
            //Console.WriteLine("Program completed");
            //Console.WriteLine("Old registers:");
            //RenderManager.PrintRegisters(oldRegisters);
            //Console.WriteLine("New registers:");
            //RenderManager.PrintRegisters(_urm.Registers);

            //string input;
            //do
            //{
            //    Console.WriteLine("\nSave new register values? [y/n]");
            //    input = Console.ReadLine().Trim().ToLower();
            //} while (input != "y" && input != "n");


            //if (input == "n")
            //    _urm.SetRegistersValues(oldRegisters);
        }

        private void RenderCurrentState()
        {
            var registersForm = new RegistersForm(2, 8, "Registers", _urm.Registers);
            registersForm.Show();

            var k = 1;
            var instructionsForm = new InstructionsForm(2, 13, "Instructions", _urm.Instructions.Select(i => $" [{k++}] {i.ToString()} ").ToArray());
            instructionsForm.Show();
        }

        private void ShowMessage(string message, ConsoleColor color)
        {
            Console.Clear();
            var resultForm = new Form(2, 2, "Message", new[] { message }, true, color);
            resultForm.Show();
            Console.ReadKey();
        }
    }
}
