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
            if (!method.IsPublic)
            {
                var types = ReflectionHelp.ConvertToType(method.GetParameters());
                MethodInvoker = (target, arguments) => method.Invoke(target, ReflectionHelp.GetArgumentByType(arguments, types));
                return;
            }

            var type = method.DeclaringType;
            DynamicMethod Dmethod = new DynamicMethod(ReflectionHelp.GetMemberSignName(method), ReflectionHelp.ObjectType, new Type[] { ReflectionHelp.ObjectType, ReflectionHelp.ObjArrayType });

            var il = Dmethod.GetILGenerator();
            ReflectionHelp.ILLdarg(il, 0);
            ReflectionHelp.ILCastclass(il, ReflectionHelp.ObjectType, type);
            var args = method.GetParameters();
            if (args != null)
            {
                for (byte i = 0; i < args.Length; i++)
                {
                    ReflectionHelp.ILLdarg(il, 1);
                    ReflectionHelp.ILLdelem(il, i, ReflectionHelp.ObjectType);
                    ReflectionHelp.ILCastclass(il, ReflectionHelp.ObjectType, args[i].ParameterType);
                }
            }
            il.Emit(OpCodes.Callvirt, method);
            if(method.ReturnType != ReflectionHelp.VoidType)
            {
                ReflectionHelp.ILCastclass(il, method.ReturnType, ReflectionHelp.ObjectType);
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

    /// <summary>
    /// 函数访问工厂 MethodReflectionCache
    /// </summary>
    public class DelegateMethodReflectionFactory : IFastReflectionFactory<MethodInfo, IMethodInvoker>
    {
        public IMethodInvoker Create(MethodInfo key)
        {
            return new DelegateMethodInvoker(key);
        }
        IMethodInvoker IFastReflectionFactory<MethodInfo, IMethodInvoker>.Create(MethodInfo key)
        {
            return this.Create(key);
        }
    }
}
