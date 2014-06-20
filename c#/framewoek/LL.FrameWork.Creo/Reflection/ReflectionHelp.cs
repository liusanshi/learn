using System;
using System.Text;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Diagnostics;
using System.Diagnostics.SymbolStore;
using System.Linq.Expressions;

namespace LL.FrameWork.Core.Reflection
{
    /// <summary>
    /// 反射帮助类
    /// </summary>
    public static class ReflectionHelper
    {
        #region decimal转换函数
        /// <summary>
        /// 数字类型 隐式转换为 decimal
        /// </summary>
        public readonly static Dictionary<TypeCode, MethodInfo> DecimalImplicitMethods = new Dictionary<TypeCode, MethodInfo>(13);
        /// <summary>
        /// decimal 显示转换为 其他数字类型
        /// </summary>
        public readonly static Dictionary<TypeCode, MethodInfo> DecimalExplicitMethods = new Dictionary<TypeCode, MethodInfo>(13);

        static ReflectionHelper()
        {
            //op_Implicit\op_Explicit
            var methods = typeof(decimal).GetMethods(BindingFlags.Static | BindingFlags.Public);
            foreach (var item in methods)
            {
                if (item.Name == "op_Implicit")
                {
                    DecimalImplicitMethods[Type.GetTypeCode(item.GetParameters()[0].ParameterType)] = item;
                }
                else if (item.Name == "op_Explicit")
                {
                    DecimalExplicitMethods[Type.GetTypeCode(item.ReturnType)] = item;
                }
            }
        }

        #endregion

        /// <summary>
        /// 字符串处理缓存
        /// </summary>
        private static readonly StringBuilder Stringbuff = new StringBuilder();
        
        /// <summary>
        /// 从表达式目录树中提取的属性元数据。
        /// 该 Lambda 表达式返回收到为一个表达式树的属性的值。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="propertyReturnsExpression"></param>
        /// <returns></returns>
        public static PropertyInfo ExtractPropertyInfo<T, TResult>(Expression<Func<T, TResult>> propertyReturnsExpression)
        {
            if (propertyReturnsExpression == null)
            {
                throw new ArgumentNullException("propertyReturnsExpression");
            }

            var body = propertyReturnsExpression.Body as MemberExpression;
            if (body == null)
            {
                throw new ArgumentException("propertyReturnsExpression Lambda表达式返回的值不是属性。");
            }

            var targetProperty = body.Member as PropertyInfo;
            if (targetProperty == null)
            {
                throw new ArgumentException("propertyReturnsExpression Lambda表达式返回的值不是属性。");
            }

            return targetProperty;
        }

        /// <summary>
        /// 获取成员的标识
        /// </summary>
        /// <param name="mi"></param>
        /// <returns></returns>
        public static string GetMemberSignName(MemberInfo mi)
        {
            Stringbuff.Length = 0;
            Stringbuff.Append(mi.DeclaringType.FullName).Append("@");
            switch (mi.MemberType)
            {
                case MemberTypes.Constructor:
                    Stringbuff.Append(mi.DeclaringType.Name);
                    GetParamsSignName(Stringbuff, ((ConstructorInfo)mi).GetParameters());
                    break;
                case MemberTypes.Method:
                    Stringbuff.Append(mi.Name);
                    GetParamsSignName(Stringbuff, ((MethodInfo)mi).GetParameters());
                    break;
                default:
                case MemberTypes.Custom:
                case MemberTypes.Event:
                case MemberTypes.NestedType:
                case MemberTypes.Field:
                case MemberTypes.Property:
                case MemberTypes.TypeInfo:
                    Stringbuff.Append(mi.Name);
                    break;
            }
            return Stringbuff.ToString();
        }

        private static void GetParamsSignName(StringBuilder sb, params ParameterInfo[] args)
        {
            if (args == null || args.Length == 0) return;
            foreach (var item in args)
            {
                sb.AppendFormat("-{0}", item.ParameterType.Name);
            }
        }

        /// <summary>
        /// 空类型
        /// </summary>
        public readonly static Type VoidType = typeof(void);
        /// <summary>
        /// object类型
        /// </summary>
        public readonly static Type ObjectType = typeof(object);
        /// <summary>
        /// object[]类型
        /// </summary>
        public readonly static Type ObjArrayType = typeof(object[]);

