
namespace LL.Framework.Core.Reflection
{
    /// <summary>
    /// 属性访问器
    /// </summary>
    public interface IPropertyAccessor
    {
        object Get(object Target);

        void Set(object Target, object Value);
    }
    /// <summary>
    /// 字段访问器
    /// </summary>
    public interface IFieldAccessor
    {
        object Get(object Target);

        void Set(object Target, object Value);
    }
    /// <summary>
    /// 方法调用器
    /// </summary>
    public interface IMethodInvoker
    {
        object Invoke(object Target, params object[] args);
    }

    /// <summary>
    /// 创建对象
    /// </summary>
    /// <typeparam name="TTarget"></typeparam>
    public interface IConstructorInvoker
    {
        object Invoke(params object[] args);
    }
}
