using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LL.FrameWork.Web.MVC
{
    public class TempDataDictionary : IDictionary<string, object>, ICollection<KeyValuePair<string, object>>, IEnumerable<KeyValuePair<string, object>>, IEnumerable
    {
        /// <summary>
        /// 创建一个 TempDataDictionary 临时数据对象
        /// </summary>
        public TempDataDictionary()
        {
            this._data = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        }

        private Dictionary<string, object> _data;
        /// <summary>
        /// 记录临时数据里面的数据(如果获取过就删除记录，添加就会记录)
        /// </summary>
        private HashSet<string> _initialKeys = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        /// <summary>
        /// 记录需要保持的数据
        /// </summary>
        private HashSet<string> _retainedKeys = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        /// <summary>
        /// 数量
        /// </summary>
        public int Count
        {
            get
            {
                return this._data.Count;
            }
        }
        /// <summary>
        /// Keys
        /// </summary>
        public ICollection<string> Keys
        {
            get
            {
                return this._data.Keys;
            }
        }
        /// <summary>
        /// 所有值
        /// </summary>
        public ICollection<object> Values
        {
            get
            {
                return this._data.Values;
            }
        }
        /// <summary>
        /// 是否只读
        /// </summary>
        bool ICollection<KeyValuePair<string, object>>.IsReadOnly
        {
            get
            {
                return ((ICollection<KeyValuePair<string, object>>)this._data).IsReadOnly;
            }
        }
        /// <summary>
        /// 获取或者设置值，会记录过程
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object this[string key]
        {
            get
            {
                object result;
                if (this.TryGetValue(key, out result))
                {
                    this._initialKeys.Remove(key);
                    return result;
                }
                return null;
            }
            set
            {
                this._data[key] = value;
                this._initialKeys.Add(key);
            }
        }
        /// <summary>
        /// 保持数据
        /// </summary>
        public void Keep()
        {
            this._retainedKeys.Clear();
            this._retainedKeys.UnionWith(this._data.Keys);
        }
        /// <summary>
        /// 保持指定的数据
        /// </summary>
        /// <param name="key"></param>
        public void Keep(string key)
        {
            this._retainedKeys.Add(key);
        }
        /// <summary>
        /// 查看值，不会记录过程
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object Peek(string key)
        {
            object result;
            this._data.TryGetValue(key, out result);
            return result;
        }
        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="tempDataProvider"></param>
        public void Load(ControllerContext controllerContext, ITempDataProvider tempDataProvider)
        {
            IDictionary<string, object> dictionary = tempDataProvider.LoadTempData(controllerContext);
            this._data = ((dictionary != null) ? new Dictionary<string, object>(dictionary, StringComparer.OrdinalIgnoreCase) : new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase));
            this._initialKeys = new HashSet<string>(this._data.Keys, StringComparer.OrdinalIgnoreCase);
            this._retainedKeys.Clear();
        }
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="tempDataProvider"></param>
        public void Save(ControllerContext controllerContext, ITempDataProvider tempDataProvider)
        {
            string[] second = this._initialKeys.Union(this._retainedKeys, StringComparer.OrdinalIgnoreCase).ToArray<string>();
            string[] array = this._data.Keys.Except(second, StringComparer.OrdinalIgnoreCase).ToArray<string>();
            for (int i = 0; i < array.Length; i++)
            {
                string key = array[i];
                this._data.Remove(key);
            }
            tempDataProvider.SaveTempData(controllerContext, this._data);
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(string key, object value)
        {
            this._data.Add(key, value);
            this._initialKeys.Add(key);
        }
        /// <summary>
        /// 清空数据
        /// </summary>
        public void Clear()
        {
            this._data.Clear();
            this._retainedKeys.Clear();
            this._initialKeys.Clear();
        }
        /// <summary>
        /// 是否包含key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(string key)
        {
            return this._data.ContainsKey(key);
        }
        /// <summary>
        /// 是否包含值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool ContainsValue(object value)
        {
            return this._data.ContainsValue(value);
        }
        /// <summary>
        /// 移出 key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(string key)
        {
            this._retainedKeys.Remove(key);
            this._initialKeys.Remove(key);
            return this._data.Remove(key);
        }
        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(string key, out object value)
        {
            this._initialKeys.Remove(key);
            return this._data.TryGetValue(key, out value);
        }

        void ICollection<KeyValuePair<string, object>>.CopyTo(KeyValuePair<string, object>[] array, int index)
        {
            ((ICollection<KeyValuePair<string, object>>)this._data).CopyTo(array, index);
        }
        void ICollection<KeyValuePair<string, object>>.Add(KeyValuePair<string, object> keyValuePair)
        {
            this._initialKeys.Add(keyValuePair.Key);
            ((ICollection<KeyValuePair<string, object>>)this._data).Add(keyValuePair);
        }
        bool ICollection<KeyValuePair<string, object>>.Contains(KeyValuePair<string, object> keyValuePair)
        {
            return ((ICollection<KeyValuePair<string, object>>)this._data).Contains(keyValuePair);
        }
        bool ICollection<KeyValuePair<string, object>>.Remove(KeyValuePair<string, object> keyValuePair)
        {
            this._initialKeys.Remove(keyValuePair.Key);
            return ((ICollection<KeyValuePair<string, object>>)this._data).Remove(keyValuePair);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            //return this._data.GetEnumerator();
            return new TempDataDictionary.TempDataDictionaryEnumerator(this);
        }
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            //return this._data.GetEnumerator();
            return new TempDataDictionary.TempDataDictionaryEnumerator(this);
        }

        /// <summary>
        /// 临时数据字典的 迭代器
        /// </summary>
        private sealed class TempDataDictionaryEnumerator : IEnumerator<KeyValuePair<string, object>>, IDisposable, IEnumerator
        {
            private IEnumerator<KeyValuePair<string, object>> _enumerator;
            private TempDataDictionary _tempData;
            /// <summary>
            /// 当前项
            /// </summary>
            public KeyValuePair<string, object> Current
            {
                get
                {
                    KeyValuePair<string, object> current = this._enumerator.Current;
                    this._tempData._initialKeys.Remove(current.Key);//获取的时候删除需要保存数据的记录
                    return current;
                }
            }
            object IEnumerator.Current
            {
                get
                {
                    return this.Current;
                }
            }
            public TempDataDictionaryEnumerator(TempDataDictionary tempData)
            {
                this._tempData = tempData;
                this._enumerator = this._tempData._data.GetEnumerator();
            }
            public bool MoveNext()
            {
                return this._enumerator.MoveNext();
            }
            public void Reset()
            {
                this._enumerator.Reset();
            }
            void IDisposable.Dispose()
            {
                this._enumerator.Dispose();
            }
        }
    }
}