        /// <summary>
        /// 默认的方法标记
        /// </summary>
        public readonly static MethodAttributes DefaultMethodAttributes
            = MethodAttributes.HideBySig | MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.Final | MethodAttributes.NewSlot;

        /// <summary>
        /// 加载参数
        /// </summary>
        /// <param name="il"></param>
        /// <param name="index"></param>
        public static void ILLdarg(ILGenerator il, byte index)
        {
            switch (index)
            {
                case 0:
                    il.Emit(OpCodes.Ldarg_0);
                    break;
                case 1:
                    il.Emit(OpCodes.Ldarg_1);
                    break;
                case 2:
                    il.Emit(OpCodes.Ldarg_2);
                    break;
                case 3:
                    il.Emit(OpCodes.Ldarg_3);
                    break;
                default:
                    il.Emit(OpCodes.Ldarg, (int)index);
                    break;
            }
        }
        /// <summary>
        /// 加载整数
        /// </summary>
        /// <param name="il"></param>
        /// <param name="i"></param>
        public static void ILLdcInt(ILGenerator il, int i)
        {
            switch (i)
            {
                case -1:
                    il.Emit(OpCodes.Ldc_I4_M1);
                    break;
                case 0:
                    il.Emit(OpCodes.Ldc_I4_0);
                    break;
                case 1:
                    il.Emit(OpCodes.Ldc_I4_1);
                    break;
                case 2:
                    il.Emit(OpCodes.Ldc_I4_2);
                    break;
                case 3:
                    il.Emit(OpCodes.Ldc_I4_3);
                    break;
                case 4:
                    il.Emit(OpCodes.Ldc_I4_4);
                    break;
                case 5:
                    il.Emit(OpCodes.Ldc_I4_5);
                    break;
                case 6:
                    il.Emit(OpCodes.Ldc_I4_6);
                    break;
                case 7:
                    il.Emit(OpCodes.Ldc_I4_7);
                    break;
                case 8:
                    il.Emit(OpCodes.Ldc_I4_8);
                    break;
                default:
                    il.Emit(OpCodes.Ldc_I4, i);
                    break;
            }
        }

        /// <summary>
        /// 加载数组中的数据
        /// </summary>
        /// <param name="il"></param>
        /// <param name="index"></param>
        /// <param name="arrType"></param>
        public static void ILLdelem(ILGenerator il, byte index, Type arrType)
        {
            ILLdcInt(il, index);
            il.Emit(GetLdelemOpCode(Type.GetTypeCode(arrType)));
        }

        /// <summary>
        /// 抛出异常
        /// </summary>
        /// <param name="il"></param>
        /// <param name="exp"></param>
        public static void ILThrow<Exp>(ILGenerator il, string expmsg) where Exp : Exception
        {
            var expType = typeof(Exp);
            if (!string.IsNullOrEmpty(expmsg))
            {
                il.Emit(OpCodes.Ldstr, expmsg);
                il.Emit(OpCodes.Newobj, expType.GetConstructor(new Type[] { typeof(string) }));
            }
            else
            {
                il.Emit(OpCodes.Newobj, expType.GetConstructor(Type.EmptyTypes));
            }
            il.Emit(OpCodes.Throw);
        }

