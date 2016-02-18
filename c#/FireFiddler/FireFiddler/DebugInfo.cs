using System;

using Newtonsoft.Json.Linq;

namespace FireFiddler
{
    /// <summary>
    /// 调试信息
    /// </summary>
    public class DebugInfo
    {
        public DebugInfo(JObject obj)
        {
            if (obj != null && obj.HasValues)
            {
                Type = TryGetValue<string>(obj, "Type");
                Label = TryGetValue<string>(obj, "Label");
                File = TryGetValue<string>(obj, "File");
                Line = TryGetValue<int>(obj, "Line");
            }
        }

        /// <summary>
        /// 安全获取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private T TryGetValue<T>(JObject obj, string key)
        {
            if (obj != null && obj.HasValues)
            {
                JToken val;
                if (obj.TryGetValue(key, out val))
                {
                    return val.ToObject<T>();
                }
            }
            return default(T);
        }

        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// 输出文件
        /// </summary>
        public string File { get; set; }

        /// <summary>
        /// 输出行数
        /// </summary>
        public int Line { get; set; }
    }
}
