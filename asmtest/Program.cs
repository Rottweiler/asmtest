using System;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace asmtest
{
    class Program
    {

        static void Main(string[] args)
        {
            /* Test: Xor encryption */
            byte[] msg = Encoding.UTF8.GetBytes("Hello world");
            byte[] enc = XorData(msg, new byte[] { 0x3D, 0x40 }); //encrypt
            enc = XorData(enc, new byte[] { 0x3D, 0x40 }); //decrypt
            Console.WriteLine(Encoding.UTF8.GetString(enc)); //print

            /* Test: Assembly.Load(byte[]) */
            var asm = Assembly_load(File.ReadAllBytes("asmtest.exe"));
            Console.WriteLine(asm.EntryPoint.Name.ToString());

            Console.ReadLine();
        }


        /*
         * Assembly.Load
         */
        delegate Assembly Load(byte[] data);
        static Assembly Assembly_load(byte[] data)
        {
            Load load = ILEngine.CreateMethod<Load>(new Instruction(OpCodes.Ldarg_0), 
                                                    new Instruction(OpCodes.Call, typeof(Assembly).GetMethod("Load", new Type[] { typeof(byte[]) })),
                                                    new Instruction(OpCodes.Ret));
            return load(data);
        }

        /*
         * Xor encryption
         */
        delegate byte Xor(byte x, byte y);
        delegate int Mod(int x, int y);

        static byte[] XorData(byte[] data, byte[] key)
        {
            Xor xor = ILEngine.CreateMethod<Xor>(new Instruction(OpCodes.Ldarg_0),
                                                 new Instruction(OpCodes.Ldarg_1),
                                                 new Instruction(OpCodes.Xor),
                                                 new Instruction(OpCodes.Ret));

            Mod mod = ILEngine.CreateMethod<Mod>(new Instruction(OpCodes.Ldarg_0),
                                                 new Instruction(OpCodes.Ldarg_1),
                                                 new Instruction(OpCodes.Rem),
                                                 new Instruction(OpCodes.Ret));

            byte[] enc = data.Clone() as byte[];
            for (int i = 0; i < enc.Length; i++)
            {
                enc[i] = xor(enc[i], key[mod(i, key.Length)]);
            }
            return enc;
        }
    }
}