        /// <summary>
        /// 转换类型
        /// </summary>
        /// <param name="il"></param>
        /// <param name="fromType"></param>
        /// <param name="returnType"></param>
        public static void ILCastclass(ILGenerator il, Type fromType, Type returnType)
        {
            if (fromType != returnType)
            {
                if (!returnType.IsValueType)//是引用类型
                {
                    if (fromType.IsValueType)
                    {
                        il.Emit(OpCodes.Box, fromType);
                    }
                    else
                    {
                        if (fromType.IsAssignableFrom(returnType))
                            il.Emit(OpCodes.Castclass, returnType);
                    }
                }
                else //值类型
                {
                    var fromTypeCode = Type.GetTypeCode(fromType);
                    var returnTypeCode = Type.GetTypeCode(returnType);
                    //这里比较麻烦
                    if (returnTypeCode == TypeCode.Decimal) //返回Decimal
                    {
                        //call valuetype [mscorlib]System.Decimal [mscorlib]System.Decimal::op_Implicit(int32)
                        il.EmitCall(OpCodes.Call, DecimalImplicitMethods[fromTypeCode], null);

                    }
                    //这里比较麻烦
                    else if (fromTypeCode == TypeCode.Decimal) //传进来的是Decimal
                    {
                        //call int32 [mscorlib]System.Decimal::op_Explicit(valuetype [mscorlib]System.Decimal)
                        il.EmitCall(OpCodes.Call, DecimalExplicitMethods[returnTypeCode], null);
                    }
                    else
                    {
                        if (fromType.IsValueType)
                        {
                            il.Emit(GetConvOpCode(Type.GetTypeCode(returnType)));
                        }
                        else
                        {
                            il.Emit(OpCodes.Unbox_Any, returnType);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 将参数信息转换为参数类型数组
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static Type[] ConvertToType(ParameterInfo[] args)
        {
            if (args == null) return null;
            if (args.Length == 0) return Type.EmptyTypes;
            var result = new Type[args.Length];
            for (int i = 0; i < args.Length; i++)
            {
                result[i] = args[i].ParameterType;
            }
            return result;
        }

        /// <summary>
        /// 将参数根据类型捋顺
        /// </summary>
        /// <param name="args"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        public static object[] GetArgumentByType(object[] args, Type[] types)
        {
            if (types == null || types.Length == 0) return null;
            object[] result = new object[types.Length];
            args = args == null ? new object[0] : args;
            int i1 = 0;
            for (int i = 0; i < types.Length; i++)
            {
                do
                {
                    if (args.Length <= i1 || args[i1] == null)
                    {
                        result[i] = null;
                        i1++;
                        break;
                    }
                    if (args[i1].GetType() == types[i])
                    {
                        result[i] = args[i1];
                        i1++;
                        break;
                    }
                    i1++;

                } while (true);
            }
            return result;
        }

        #region OpCode
        /// <summary>
        /// 子类型之间的转化
        /// </summary>
        /// <param name="typeCode"></param>
        /// <returns></returns>
        private static OpCode GetConvOpCode(TypeCode typeCode)
        {
            switch (typeCode)
            {
                case TypeCode.Boolean:
                    return OpCodes.Conv_I1;

                case TypeCode.Char:
                    return OpCodes.Conv_I2;

                case TypeCode.SByte:
                    return OpCodes.Conv_I1;

                case TypeCode.Byte:
                    return OpCodes.Conv_U1;

                case TypeCode.Int16:
                    return OpCodes.Conv_I2;

                case TypeCode.UInt16:
                    return OpCodes.Conv_U2;

                case TypeCode.Int32:
                    return OpCodes.Conv_I4;

                case TypeCode.UInt32:
                    return OpCodes.Conv_U4;

                case TypeCode.Int64:
                    return OpCodes.Conv_I8;

                case TypeCode.UInt64:
                    return OpCodes.Conv_I8;

                case TypeCode.Single:
                    return OpCodes.Conv_R4;

                case TypeCode.Double:
                    return OpCodes.Conv_R8;
            }
            return OpCodes.Nop;
        }
        /// <summary>
        /// 加载数组里面的值
        /// </summary>
        /// <param name="typeCode"></param>
        /// <returns></returns>
        private static OpCode GetLdelemOpCode(TypeCode typeCode)
        {
            switch (typeCode)
            {
                case TypeCode.Object:
                case TypeCode.DBNull:
                    return OpCodes.Ldelem_Ref;

                case TypeCode.Boolean:
                    return OpCodes.Ldelem_I1;

                case TypeCode.Char:
                    return OpCodes.Ldelem_I2;

                case TypeCode.SByte:
                    return OpCodes.Ldelem_I1;

                case TypeCode.Byte:
                    return OpCodes.Ldelem_U1;

                case TypeCode.Int16:
                    return OpCodes.Ldelem_I2;

                case TypeCode.UInt16:
                    return OpCodes.Ldelem_U2;

                case TypeCode.Int32:
                    return OpCodes.Ldelem_I4;

                case TypeCode.UInt32:
                    return OpCodes.Ldelem_U4;

                case TypeCode.Int64:
                    return OpCodes.Ldelem_I8;

                case TypeCode.UInt64:
                    return OpCodes.Ldelem_I8;

                case TypeCode.Single:
                    return OpCodes.Ldelem_R4;

                case TypeCode.Double:
                    return OpCodes.Ldelem_R8;

                case TypeCode.String:
                    return OpCodes.Ldelem_Ref;
            }
            return OpCodes.Nop;
        }

        /// <summary>
        /// 根据值类型，返回使用哪个指令将值类型复制到计算堆栈上(ldobj指令的功能)
        /// <remark>abu 2007-10-16 11:49 AF043</remark>
        /// </summary>
        /// <param name="typeCode">The type code.</param>
        /// <returns></returns>
        private static OpCode GetLdindOpCode(TypeCode typeCode)
        {
            switch (typeCode)
            {
                case TypeCode.Boolean:
                    return OpCodes.Ldind_I1;

                case TypeCode.Char:
                    return OpCodes.Ldind_I2;

                case TypeCode.SByte:
                    return OpCodes.Ldind_I1;

                case TypeCode.Byte:
                    return OpCodes.Ldind_U1;

                case TypeCode.Int16:
                    return OpCodes.Ldind_I2;

                case TypeCode.UInt16:
                    return OpCodes.Ldind_U2;

                case TypeCode.Int32:
                    return OpCodes.Ldind_I4;

                case TypeCode.UInt32:
                    return OpCodes.Ldind_U4;

                case TypeCode.Int64:
                    return OpCodes.Ldind_I8;

                case TypeCode.UInt64:
                    return OpCodes.Ldind_I8;

                case TypeCode.Single:
                    return OpCodes.Ldind_R4;

                case TypeCode.Double:
                    return OpCodes.Ldind_R8;

                case TypeCode.String:
                    return OpCodes.Ldind_Ref;
            }
            return OpCodes.Nop;
        }

        /// <summary>
        /// 替换数组里面的值
        /// </summary>
        /// <param name="typeCode"></param>
        /// <returns></returns>
        private static OpCode GetStelemOpCode(TypeCode typeCode)
        {
            switch (typeCode)
            {
                case TypeCode.Object:
                case TypeCode.DBNull:
                    return OpCodes.Stelem_Ref;

                case TypeCode.Boolean:
                    return OpCodes.Stelem_I1;

                case TypeCode.Char:
                    return OpCodes.Stelem_I2;

                case TypeCode.SByte:
                    return OpCodes.Stelem_I1;

                case TypeCode.Byte:
                    return OpCodes.Stelem_I1;

                case TypeCode.Int16:
                    return OpCodes.Stelem_I2;

                case TypeCode.UInt16:
                    return OpCodes.Stelem_I2;

                case TypeCode.Int32:
                    return OpCodes.Stelem_I4;

                case TypeCode.UInt32:
                    return OpCodes.Stelem_I4;

                case TypeCode.Int64:
                    return OpCodes.Stelem_I8;

                case TypeCode.UInt64:
                    return OpCodes.Stelem_I8;

                case TypeCode.Single:
                    return OpCodes.Stelem_R4;

                case TypeCode.Double:
                    return OpCodes.Stelem_R8;

                case TypeCode.String:
                    return OpCodes.Stelem_Ref;
            }
            return OpCodes.Nop;
        }
        #endregion

        /// <summary>
        /// emit信息
        /// </summary>
        /// <returns></returns>
        public static ModuleBuilder EmitDebug()
        {
            AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName("DynamicMethod.debugger"), AssemblyBuilderAccess.RunAndSave);

            //定义调试信息
            CustomAttributeBuilder debugAttributeBuilder = new CustomAttributeBuilder(
                typeof(DebuggableAttribute).GetConstructor(new Type[] { typeof(DebuggableAttribute.DebuggingModes) }),
                new object[] { DebuggableAttribute.DebuggingModes.DisableOptimizations | DebuggableAttribute.DebuggingModes.Default }
            );
            assemblyBuilder.SetCustomAttribute(debugAttributeBuilder);

            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("MastzouBase.DynamicProxy.Proxy.dll", "MastzouBase.DynamicProxy.Proxy.dll", true);//关键这里要true
            //ISymbolDocumentWriter DOC = moduleBuilder.DefineDocument(@"d:\IL的cs源代码片断在这里.txt", Guid.Empty, Guid.Empty, Guid.Empty); //要定义源代码位置，这个文档不需要全部源代码，只需要你想调试的il源代码翻译就可以了
            //return DOC;
            return moduleBuilder;
        }
    }
}
