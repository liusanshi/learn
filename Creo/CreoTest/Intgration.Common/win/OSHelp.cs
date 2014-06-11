using System;
using System.Collections.Generic;
using System.IO;

namespace Intgration.Common.win
{
    /// <summary>
    /// 操作系统的帮助类
    /// </summary>
    public static class OSHelp
    {
        /// <summary>
        /// 是否win7操作系统
        /// </summary>
        /// <returns></returns>
        public static bool IsWin7
        {
            get
            {
                Version version = Environment.OSVersion.Version;
                return version.Major == 6 && version.Minor == 1;
            }
        }

        /// <summary>
        /// 是否vista以及vista后续的版本
        /// </summary>
        /// <returns></returns>
        public static bool IsVistaAndSubsequentVersion
        {
            get
            {
                Version version = Environment.OSVersion.Version;
                return version.Major >= 6;
            }
        }

        /// <summary>
        /// 获取执行平台 处理 ".sys", ".dll", ".exe", ".com", ".ocx" 文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static Platform GetExcutePlatform(string filePath)
        {
            if (!File.Exists(filePath))
                throw new ArgumentException(string.Format("file: {0} not found", filePath));

            FileStream stream = File.OpenRead(filePath);
            //移动到e_lfanew的位置处
            stream.Seek(0x40 - 4, SeekOrigin.Begin);
            byte[] buf = new byte[4];
            stream.Read(buf, 0, buf.Length);
            //根据e_lfanew的值计算出Machine的位置
            int pos = BitConverter.ToInt32(buf, 0) + 4;
            stream.Seek(pos, SeekOrigin.Begin);
            buf = new byte[2];
            stream.Read(buf, 0, buf.Length);

            byte[] bufInt32 = new byte[4];
            buf.CopyTo(bufInt32, 0);

            //得到Machine的值，0x14C为32位，0x8664为64位            
            Int32 machine = BitConverter.ToInt32(bufInt32, 0);
            if (machine == 0x14C)
            {
                return Platform.X86;
            }
            else if (machine == 0x8664)
            {
                return Platform.X64;
            }
            else
            {
                return Platform.Unknown;
            }
        }
    }

    /// <summary>
    /// 平台
    /// </summary>
    public enum Platform
    {
        /// <summary>
        /// 未知
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// 32位
        /// </summary>
        X86,
        /// <summary>
        /// 64位
        /// </summary>
        X64
    }
}
