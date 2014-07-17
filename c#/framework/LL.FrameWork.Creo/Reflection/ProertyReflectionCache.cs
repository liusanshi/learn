
namespace LL.FrameWork.Core.Reflection
{
    using System;
    using System.Reflection;

    /// <summary>
    /// 属性访问器
    /// </summary>
    public class PropertyReflectionCache : FastReflectionCache<PropertyInfo, IPropertyAccessor>
    {
        protected override IPropertyAccessor Create(PropertyInfo key)
        {
            return FastReflectionFactory.ProertyReflectionFactory.Create(key);
        }
    }

#if Castle  
    
using Castle.DynamicProxy;
using Castle.DynamicProxy.Generators;
using Castle.DynamicProxy.Generators.Emitters;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;

    public class PropertyAccessor : IPropertyAccessor
    {
        private IPropertyAccessor propertyaccessor;
        public PropertyAccessor() { }
        public PropertyAccessor(ProxyGenerator generator, PropertyInfo pi)
        {
            propertyaccessor = (IPropertyAccessor)Activator.CreateInstance(CreateType(generator, pi));
        }

        public object Get(object Target)
        {
            return propertyaccessor.Get(Target);
        }

        public void Set(object Target, object Value)
        {
            propertyaccessor.Set(Target, Value);
        }
        object IPropertyAccessor.Get(object Target)
        {
            return this.Get(Target);
        }

        void IPropertyAccessor.Set(object Target, object Value)
        {
            this.Set(Target, Value);
        }

        Type CreateType(ProxyGenerator Generator, PropertyInfo Property)
        {
            var type = Property.DeclaringType;

            CacheKey key = new CacheKey(Property, type, Type.EmptyTypes, null);
            var TPropAccessor = Generator.ProxyBuilder.ModuleScope.GetFromCache(key);
            if (TPropAccessor == null)
            {
                ClassEmitter classemit = new ClassEmitter(Generator.ProxyBuilder.ModuleScope, ReflectionHelper.GetMemberSignName(Property), null, new Type[] { typeof(IPropertyAccessor) });

                var getemit = classemit.CreateMethod("Get", ReflectionHelper.DefaultMethodAttributes, ReflectionHelper.ObjectType, new Type[] { ReflectionHelper.ObjectType });
                MethodInfo method = Property.GetGetMethod();
                if (Property.CanRead && method != null)
                {
                    getemit.CodeBuilder.AddStatement(new ExpressionStatement(
                        new ConvertExpression(type, ReflectionHelper.ObjectType, getemit.Arguments[0].ToExpression())));
                    getemit.CodeBuilder.AddStatement(new ReturnStatement(
                        new ConvertExpression(ReflectionHelper.ObjectType, Property.PropertyType,
                            new MethodInvocationExpression(null, method))));
                }
                else
                {
                    if (!Property.CanRead)
                    {
                        getemit.CodeBuilder.AddStatement(new ThrowStatement(typeof(NotSupportedException), "Get method is not defined for this property"));
                    }
                    else
                    {
                        getemit.CodeBuilder.AddStatement(new ThrowStatement(typeof(MethodAccessException), "Get method is not public for this property"));
                    }
                }

                method = Property.GetSetMethod();
                var setemit = classemit.CreateMethod("Set", ReflectionHelper.DefaultMethodAttributes, ReflectionHelper.VoidType, new Type[] { ReflectionHelper.ObjectType, ReflectionHelper.ObjectType });
                if (Property.CanWrite && method != null)
                {
                    MethodInvocationExpression setMethod = new MethodInvocationExpression(setemit.Arguments[0], method,
                        new ConvertExpression(Property.PropertyType, ReflectionHelper.ObjectType, setemit.Arguments[1].ToExpression()));
                    setemit.CodeBuilder.AddStatement(new ReturnStatement(setMethod));
                }
                else
                {
                    if (!Property.CanWrite)
                    {
                        setemit.CodeBuilder.AddStatement(new ThrowStatement(typeof(NotSupportedException), "Set method is not defined for this property"));
                    }
                    else
                    {
                        setemit.CodeBuilder.AddStatement(new ThrowStatement(typeof(MethodAccessException), "Set method is not public for this property"));
                    }
                }
                TPropAccessor = classemit.BuildType();
            }
            return TPropAccessor;
        }
    }
#endif
}
