using System;
using System.Linq;
using System.Collections.Generic;

namespace LL.Framework.Web.MVC
{
    /// <summary>
    /// 字典帮助类
    /// </summary>
    internal static class DictionaryHelpers
    {
        /// <summary>
        /// 查找所有含有指定前缀的key的值键对
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<string, TValue>> FindKeysWithPrefix<TValue>(IDictionary<string, TValue> dictionary, 
            string prefix)
        {
            TValue value;
            if (dictionary.TryGetValue(prefix, out value))
            {
                yield return new KeyValuePair<string, TValue>(prefix, value);
            }
            foreach (KeyValuePair<string, TValue> current in dictionary)
            {
                KeyValuePair<string, TValue> keyValuePair = current;
                string key = keyValuePair.Key;
                if (key.Length > prefix.Length && key.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                {
                    char c = key[prefix.Length];
                    if (c == '.' || c == '[')
                    {
                        yield return current;
                    }
                }
            }
            yield break;
        }
        /// <summary>
        /// 是否含有指定前缀的key
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static bool DoesAnyKeyHavePrefix<TValue>(IDictionary<string, TValue> dictionary, string prefix)
        {
            return DictionaryHelpers.FindKeysWithPrefix<TValue>(dictionary, prefix).Any();
        }
        /// <summary>
        /// 根据key获取value，没有则返回默认值
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="default"></param>
        /// <returns></returns>
        public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue @default)
        {
            TValue result;
            if (dict.TryGetValue(key, out result))
            {
                return result;
            }
            return @default;
        }
    }
}
