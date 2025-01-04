using System.Text;
using URM_Emulator;

URM urm = new URM();

urm.SetRegisterValue(1, 7);
urm.SetRegisterValue(2, 9);
urm.SetRegisterValue(6, 6);
urm.SetRegisterValue(9223, 1);

string instructions =
    "J 2 3 0\n"
  + "S 3\n"
  + "S 1\n"
  + "J 1 1 1\n";

urm.SetInstructions(instructions);

var registers = urm.Registers;

ConsoleIO io = new ConsoleIO(urm);
io.Run();

//ConsoleIO consoleIO = new ConsoleIO(urm);
//consoleIO.Run();

//urm.SetRegisterValue(1, 7);
//urm.SetRegisterValue(2, 9);
//urm.PrintRegisters();
//urm.ExecuteInstructions();