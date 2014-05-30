using System;
using System.Reflection;
using System.Configuration;
using RedGlovePermission.IDAL;
using RedGlovePermission.Lib;

namespace RedGlovePermission.DALFactory
{
    /// <summary>
    /// 抽象工厂模式
    /// </summary>
    public sealed class DataAccess
    {
        private static readonly string AssemblyPath = ConfigurationManager.AppSettings["DataDAL"];

        /// <summary>
        /// 创建对象或从缓存获取
        /// </summary>
        public static object CreateObject(string AssemblyPath, string ClassNamespace)
        {
            object objType = DataCache.GetCache(ClassNamespace);//从缓存读取
            if (objType == null)
            {
                try
                {
                    objType = Assembly.Load(AssemblyPath).CreateInstance(ClassNamespace);//反射创建
                    DataCache.SetCache(ClassNamespace, objType);// 写入缓存
                }
                catch
                { }
            }
            return objType;
        }
        
        /// <summary>
        /// 创建权限标识数据层接口
        /// </summary>
        public static RedGlovePermission.IDAL.IRGP_AuthorityDir CreateRGP_AuthorityDir()
        {
            string ClassNamespace = AssemblyPath + ".RGP_AuthorityDir";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (RedGlovePermission.IDAL.IRGP_AuthorityDir)objType;
        }

        /// <summary>
        /// 创建系统配置数据层接口
        /// </summary>
        public static RedGlovePermission.IDAL.IRGP_Configuration CreateRGP_Configuration()
        {

            string ClassNamespace = AssemblyPath + ".RGP_Configuration";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (RedGlovePermission.IDAL.IRGP_Configuration)objType;
        }

        /// <summary>
        /// 创建分组数据层接口
        /// </summary>
        public static RedGlovePermission.IDAL.IRGP_Groups CreateRGP_Groups()
        {
            string ClassNamespace = AssemblyPath + ".RGP_Groups";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (RedGlovePermission.IDAL.IRGP_Groups)objType;
        }

        /// <summary>
        /// 创建模块管理数据层接口
        /// </summary>
        public static RedGlovePermission.IDAL.IRGP_Modules CreateRGP_Modules()
        {
            string ClassNamespace = AssemblyPath + ".RGP_Modules";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (RedGlovePermission.IDAL.IRGP_Modules)objType;
        }

        /// <summary>
        /// 创建角色管理数据层接口
        /// </summary>
        public static RedGlovePermission.IDAL.IRGP_Roles CreateRGP_Roles()
        {
            string ClassNamespace = AssemblyPath + ".RGP_Roles";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (RedGlovePermission.IDAL.IRGP_Roles)objType;
        }

        /// <summary>
        /// 创建用户数据层接口
        /// </summary>
        public static RedGlovePermission.IDAL.IUsers CreateUsers()
        {
            string ClassNamespace = AssemblyPath + ".Users";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (RedGlovePermission.IDAL.IUsers)objType;
        }

    }
}
