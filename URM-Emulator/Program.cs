using URM_Emulator;

URM urm = new URM();

urm.SetRegisterValue(1, 7);
urm.SetRegisterValue(2, 9);
urm.PrintRegisters();

string instructions =
    "J 2 3 0\n"
  + "S 3\n"
  + "S 1\n"
  + "J 1 1 1\n";

urm.SetInstructions(instructions);
urm.ExecuteInstructions();