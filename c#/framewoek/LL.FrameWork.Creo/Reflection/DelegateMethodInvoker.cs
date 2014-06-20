using System;
using System.Reflection;
using System.Reflection.Emit;

namespace LL.FrameWork.Core.Reflection
{
    public class DelegateMethodInvoker : IMethodInvoker
    {
        private Func<object, object[], object> MethodInvoker;
        public DelegateMethodInvoker(MethodInfo method)
        {
            init(method);
        }

        void init(MethodInfo method)
        {
            if (method == null)
            {
                throw new ArgumentException("Argument: method is null");
            }

            var type = method.DeclaringType;
            DynamicMethod Dmethod = new DynamicMethod(ReflectionHelper.GetMemberSignName(method), ReflectionHelper.ObjectType, new Type[] { ReflectionHelper.ObjectType, ReflectionHelper.ObjArrayType });

            var il = Dmethod.GetILGenerator();
            ReflectionHelper.ILLdarg(il, 0);
            ReflectionHelper.ILCastclass(il, ReflectionHelper.ObjectType, type);
            var args = method.GetParameters();
            if (args != null)
            {
                for (byte i = 0; i < args.Length; i++)
                {
                    ReflectionHelper.ILLdarg(il, 1);
                    ReflectionHelper.ILLdelem(il, i, ReflectionHelper.ObjectType);
                    ReflectionHelper.ILCastclass(il, ReflectionHelper.ObjectType, args[i].ParameterType);
                }
            }
            il.Emit(OpCodes.Callvirt, method);
            if (method.ReturnType != ReflectionHelper.VoidType)
            {
                ReflectionHelper.ILCastclass(il, method.ReturnType, ReflectionHelper.ObjectType);
            }
            il.Emit(OpCodes.Ret);
            MethodInvoker = (Func<object, object[], object>)Dmethod.CreateDelegate(typeof(Func<object, object[], object>));
        }

        public object Invoke(object Target, params object[] args)
        {
            return MethodInvoker(Target, args);
        }
        object IMethodInvoker.Invoke(object Target, params object[] args)
        {
            return this.Invoke(Target, args);
        }
    }

    public class PrimitiveMethodInvoker : IMethodInvoker
    {
        MethodInfo Method = null;
        public PrimitiveMethodInvoker(MethodInfo method)
        {
            Method = method;
        }
        public object Invoke(object Target, params object[] args)
        {
            var types = ReflectionHelper.ConvertToType(Method.GetParameters());
            return Method.Invoke(Target, ReflectionHelper.GetArgumentByType(args, types));
        }
        object IMethodInvoker.Invoke(object Target, params object[] args)
        {
            return this.Invoke(Target, args);
        }
    }

    /// <summary>
    /// 函数访问工厂 MethodReflectionCache
    /// </summary>
    public class DelegateMethodReflectionFactory : IFastReflectionFactory<MethodInfo, IMethodInvoker>
    {
        public IMethodInvoker Create(MethodInfo key)
        {
            var type = key.DeclaringType;
            if (!type.IsPublic || !key.IsPublic)
            {
                return new PrimitiveMethodInvoker(key);
            }
            else
            {
                return new DelegateMethodInvoker(key);
            }
        }
        IMethodInvoker IFastReflectionFactory<MethodInfo, IMethodInvoker>.Create(MethodInfo key)
        {
            return this.Create(key);
        }
    }
}
