using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

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
            string BasePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            LogPath = Path.Combine(Path.GetDirectoryName(BasePath), "FireFiddler\\log");
            CfgPath = Path.Combine(Path.GetDirectoryName(BasePath), "FireFiddler\\");

            initallDir(LogPath);
            initallDir(CfgPath);
        }

        /// <summary>
        /// 初始化目录
        /// </summary>
        /// <param name="dir"></param>
        static void initallDir(string dir)
        {
            if (!Directory.Exists(dir)) 
            {
                initallDir(Path.GetDirectoryName(dir));
                Directory.CreateDirectory(dir);
            }
        }

        /// <summary>
        /// 日志文件记录地址
        /// </summary>
        public static readonly string LogPath;
        /// <summary>
        /// 配置文件存放地址
        /// </summary>
        public static readonly string CfgPath;

        /// <summary>
        /// 记录错误信息
        /// </summary>
        /// <param name="exp"></param>
        public static void Log(Exception exp) 
        {
            StringBuilder sb = new StringBuilder(100);
            sb.Append(exp.Message);
            sb.Append(exp.StackTrace);
            Log(sb.ToString());
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="msg">日志文本信息</param>
        public static void Log(string msg)
        {
            string FileFullName = Path.Combine(LogPath, DateTime.Now.ToString("yyyyMMdd") + ".txt");

            File.AppendAllText(FileFullName, msg);
        }
    }
}
