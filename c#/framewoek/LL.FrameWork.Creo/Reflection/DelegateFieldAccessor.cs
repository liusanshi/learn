using System;
using System.Diagnostics.SymbolStore;
using System.Reflection;
using System.Reflection.Emit;

namespace LL.FrameWork.Core.Reflection
{
    public class DelegateFieldAccessor : IFieldAccessor
    {
        private Func<object, object> FieldGet;
        private Action<object, object> FieldSet;
        public DelegateFieldAccessor(FieldInfo field)
        {
            init(field);
        }

        void init(FieldInfo field)
        {
            if (field == null)
            {
                throw new ArgumentException("Argument: field is null");
            }
            if (!field.IsPublic)
            {
                FieldGet = obj => field.GetValue(obj);
                FieldSet = (target, val) => field.SetValue(target, val);
                return;
            }
            var name = ReflectionHelp.GetMemberSignName(field);
            var type = field.DeclaringType;

#if DEBUG1
            ModuleBuilder moduleBuilder = ReflectionHelp.EmitDebug();
            ISymbolDocumentWriter doc = moduleBuilder.DefineDocument(@"d:\IL的cs源代码片断在这里.txt", Guid.Empty, Guid.Empty, Guid.Empty); //要定义源代码位置，这个文档不需要全部源代码，只需要你想调试的il源代码翻译就可以了

            DynamicMethod method = new DynamicMethod(name + "_get", ReflectionHelp.ObjectType, new Type[] { ReflectionHelp.ObjectType }, moduleBuilder);
#else
            DynamicMethod method = new DynamicMethod(name + "_get", ReflectionHelp.ObjectType, new Type[] { ReflectionHelp.ObjectType });
#endif

            var il = method.GetILGenerator();
            ReflectionHelp.ILLdarg(il, 0);
            ReflectionHelp.ILCastclass(il, ReflectionHelp.ObjectType, type);
            il.Emit(OpCodes.Ldfld, field);
            ReflectionHelp.ILCastclass(il, field.FieldType, ReflectionHelp.ObjectType);

#if DEBUG1
            il.MarkSequencePoint(doc, 1, 0, 1, 100);//DynamicMethod不支持
#endif

            il.Emit(OpCodes.Ret);
            FieldGet = (Func<object, object>)method.CreateDelegate(typeof(Func<object, object>));

            method = new DynamicMethod(name + "_set", ReflectionHelp.VoidType, new Type[] { ReflectionHelp.ObjectType, ReflectionHelp.ObjectType });
            il = method.GetILGenerator();
            ReflectionHelp.ILLdarg(il, 0);
            ReflectionHelp.ILCastclass(il, ReflectionHelp.ObjectType, type);
            ReflectionHelp.ILLdarg(il, 1);
            ReflectionHelp.ILCastclass(il, ReflectionHelp.ObjectType, field.FieldType);
            il.Emit(OpCodes.Stfld, field);
            il.Emit(OpCodes.Ret);
            FieldSet = (Action<object, object>)method.CreateDelegate(typeof(Action<object, object>));
        }

        public object Get(object Target)
        {
            if (FieldGet != null)
            {
                return FieldGet(Target);
            }
            return null;
        }

        public void Set(object Target, object Value)
        {
            if (FieldSet != null)
            {
                FieldSet(Target, Value);
            }
        }
        object IFieldAccessor.Get(object Target)
        {
            return this.Get(Target);
        }

        void IFieldAccessor.Set(object Target, object Value)
        {
            this.Set(Target, Value);
        }
    }

    /// <summary>
    /// 字段访问工厂 FieldReflectionCache
    /// </summary>
    public class DelegateFieldReflectionFactory : IFastReflectionFactory<FieldInfo, IFieldAccessor>
    {
        public IFieldAccessor Create(FieldInfo key)
        {
            return new DelegateFieldAccessor(key);
        }
        IFieldAccessor IFastReflectionFactory<FieldInfo, IFieldAccessor>.Create(FieldInfo key)
        {
            return this.Create(key);
        }
    }
}
