using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Configuration;

namespace SVNHook
{
    /// <summary>
    /// 工具类
    /// </summary>
    public static class Util
    {
        public static readonly Ftp ftp;
        public static readonly string Exec_Path = "";
        private static readonly string log_name = "log.txt";
        public static readonly string Remote_Woek_Dir = "";
        public static readonly string Local_Woek_Dir = "";
        public static readonly string Ftp_Upload_Dir = "";

        static Util()
        {
            ftp = new Ftp("14.17.31.229:21000", "svip", "tencent@123");
            Exec_Path = AppDomain.CurrentDomain.BaseDirectory;
            Remote_Woek_Dir = ConfigurationManager.AppSettings["Remote_Woek_Dir"]; //远程工作目录
            Local_Woek_Dir = ConfigurationManager.AppSettings["Local_Woek_Dir"];   //本地工作目录
            Ftp_Upload_Dir = ConfigurationManager.AppSettings["Ftp_Upload_Dir"];   //上传服务器的地址
            if (string.IsNullOrEmpty(Ftp_Upload_Dir))
            {
                Ftp_Upload_Dir = "imgcache.qq.com/htdocs";
            }
        }

        /// <summary>
        /// 同步路径
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="targetPath"></param>
        public static void SyncDict(string sourcePath, string targetPath) 
        {
            
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="content"></param>
        public static void Log(string content)
        {
            string path = Path.Combine(Exec_Path, log_name);
            string prex = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ";
            File.AppendAllText(path, prex + content + "\r\n", Encoding.UTF8);
        }

        /// <summary>
        /// 记录错误日志
        /// </summary>
        /// <param name="ex"></param>
        public static void LogException(Exception ex) 
        {
            Util.Log("========================================");
            Util.Log(ex.Message);
            Util.Log(ex.StackTrace);
            Util.Log("========================================");
        }
    }    
}
