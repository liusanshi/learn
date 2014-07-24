using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LL.Framework.Web.MVC
{
    /// <summary>
    /// 视图的数据字典
    /// </summary>
    public class ViewDataDictionary : IDictionary<string, object>, ICollection<KeyValuePair<string, object>>
    {
        private readonly Dictionary<string, object> _innerDictionary;
        private readonly ModelStateDictionary _modelState;
        private object _model;

        #region 构造函数
        /// <summary>
        /// 创建 ViewDataDictionary 对象
        /// </summary>
        public ViewDataDictionary()
            : this((object)null)
        {
        }
        /// <summary>
        /// 创建 ViewDataDictionary 对象
        /// </summary>
        /// <param name="model"></param>
        public ViewDataDictionary(object model)
        {
            this._innerDictionary = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            this._modelState = new ModelStateDictionary();
            this.Model = model;
        }
        /// <summary>
        /// 创建 ViewDataDictionary 对象
        /// </summary>
        /// <param name="dictionary"></param>
        public ViewDataDictionary(ViewDataDictionary dictionary)
        {
            this._innerDictionary = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            this._modelState = new ModelStateDictionary();
            if (dictionary == null)
            {
                throw new ArgumentNullException("dictionary");
            }
            foreach (KeyValuePair<string, object> current in dictionary)
            {
                this._innerDictionary.Add(current.Key, current.Value);
            }
            foreach (KeyValuePair<string, ModelState> current2 in dictionary.ModelState)
            {
                this.ModelState.Add(current2.Key, current2.Value);
            }
            this.Model = dictionary.Model;
        }
        #endregion

        /// <summary>
        /// 模型状态
        /// </summary>
        public ModelStateDictionary ModelState
        {
            get
            {
                return this._modelState;
            }
        }
        /// <summary>
        /// 模型
        /// </summary>
        public object Model
        {
            get
            {
                return this._model;
            }
            set
            {
                this._model = value;
            }
        }

        #region IDictionary<string, object> 成员
        public void Add(string key, object value)
        {
            _innerDictionary.Add(key, value);
        }

        public bool ContainsKey(string key)
        {
            return _innerDictionary.ContainsKey(key);
        }

        public ICollection<string> Keys
        {
            get { return _innerDictionary.Keys; }
        }

        public bool Remove(string key)
        {
            return _innerDictionary.Remove(key);
        }

        public bool TryGetValue(string key, out object value)
        {
            return _innerDictionary.TryGetValue(key, out value);
        }

        public ICollection<object> Values
        {
            get { return _innerDictionary.Values; }
        }

        public object this[string key]
        {
            get
            {
                object val;
                _innerDictionary.TryGetValue(key, out val);
                return val;
            }
            set
            {
                _innerDictionary[key] = value;
            }
        }

        public void Add(KeyValuePair<string, object> item)
        {
            ((ICollection<KeyValuePair<string, object>>)_innerDictionary).Add(item);
        }

        public void Clear()
        {
            _innerDictionary.Clear();
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            return ((ICollection<KeyValuePair<string, object>>)_innerDictionary).Contains(item);
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<string, object>>)_innerDictionary).CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _innerDictionary.Count; }
        }

        public bool IsReadOnly
        {
            get { return ((ICollection<KeyValuePair<string, object>>)_innerDictionary).IsReadOnly; }
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            return ((ICollection<KeyValuePair<string, object>>)_innerDictionary).Remove(item);
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return _innerDictionary.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ((System.Collections.IEnumerable)_innerDictionary).GetEnumerator();
        }
        #endregion
    }
}
