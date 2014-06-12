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

        /// <summary>
        /// 文档查询 会查找递归查找
        /// </summary>
        /// <param name="path"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static IEnumerable<string> SearchFile(string path, string pattern)
        {
            if (!Directory.Exists(path)) return Enumerable.Empty<string>();

            return Directory.GetFiles(path, pattern, SearchOption.AllDirectories);
        }

        /// <summary>
        /// 文档查询 会查找递归查找 会优先查找指定的文件夹
        /// </summary>
        /// <param name="path"></param>
        /// <param name="folders"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static IEnumerable<string> SearchFile(string path, IEnumerable<string> folders, string pattern)
        {
            IEnumerable<string> result;
            foreach (var item in folders.Select(p => Path.Combine(path, p.TrimStart('\\'))))
            {
                result = SearchFile(item, pattern);
                if (result.Any()) return result;
            }

            return SearchFile(path, pattern);
        }
    }
}
