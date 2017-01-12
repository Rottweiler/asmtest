using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace asmtest
{

    /*
     *  Created by Rottweiler ( http://github.com/Rottweiler )
     * 
     *  Copyright 2017 Rottweiler
     *  This file is part of asmtest.
     *  asmtest is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version
     *  asmtest is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
     *  You should have received a copy of the GNU General Public License along with asmtest. If not, see http://www.gnu.org/licenses/.
     * 
     *  Basically, use as you'd like, as long as you leave the credits.
     * 
     */

    /// <summary>
    /// ILEngine
    /// </summary>
    public class ILEngine
    {
        private static Type[] GetParameterTypes<T>()
        {
            List<Type> types = new List<Type>();
            foreach (ParameterInfo par in typeof(T).GetMethod("Invoke").GetParameters())
            {
                types.Add(par.ParameterType);
            }
            return types.ToArray();
        }

        public static T CreateMethod<T>(params Instruction[] instructions)
        {
            var dynamic_method = new DynamicMethod("", typeof(T).GetMethod("Invoke").ReturnType, GetParameterTypes<T>(), typeof(ILEngine).Module);
            ILGenerator gen = dynamic_method.GetILGenerator();

            foreach (Instruction inst in instructions)
            {
                if (inst.Operand == null)
                {
                    gen.Emit(inst.OpCode);
                }
                else
                {
                    /* CALL */
                    if (inst.OpCode == OpCodes.Call)
                    {
                        gen.EmitCall(OpCodes.Call, (MethodInfo)inst.Operand, null);
                        continue;
                    }

                    /*if(inst.OpCode == OpCodes.Calli)
                    {
                        gen.EmitCalli(OpCodes.Calli, (MethodInfo)inst.Operand, null); //CallingConvention.StdCall, T, new Type[] { T, T }
                        continue;
                    }*/

                    //gen.EmitWriteLine();

                    /* TYPES */
                    if (inst.Operand.GetType() == typeof(LocalBuilder))
                    {
                        var value = (LocalBuilder)inst.Operand;
                        gen.Emit(inst.OpCode, value);
                        continue;
                    }

                    if (inst.Operand.GetType() == typeof(FieldInfo))
                    {
                        var value = (FieldInfo)inst.Operand;
                        gen.Emit(inst.OpCode, value);
                        continue;
                    }

                    if (inst.Operand.GetType() == typeof(Label[]))
                    {
                        var value = (Label[])inst.Operand;
                        gen.Emit(inst.OpCode, value);
                        continue;
                    }

                    if (inst.Operand.GetType() == typeof(Label))
                    {
                        var value = (Label)inst.Operand;
                        gen.Emit(inst.OpCode, value);
                        continue;
                    }

                    if (inst.Operand.GetType() == typeof(string))
                    {
                        var value = (string)inst.Operand;
                        gen.Emit(inst.OpCode, value);
                        continue;
                    }

                    if (inst.Operand.GetType() == typeof(float))
                    {
                        var value = (float)inst.Operand;
                        gen.Emit(inst.OpCode, value);
                        continue;
                    }

                    if (inst.Operand.GetType() == typeof(byte))
                    {
                        var value = (byte)inst.Operand;
                        gen.Emit(inst.OpCode, value);
                        continue;
                    }

                    if (inst.Operand.GetType() == typeof(sbyte))
                    {
                        var value = (sbyte)inst.Operand;
                        gen.Emit(inst.OpCode, value);
                        continue;
                    }

                    if (inst.Operand.GetType() == typeof(double))
                    {
                        var value = (double)inst.Operand;
                        gen.Emit(inst.OpCode, value);
                        continue;
                    }

                    if (inst.Operand.GetType() == typeof(int))
                    {
                        var value = (int)inst.Operand;
                        gen.Emit(inst.OpCode, value);
                        continue;
                    }

                    if (inst.Operand.GetType() == typeof(MethodInfo))
                    {
                        var value = (MethodInfo)inst.Operand;
                        gen.Emit(inst.OpCode, value);
                        continue;
                    }

                    if (inst.Operand.GetType() == typeof(short))
                    {
                        var value = (short)inst.Operand;
                        gen.Emit(inst.OpCode, value);
                        continue;
                    }

                    if (inst.Operand.GetType() == typeof(SignatureHelper))
                    {
                        var value = (SignatureHelper)inst.Operand;
                        gen.Emit(inst.OpCode, value);
                        continue;
                    }

                    if (inst.Operand.GetType() == typeof(ConstructorInfo))
                    {
                        var value = (ConstructorInfo)inst.Operand;
                        gen.Emit(inst.OpCode, value);
                        continue;
                    }

                    if (inst.Operand.GetType() == typeof(Type))
                    {
                        var value = (Type)inst.Operand;
                        gen.Emit(inst.OpCode, value);
                        continue;
                    }

                    if (inst.Operand.GetType() == typeof(long))
                    {
                        var value = (long)inst.Operand;
                        gen.Emit(inst.OpCode, value);
                        continue;
                    }
                }

            }

            return (T)(dynamic_method.CreateDelegate(typeof(T)) as object);
        }
    }

    /// <summary>
    /// Instruction
    /// </summary>
    public class Instruction
    {
        public OpCode OpCode { get; set; }
        public object Operand { get; set; }

        public Instruction(OpCode op_code, object operand)
        {
            OpCode = op_code;
            Operand = operand;

            //todo check valid operand type
        }

        public Instruction(OpCode op_code)
        {
            OpCode = op_code;
        }

        public static Instruction Create(OpCode op_code)
        {
            return new Instruction(op_code);
        }

        public static Instruction Create(OpCode op_code, object operand)
        {
            return new Instruction(op_code, operand);
        }

        public Instruction() { }
    }
}
