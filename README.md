# asmtest
Run IL assembly code directly from your program

```c#
//Create a delegate to hold the method signature
delegate byte WriteLine(string s);

//Get a reference to Console.WriteLine(string);
var wl_ref = typeof(System.Console).GetMethod("WriteLine", new Type[] { typeof(string) });

//Create the dynamic method
var Console_WriteLine = ILEngine.CreateMethod<WriteLine>(new Instruction(OpCodes.Ldarg_0), // argument 0 (string s)
                                                         new Instruction(OpCodes.Call, wl_ref), // call Console.WriteLine
                                                         new Instruction(OpCodes.Ret)); // return

//Now use it like you normally would
Console_WriteLine("Hello world!");