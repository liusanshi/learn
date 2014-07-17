using System;

namespace LL.Framework.Core.Infrastructure.Adapter
{
    /// <summary>
    /// 类型匹配使用的 标识
    /// </summary>
    /// <typeparam name="TFrom"></typeparam>
    /// <typeparam name="TTo"></typeparam>
    public class TypeMapIdentity : IEquatable<TypeMapIdentity>
    {
        private Type _TFrom;
        private Type _TTo;
        private int? HashCode;

        public TypeMapIdentity(Type from, Type to)
        {
            _TFrom = from;
            _TTo = to;
        }

        /// <summary>
        /// 创建一个 标识
        /// </summary>
        /// <typeparam name="TFm"></typeparam>
        /// <typeparam name="TTo"></typeparam>
        /// <returns></returns>
        public static TypeMapIdentity GetIdentity<TFrom, TTo>()
        {
            return new TypeMapIdentity(typeof(TFrom), typeof(TTo));
        }
        /// <summary>
        /// from 类型
        /// </summary>
        public Type From { get { return _TFrom; } }
        /// <summary>
        /// to 类型
        /// </summary>
        public Type To { get { return _TTo; } }

        /// <summary>
        /// 相等比较
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            var temp = obj as TypeMapIdentity;
            if ((object)temp == null) return false;

            return temp._TFrom == _TFrom && temp._TTo == _TTo;
        }
        /// <summary>
        /// 获取hash值
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            if (!HashCode.HasValue)
            {
                HashCode = _TFrom.GetHashCode() * _TTo.GetHashCode();
            }
            return HashCode.Value;
        }

        /// <summary>
        /// 相等比较
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(TypeMapIdentity other)
        {
            if (other == (object)null) return false;

            return other._TFrom == _TFrom && other._TTo == _TTo;
        }

        /// <summary>
        /// 重载操作符 ==
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(TypeMapIdentity left, TypeMapIdentity right)
        {
            if ((object)left == null || (object)right == null) return false;
            return left.Equals(right);
        }
        /// <summary>
        /// 重载操作符 ==
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(TypeMapIdentity left, object right)
        {
            if ((object)left == null || (object)right == null) return false;
            return left.Equals(right);
        }
        /// <summary>
        /// 重载操作符 ==
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(object left, TypeMapIdentity right)
        {
            if ((object)left == null || (object)right == null) return false;
            return right.Equals(left);
        }
        /// <summary>
        /// 重载操作符 !=
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(TypeMapIdentity left, TypeMapIdentity right)
        {
            return !(left == right);
        }
        /// <summary>
        /// 重载操作符 !=
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(TypeMapIdentity left, object right)
        {
            return !(left == right);
        }
        /// <summary>
        /// 重载操作符 !=
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(object left, TypeMapIdentity right)
        {
            return !(left == right);
        }
    }
}
