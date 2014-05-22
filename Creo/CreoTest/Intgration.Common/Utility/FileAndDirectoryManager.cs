using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Intgration.Common.Utility
{
    /// <summary>
    /// 文件、文件夹管理
    /// </summary>
    public static class FileAndDirectoryManager
    {
        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="path"></param>
        public static void CreateDir(string path)
        {
            if (!Directory.Exists(path))
            {
                CreateDir(Path.GetDirectoryName(path));
                Directory.CreateDirectory(path);
            }
        }
    }
}
