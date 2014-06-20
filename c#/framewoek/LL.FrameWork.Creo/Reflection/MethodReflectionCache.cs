namespace LL.FrameWork.Core.Reflection
{
    using System;
    using System.Reflection;

    public class MethodReflectionCache : FastReflectionCache<MethodInfo, IMethodInvoker>
    {
        protected override IMethodInvoker Create(MethodInfo key)
        {
            return FastReflectionFactory.MethodReflectionFactory.Create(key);
        }
    }

#if Castle

using Castle.DynamicProxy;
using Castle.DynamicProxy.Generators;
using Castle.DynamicProxy.Generators.Emitters;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
    public class MethodInvoker : IMethodInvoker
    {
        private IMethodInvoker methodinvoker;
        public MethodInvoker() { }
        public MethodInvoker(ProxyGenerator generator, MethodInfo method)
        {
            methodinvoker = (IMethodInvoker)Activator.CreateInstance(CreateType(generator, method));
        }

        public object Invoke(object Target, params object[] args)
        {
            return methodinvoker.Invoke(Target, args);
        }
        object IMethodInvoker.Invoke(object Target, params object[] args)
        {
            return this.Invoke(Target, args);
        }

        Type CreateType(ProxyGenerator Generator, MethodInfo method)
        {
            var type = method.DeclaringType;

            CacheKey key = new CacheKey(method, type, Type.EmptyTypes, null);
            var MethodType = Generator.ProxyBuilder.ModuleScope.GetFromCache(key);
            if (MethodType == null)
            {
                ClassEmitter classemit = new ClassEmitter(Generator.ProxyBuilder.ModuleScope, ReflectionHelper.GetMemberSignName(method), null, new Type[] { typeof(IMethodInvoker) });

                var invokeemit = classemit.CreateMethod("Invoke", ReflectionHelper.DefaultMethodAttributes, ReflectionHelper.ObjectType, new Type[] { ReflectionHelper.ObjectType, ReflectionHelper.ObjArrayType });
                if (method.IsPublic)
                {
                    invokeemit.CodeBuilder.AddStatement(new ExpressionStatement(
                        new ConvertExpression(type, ReflectionHelper.ObjectType, invokeemit.Arguments[0].ToExpression())));

                    var para = method.GetParameters();//组织参数
                    Expression[] Expressions = null;
                    if (para != null && para.Length > 0)
                    {
                        Expressions = new Expression[para.Length];
                        var ArgReference = invokeemit.Arguments[1];
                        int i = 0;
                        foreach (var item in para)
                        {
                            Expressions[i] = new ConvertExpression(item.ParameterType, new LoadArrayElementExpression(i, ArgReference, ReflectionHelper.ObjectType));
                            i++;
                        }
                    }
                    if (Expressions == null)
                    {
                        invokeemit.CodeBuilder.AddStatement(new ReturnStatement(
                            new ConvertExpression(ReflectionHelper.ObjectType, method.ReturnType,
                                new MethodInvocationExpression(null, method))));
                    }
                    else
                    {
                        invokeemit.CodeBuilder.AddStatement(new ReturnStatement(
                            new ConvertExpression(ReflectionHelper.ObjectType, method.ReturnType,
                                new MethodInvocationExpression(null, method, Expressions))));
                    }
                }
                else
                {
                    invokeemit.CodeBuilder.AddStatement(new ThrowStatement(typeof(MethodAccessException),
                      "Method:" + method.Name + " is not public"));
                }
                MethodType = classemit.BuildType();
            }
            return MethodType;
        }
    }
#endif
}
