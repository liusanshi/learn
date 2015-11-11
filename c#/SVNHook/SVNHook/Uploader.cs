using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SVNHook
{
    /// <summary>
    /// 上传接口
    /// </summary>
    public abstract class Uploader
    {
        /// <summary>
        /// 上传
        /// </summary>
        public abstract void upload();
        /// <summary>
        /// 文件上传路径
        /// </summary>
        public string FilePath { get; protected set; }

        /// <summary>
        /// 获取远程文件的路径
        /// </summary>
        /// <returns></returns>
        protected string GetRemotePath()
        {
            //2015-10-22 12:48:31 Commitfile:E:/svn/isd_qqvipserver_rep/ClubDev1Imgcache_proj/trunk/imgcache.qq.com/htdocs/vipstyle/game/act/20150618_qmtj_yy/img/bg_06.jpg

            //将其中的地址找出来
            //imgcache.qq.com/htdocs
            //var index = FilePath.IndexOf(partten);
            //if (index > -1)
            //{
            //    return FilePath.Substring(index + partten.Length + 1);
            //}

            //Util.Remote_Woek_Dir
            //Util.Local_Woek_Dir
            //http://tc-svn.tencent.com/isd/isd_qqvipserver_rep/ClubDev1Imgcache_proj/trunk/imgcache.qq.com/htdocs/vipstyle/game/act/xtz
            //E:\0000000【svn】0000000\imagecache

            var ftp_path = Util.Remote_Woek_Dir + FilePath.Substring(Util.Local_Woek_Dir.Length).Replace("\\", "/");
            var index = ftp_path.IndexOf(Util.Ftp_Upload_Dir);
            if (index > -1)
            {
                return ftp_path.Substring(index + Util.Ftp_Upload_Dir.Length + 1);
            }
            return "";
        }
    }

    /// <summary>
    /// 文件上传单元
    /// </summary>
    public class FileUploader : Uploader
    {

        public FileUploader(string filepath)
        {
            FilePath = filepath;
        }

        public override void upload()
        {
            try
            {
                //ftp://14.17.31.229:21000/vipstyle/game/zhuanqu/cf_20151014
                string RemoteFile = GetRemotePath();
                if (string.IsNullOrWhiteSpace(RemoteFile)) return;
                Util.Log("copyto : " + RemoteFile);
                Util.ftp.upload(RemoteFile, FilePath);
            }
            catch (Exception ex)
            {
                Util.LogException(ex);
            }
        }
    }

    /// <summary>
    /// 路径上传单元
    /// </summary>
    public class DirUploader : Uploader
    {
        public DirUploader(string dirpath)
        {
            FilePath = dirpath;
        }

        /// <summary>
        /// 上传
        /// </summary>
        public override void upload()
        {
            try
            {
                //2015-10-22 12:46:46 E:/svn/isd_qqvipserver_rep/ClubDev1Imgcache_proj/trunk/imgcache.qq.com/htdocs/vipstyle/game/act/20150618_qmtj_yy/img
                //ftp://14.17.31.229:21000/vipstyle/game/zhuanqu/cf_20151014
                string RemoteDir = GetRemotePath();
                if (string.IsNullOrWhiteSpace(RemoteDir)) return;
                Util.Log("createdir : " + RemoteDir);
                Util.ftp.createDirectory(RemoteDir); //新建文件夹
            }
            catch (Exception ex)
            {
                Util.LogException(ex);
            }
        }
    }

    public static class UploaderFactroy
    {
        public static Uploader CreateUploader(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return null;
            if (File.Exists(path))
            {
                return new FileUploader(path);
            }
            else
            {
                return new DirUploader(path);
            }
        }
    }
}
