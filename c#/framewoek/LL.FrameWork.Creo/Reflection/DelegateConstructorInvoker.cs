using System;
using System.Reflection;
using System.Reflection.Emit;

namespace LL.Framework.Core.Reflection
{
    public class DelegateConstructorInvoker : IConstructorInvoker
    {
        private Func<object[], object> Constructor;
        public DelegateConstructorInvoker(ConstructorInfo cons)
        {
            init(cons);
        }

        public object Invoke(params object[] args)
        {
            if (Constructor != null)
            {
                return Constructor(args);
            }
            return null;
        }
        object IConstructorInvoker.Invoke(params object[] args)
        {
            return this.Invoke(args);
        }

        private void init(ConstructorInfo constructor)
        {
            if (constructor == null)
            {
                throw new ArgumentException("Argument: constructor is null");
            }
            if (!constructor.IsPublic)
            {
                var types = ReflectionHelper.ConvertToType(constructor.GetParameters());
                Constructor = arguments => constructor.Invoke(ReflectionHelper.GetArgumentByType(arguments, types));
                return;
            }

            DynamicMethod method = new DynamicMethod(ReflectionHelper.GetMemberSignName(constructor), ReflectionHelper.ObjectType, new Type[] { ReflectionHelper.ObjArrayType });

            var il = method.GetILGenerator();
            var args = constructor.GetParameters();
            if (args != null)
            {
                for (byte i = 0; i < args.Length; i++)
                {
                    ReflectionHelper.ILLdarg(il, 0);
                    ReflectionHelper.ILLdelem(il, i, ReflectionHelper.ObjectType);
                    ReflectionHelper.ILCastclass(il, ReflectionHelper.ObjectType, args[i].ParameterType);
                }
            }
            il.Emit(OpCodes.Newobj, constructor);
            ReflectionHelper.ILCastclass(il, constructor.DeclaringType, ReflectionHelper.ObjectType);
            il.Emit(OpCodes.Ret);

            Constructor = (Func<object[], object>)method.CreateDelegate(typeof(Func<object[], object>));
        }
    }

    public class PrimitiveConstructorInvoker : IConstructorInvoker
    {
        ConstructorInfo Constructor = null;
        public PrimitiveConstructorInvoker(ConstructorInfo cons)
        {
            Constructor = cons;
        }

        public object Invoke(params object[] args)
        {
            var types = ReflectionHelper.ConvertToType(Constructor.GetParameters());
            return Constructor.Invoke(ReflectionHelper.GetArgumentByType(args, types));
        }
        object IConstructorInvoker.Invoke(params object[] args)
        {
            return this.Invoke(args);
        }
    }

    /// <summary>
    /// 构造函数访问工厂
    /// </summary>
    public class DelegateConstructorReflectionFactory : IFastReflectionFactory<ConstructorInfo, IConstructorInvoker>
    {
        public IConstructorInvoker Create(ConstructorInfo key)
        {
            var type = key.DeclaringType;
            if (!type.IsPublic || !key.IsPublic)
            {
                return new PrimitiveConstructorInvoker(key);
            }
            else
            {
                return new DelegateConstructorInvoker(key);
            }
        }
        IConstructorInvoker IFastReflectionFactory<ConstructorInfo, IConstructorInvoker>.Create(ConstructorInfo key)
        {
            return this.Create(key);
        }
    }
}
