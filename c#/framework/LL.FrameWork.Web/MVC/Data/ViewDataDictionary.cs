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
        /// <summary>
        /// 是否已经初始化了model
        /// </summary>
        private bool isInitModel = false;
        /// <summary>
        /// 延迟获取model
        /// </summary>
        private Func<object> _getModel;

        #region 构造函数
        /// <summary>
        /// 创建 ViewDataDictionary 对象
        /// </summary>
        public ViewDataDictionary()
        {
            this._innerDictionary = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            this._modelState = new ModelStateDictionary();
        }
        /// <summary>
        /// 创建 ViewDataDictionary 对象
        /// </summary>
        /// <param name="getmodel"></param>
        public ViewDataDictionary(Func<object> getmodel)
            : this()
        {
            isInitModel = false;
            _getModel = getmodel;
        }
        /// <summary>
        /// 创建 ViewDataDictionary 对象
        /// </summary>
        /// <param name="model"></param>
        public ViewDataDictionary(object model)
            : this()
        {
            isInitModel = true;
            this._model = model;
            _getModel = () => _model;
        }
        /// <summary>
        /// 创建 ViewDataDictionary 对象
        /// </summary>
        /// <param name="dictionary"></param>
        public ViewDataDictionary(ViewDataDictionary dictionary)
            : this()
        {
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
            isInitModel = true;
            this._model = dictionary.Model;
            _getModel = () => _model;
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
                if (!isInitModel && _getModel != null)
                {
                    _model = _getModel();
                    isInitModel = true;
                }
                return _model;
            }
            set
            {
                _model = value;
                if (_model != null)
                    isInitModel = true;
            }
        }

        /// <summary>
        /// 获取模型的延迟方法
        /// </summary>
        public Func<object> GetModel
        {
            get { return _getModel; }
            set
            {
                _getModel = value;
                isInitModel = false;
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
