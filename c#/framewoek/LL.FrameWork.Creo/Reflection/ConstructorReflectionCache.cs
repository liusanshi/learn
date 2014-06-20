using System;
using System.Reflection;

using Castle.DynamicProxy;
using Castle.DynamicProxy.Generators;
using Castle.DynamicProxy.Generators.Emitters;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;

namespace LL.FrameWork.Core.Reflection
{
    public class ConstructorInvoker : IConstructorInvoker
    {
        private IConstructorInvoker Constructor;
        public ConstructorInvoker() { }
        public ConstructorInvoker(ProxyGenerator generator, ConstructorInfo constructor)
        {
            Constructor = (IConstructorInvoker)Activator.CreateInstance(CreateType(generator, constructor));
        }

        public object Invoke(params object[] args)
        {
            return Constructor.Invoke(args);
        }

        object IConstructorInvoker.Invoke(params object[] args)
        {
            return this.Invoke(args);
        }

        private Type CreateType(ProxyGenerator Generator, ConstructorInfo constructor)
        {
            var type = constructor.DeclaringType;

            CacheKey key = new CacheKey(constructor, type, Type.EmptyTypes, null);
            var ConstructorType = Generator.ProxyBuilder.ModuleScope.GetFromCache(key);
            if (ConstructorType == null)
            {
                ClassEmitter classemit = new ClassEmitter(Generator.ProxyBuilder.ModuleScope, ReflectionHelper.GetMemberSignName(constructor), null, new Type[] { typeof(IConstructorInvoker) });

                var invokeemit = classemit.CreateMethod("Invoke", ReflectionHelper.DefaultMethodAttributes, ReflectionHelper.ObjectType, new Type[] { ReflectionHelper.ObjArrayType });

                if (constructor.IsPublic)
                {
                    var para = constructor.GetParameters();//组织参数
                    Expression[] Expressions = null;
                    if (para != null && para.Length > 0)
                    {
                        Expressions = new Expression[para.Length];
                        var ArgReference = invokeemit.Arguments[0];
                        int i = 0;
                        foreach (var item in para)
                        {
                            Expressions[i] = new ConvertExpression(item.ParameterType, new LoadArrayElementExpression(i, ArgReference, ReflectionHelper.ObjectType));
                            i++;
                        }
                    }
                    if (Expressions == null)
                    {
                        invokeemit.CodeBuilder.AddStatement(new ReturnStatement(new NewInstanceExpression(constructor)));
                    }
                    else
                    {
                        invokeemit.CodeBuilder.AddStatement(new ReturnStatement(new NewInstanceExpression(constructor, Expressions)));
                    }
                }
                else
                {
                    invokeemit.CodeBuilder.AddStatement(new ThrowStatement(typeof(MemberAccessException), "the constructor is not public"));
                }
                ConstructorType = classemit.BuildType();
            }
            return ConstructorType;
        }
    }
    public class ConstructorReflectionCache : FastReflectionCache<ConstructorInfo, IConstructorInvoker>
    {
        protected override IConstructorInvoker Create(ConstructorInfo key)
        {
            return FastReflectionFactory.ConstructorReflectionFactory.Create(key);
        }
    }
}
