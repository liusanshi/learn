using System;
using System.Reflection;
using System.Reflection.Emit;

using Castle.DynamicProxy.Generators.Emitters;
using Castle.DynamicProxy.Generators.Emitters.CodeBuilders;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;

namespace LL.FrameWork.Core.Reflection
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
                var types = ReflectionHelp.ConvertToType(constructor.GetParameters());
                Constructor = arguments => constructor.Invoke(ReflectionHelp.GetArgumentByType(arguments, types));
                return;
            }

            DynamicMethod method = new DynamicMethod(ReflectionHelp.GetMemberSignName(constructor), ReflectionHelp.ObjectType, new Type[] { ReflectionHelp.ObjArrayType });

            var il = method.GetILGenerator();
            var args = constructor.GetParameters();
            if (args != null)
            {
                for (byte i = 0; i < args.Length; i++)
                {
                    ReflectionHelp.ILLdarg(il, 0);
                    ReflectionHelp.ILLdelem(il, i, ReflectionHelp.ObjectType);
                    ReflectionHelp.ILCastclass(il, ReflectionHelp.ObjectType, args[i].ParameterType);
                }
            }
            il.Emit(OpCodes.Newobj, constructor);
            ReflectionHelp.ILCastclass(il, constructor.DeclaringType, ReflectionHelp.ObjectType);
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
            var types = ReflectionHelp.ConvertToType(Constructor.GetParameters());
            return Constructor.Invoke(ReflectionHelp.GetArgumentByType(args, types));
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
