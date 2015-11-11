using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireFiddler
{
    /// <summary>
    /// 工具类
    /// </summary>
    public static class Util
    {
        /// <summary>
        /// 静态构造函数
        /// </summary>
        static Util()
        {
            LogPath = "";
            CfgPath = "";
        }

        /// <summary>
        /// 日志文件记录地址
        /// </summary>
        public static readonly string LogPath;
        /// <summary>
        /// 配置文件存放地址
        /// </summary>
        public static readonly string CfgPath;
    }
}
