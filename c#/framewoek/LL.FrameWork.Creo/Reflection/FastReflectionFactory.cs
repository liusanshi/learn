using System;
using System.Reflection;

using Castle.DynamicProxy;

namespace LL.FrameWork.Core.Reflection
{
    /// <summary>
    /// 反射工厂接口
    /// </summary>
    public interface IFastReflectionFactory<TKey, TValue>
    {
        TValue Create(TKey key);
    }

    /// <summary>
    /// 属性访问工厂 ProertyReflectionCache
    /// </summary>
    public class ProertyReflectionFactory : IFastReflectionFactory<PropertyInfo, IPropertyAccessor>
    {
        ProxyGenerator Generator;
        public ProertyReflectionFactory() { }
        public ProertyReflectionFactory(ProxyGenerator generator) { Generator = generator; }
        public IPropertyAccessor Create(PropertyInfo key)
        {
            return new PropertyAccessor(Generator, key);
        }
        IPropertyAccessor IFastReflectionFactory<PropertyInfo, IPropertyAccessor>.Create(PropertyInfo key)
        {
            return this.Create(key);
        }
    }
    /// <summary>
    /// 字段访问工厂 FieldReflectionCache
    /// </summary>
    public class FieldReflectionFactory : IFastReflectionFactory<FieldInfo, IFieldAccessor>
    {
        ProxyGenerator Generator;
        public FieldReflectionFactory() { }
        public FieldReflectionFactory(ProxyGenerator generator) { Generator = generator; }
        public IFieldAccessor Create(FieldInfo key)
        {
            return new FieldAccessor(Generator, key);
        }
        IFieldAccessor IFastReflectionFactory<FieldInfo, IFieldAccessor>.Create(FieldInfo key)
        {
            return this.Create(key);
        }
    }
    /// <summary>
    /// 函数访问工厂 MethodReflectionCache
    /// </summary>
    public class MethodReflectionFactory : IFastReflectionFactory<MethodInfo, IMethodInvoker>
    {
        ProxyGenerator Generator;
        public MethodReflectionFactory() { }
        public MethodReflectionFactory(ProxyGenerator generator) { Generator = generator; }
        public IMethodInvoker Create(MethodInfo key)
        {
            return new MethodInvoker(Generator, key);
        }
        IMethodInvoker IFastReflectionFactory<MethodInfo, IMethodInvoker>.Create(MethodInfo key)
        {
            return this.Create(key);
        }
    }
    /// <summary>
    /// 构造函数访问工厂
    /// </summary>
    public class ConstructorReflectionFactory : IFastReflectionFactory<ConstructorInfo, IConstructorInvoker>
    {
        ProxyGenerator Generator;
        public ConstructorReflectionFactory() { }
        public ConstructorReflectionFactory(ProxyGenerator generator) { Generator = generator; }
        public IConstructorInvoker Create(ConstructorInfo key)
        {
            return new ConstructorInvoker(Generator, key);
        }
        IConstructorInvoker IFastReflectionFactory<ConstructorInfo, IConstructorInvoker>.Create(ConstructorInfo key)
        {
            return this.Create(key);
        }
    }

    public static class FastReflectionFactory
    {
        static IFastReflectionFactory<ConstructorInfo, IConstructorInvoker> _ConstructorReflectionFactory;
        static IFastReflectionFactory<MethodInfo, IMethodInvoker> _MethodReflectionFactory;
        static IFastReflectionFactory<FieldInfo, IFieldAccessor> _FieldReflectionFactory;
        static IFastReflectionFactory<PropertyInfo, IPropertyAccessor> _ProertyReflectionFactory;
        static ProxyGenerator generator = null;

        static FastReflectionFactory()
        {
            //IProxyBuilder builder = new DefaultProxyBuilder(new ModuleScope(true));
            //generator = new ProxyGenerator(builder);

            //_ConstructorReflectionFactory = new ConstructorReflectionFactory(generator);
            //_MethodReflectionFactory = new MethodReflectionFactory(generator);
            //_FieldReflectionFactory = new FieldReflectionFactory(generator);
            //_ProertyReflectionFactory = new ProertyReflectionFactory(generator);

            _ConstructorReflectionFactory = new DelegateConstructorReflectionFactory();
            _MethodReflectionFactory = new DelegateMethodReflectionFactory();
            _FieldReflectionFactory = new DelegateFieldReflectionFactory();
            _ProertyReflectionFactory = new DelegateProertyReflectionFactory();
        }

        public static void SaveCache()
        {
            try
            {
                generator.ProxyBuilder.ModuleScope.SaveAssembly(false);
                generator.ProxyBuilder.ModuleScope.SaveAssembly(true);                
            }
            catch
            { }
        }
        /// <summary>
        /// 属性访问对象
        /// </summary>
        public static IFastReflectionFactory<PropertyInfo, IPropertyAccessor> ProertyReflectionFactory
        {
            get { return _ProertyReflectionFactory; }
            set { _ProertyReflectionFactory = value; }
        }
        /// <summary>
        /// 字段访问对象
        /// </summary>
        public static IFastReflectionFactory<FieldInfo, IFieldAccessor> FieldReflectionFactory
        {
            get { return _FieldReflectionFactory; }
            set { _FieldReflectionFactory = value; }
        }
        /// <summary>
        /// 方法访问对象
        /// </summary>
        public static IFastReflectionFactory<MethodInfo, IMethodInvoker> MethodReflectionFactory
        {
            get { return _MethodReflectionFactory; }
            set { _MethodReflectionFactory = value; }
        }
        /// <summary>
        /// 构造器访问对象
        /// </summary>
        public static IFastReflectionFactory<ConstructorInfo, IConstructorInvoker> ConstructorReflectionFactory
        {
            get { return _ConstructorReflectionFactory; }
            set { _ConstructorReflectionFactory = value; }
        }


    }
}
