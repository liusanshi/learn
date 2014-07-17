using System;
using System.Collections.Generic;
using System.Threading;

namespace LL.Framework.Web.MVC
{
    /// <summary>
    /// 泛型读写缓存类 来自System.Web.Mvc.ReaderWriterCache
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    internal abstract class ReaderWriterCache<TKey, TValue>
    {
        private readonly Dictionary<TKey, TValue> _cache;
        private readonly ReaderWriterLockSlim _rwLock = new ReaderWriterLockSlim();
        protected Dictionary<TKey, TValue> Cache
        {
            get
            {
                return this._cache;
            }
        }
        protected ReaderWriterCache()
            : this(null)
        {
        }
        protected ReaderWriterCache(IEqualityComparer<TKey> comparer)
        {
            this._cache = new Dictionary<TKey, TValue>(1024, comparer);
        }
        protected TValue FetchOrCreateItem(TKey key, Func<TValue> creator)
        {
            this._rwLock.EnterReadLock();
            TValue result;
            try
            {
                TValue tValue;
                if (this._cache.TryGetValue(key, out tValue))
                {
                    result = tValue;
                    return result;
                }
            }
            finally
            {
                this._rwLock.ExitReadLock();
            }
            TValue tValue2 = creator();
            this._rwLock.EnterWriteLock();
            try
            {
                TValue tValue3;
                if (this._cache.TryGetValue(key, out tValue3))
                {
                    result = tValue3;
                }
                else
                {
                    this._cache[key] = tValue2;
                    result = tValue2;
                }
            }
            finally
            {
                this._rwLock.ExitWriteLock();
            }
            return result;
        }
    }
}
