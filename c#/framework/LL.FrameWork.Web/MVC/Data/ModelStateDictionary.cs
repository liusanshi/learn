using System;
using System.Collections.Generic;
using System.Linq;

namespace LL.Framework.Web.MVC
{
    /// <summary>
    /// 模型状态字典
    /// </summary>
    [Serializable]
    public class ModelStateDictionary : IDictionary<string, ModelState>
    {
        private readonly Dictionary<string, ModelState> _innerDictionary = new Dictionary<string, ModelState>(StringComparer.OrdinalIgnoreCase);

        #region 构造函数
        /// <summary>
        /// 创建 ModelStateDictionary 对象
        /// </summary>
        public ModelStateDictionary()
        {
        }
        /// <summary>
        /// 创建 ModelStateDictionary 对象
        /// </summary>
        /// <param name="dictionary"></param>
        public ModelStateDictionary(ModelStateDictionary dictionary)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException("dictionary");
            }
            foreach (KeyValuePair<string, ModelState> current in dictionary)
            {
                this._innerDictionary.Add(current.Key, current.Value);
            }
        }
        #endregion

        /// <summary>
        /// 是否验证通过
        /// </summary>
        public bool IsValid
        {
            get
            {
                return _innerDictionary.Values.All((ModelState modelState) => modelState.Errors.Count == 0);
            }
        }
        /// <summary>
        /// 验证指定的key是否有错误
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsValidField(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            return DictionaryHelpers.FindKeysWithPrefix<ModelState>(this, key).All(
                (KeyValuePair<string, ModelState> entry) => entry.Value.Errors.Count == 0);
        }
        /// <summary>
        /// 添加模型错误信息
        /// </summary>
        /// <param name="key"></param>
        /// <param name="exception"></param>
        public void AddModelError(string key, Exception exception)
        {
            this.GetModelStateForKey(key).Errors.Add(exception);
        }
        /// <summary>
        /// 添加模型错误信息
        /// </summary>
        /// <param name="key"></param>
        /// <param name="errorMessage"></param>
        public void AddModelError(string key, string errorMessage)
        {
            this.GetModelStateForKey(key).Errors.Add(errorMessage);
        }
        /// <summary>
        /// 合并 模型状态字典
        /// </summary>
        /// <param name="dictionary"></param>
        public void Merge(ModelStateDictionary dictionary)
        {
            if (dictionary == null)
            {
                return;
            }
            foreach (KeyValuePair<string, ModelState> current in dictionary)
            {
                this[current.Key] = current.Value;
            }
        }
        /// <summary>
        /// 根据Key返回 模型状态信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private ModelState GetModelStateForKey(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            ModelState modelState;
            if (!this.TryGetValue(key, out modelState))
            {
                modelState = new ModelState();
                this[key] = modelState;
            }
            return modelState;
        }

        #region IDictionary<string, ModelState> 成员
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(string key, ModelState value)
        {
            _innerDictionary.Add(key, value);
        }
        /// <summary>
        /// 是否包含key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(string key)
        {
            return _innerDictionary.ContainsKey(key);
        }
        /// <summary>
        /// 所有的key
        /// </summary>
        public ICollection<string> Keys
        {
            get { return _innerDictionary.Keys; }
        }
        /// <summary>
        /// 移出指定的值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(string key)
        {
            return _innerDictionary.Remove(key);
        }
        /// <summary>
        /// 获取与指定的键相关联的值。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(string key, out ModelState value)
        {
            return _innerDictionary.TryGetValue(key, out value);
        }
        /// <summary>
        /// 所有值
        /// </summary>
        public ICollection<ModelState> Values
        {
            get { return _innerDictionary.Values; }
        }
        /// <summary>
        /// 所引器
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ModelState this[string key]
        {
            get
            {
                ModelState modelState;
                _innerDictionary.TryGetValue(key, out modelState);
                return modelState;
            }
            set
            {
                _innerDictionary.Add(key, value);
            }
        }
        /// <summary>
        /// 添加值
        /// </summary>
        /// <param name="item"></param>
        public void Add(KeyValuePair<string, ModelState> item)
        {
            _innerDictionary.Add(item.Key, item.Value);
        }
        /// <summary>
        /// 清空
        /// </summary>
        public void Clear()
        {
            _innerDictionary.Clear();
        }
        /// <summary>
        /// 通过使用默认的相等比较器确定序列是否包含指定的元素。
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(KeyValuePair<string, ModelState> item)
        {
            return _innerDictionary.Contains(item);
        }
        /// <summary>
        /// 复制
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(KeyValuePair<string, ModelState>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<string, ModelState>>)_innerDictionary).CopyTo(array, arrayIndex);
        }
        /// <summary>
        /// 数量
        /// </summary>
        public int Count
        {
            get { return _innerDictionary.Count; }
        }
        /// <summary>
        /// 获取是否只读
        /// </summary>
        public bool IsReadOnly
        {
            get { return ((ICollection<KeyValuePair<string, ModelState>>)_innerDictionary).IsReadOnly; }
        }
        /// <summary>
        /// 移出
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(KeyValuePair<string, ModelState> item)
        {
            return ((ICollection<KeyValuePair<string, ModelState>>)_innerDictionary).Remove(item);
        }

        public IEnumerator<KeyValuePair<string, ModelState>> GetEnumerator()
        {
            return _innerDictionary.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ((System.Collections.IEnumerable)this._innerDictionary).GetEnumerator();
        }

        #endregion
    }
}
