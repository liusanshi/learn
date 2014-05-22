using Intgration.Common.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;

namespace Intgration.Common.win
{
    /// <summary>
    /// 文件系统权限控制
    /// </summary>
    public static class AccessControl
    {
        /// <summary>
        /// 给文件设置Everyone 完全控制权限
        /// </summary>
        /// <param name="path"></param>
        static public void FileInfoAccessControl(string path)
        {
            FileInfoAccessControl(path, "Everyone", FileSystemRights.FullControl);
        }

        /// <summary>
        /// 给文件设置权限
        /// </summary>
        /// <param name="path"></param>
        /// <param name="identity"></param>
        /// <param name="right"></param>
        public static void FileInfoAccessControl(string path, string identity, FileSystemRights right)
        {
            FileInfo fi = new FileInfo(path);
            if (!fi.Exists)
            {
                using (fi.Create()) ;
            }
            try
            {
                FileSecurity fsec = fi.GetAccessControl(AccessControlSections.All);
                FileSystemAccessRule arules = new FileSystemAccessRule(identity, right, AccessControlType.Allow);
                fsec.AddAccessRule(arules);
                fi.SetAccessControl(fsec);
            }
            catch 
            {
                throw;
            }
        }
        /// <summary>
        /// 文件夹权限
        /// </summary>
        /// <param name="path"></param>
        /// <param name="identity"></param>
        /// <param name="right"></param>
        /// <param name="inherit"></param>
        public static void DirectoryInfoAccessControl(string path, string identity, FileSystemRights right, bool inherit)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            if (!dir.Exists)
            {
                FileAndDirectoryManager.CreateDir(path);
            }
            try
            {
                DirectorySecurity dirsec = dir.GetAccessControl(AccessControlSections.All);
                FileSystemAccessRule arules = null;
                if (inherit)
                {
                    InheritanceFlags iflag = InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit;
                    arules = new FileSystemAccessRule(identity, right, iflag, PropagationFlags.None, AccessControlType.Allow);
                }
                else
                {
                    arules = new FileSystemAccessRule(identity, right, AccessControlType.Allow);

                }
                dirsec.AddAccessRule(arules);
                dir.SetAccessControl(dirsec);
            }
            catch 
            {
                throw;
            }
        }

        /// <summary>
        /// 给指定的文件夹添加Everyone 完全控制权限
        /// </summary>
        /// <param name="path"></param>
        static public void DirectoryInfoAccessControl(string path)
        {
            DirectoryInfoAccessControl(path, "Everyone", FileSystemRights.FullControl, true);
        }
    }
}
