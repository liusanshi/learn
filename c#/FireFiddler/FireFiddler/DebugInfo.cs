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
                Type = obj.GetValue("Type").ToObject<string>();
                Label = obj.GetValue("Label").ToObject<string>();
                File = obj.GetValue("File").ToObject<string>();
                Line = obj.GetValue("Line").ToObject<int>();
            }
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
