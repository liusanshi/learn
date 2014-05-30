using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;

using RedGlovePermission.Lib;
using RedGlovePermission.Model;
using RedGlovePermission.DALFactory;
using RedGlovePermission.IDAL;

namespace RedGlovePermission.BLL
{
	/// <summary>
	/// 业务逻辑类RGP_Modules 的摘要说明。
	/// </summary>
	public class RGP_Modules
	{
        private readonly IRGP_Modules dal = DataAccess.CreateRGP_Modules();

		public RGP_Modules()
		{ }

        #region 模块分类

        /// <summary>
        /// 判断模块分类是否存在
        /// </summary>
        /// <param name="ModuleTypeName">模块分类名称</param>
        /// <returns></returns>
        public bool ModuleTypeExists(string ModuleTypeName)
        {
            return dal.ModuleTypeExists(ModuleTypeName);
        }

        /// <summary>
        /// 增加一个模块分类
        /// </summary>
        /// <param name="model">模块分类实体类</param>
        /// <returns></returns>
        public int CreateModuleType(RedGlovePermission.Model.RGP_ModuleType model)
        {
            return dal.CreateModuleType(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="model">模块分类实体类</param>
        /// <returns></returns>
        public bool UpdateModuleType(RedGlovePermission.Model.RGP_ModuleType model)
        {
            return dal.UpdateModuleType(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <param name="ModuleTypeID">模块分类ID</param>
        /// <returns></returns>
        public int DeleteModuleType(int ModuleTypeID)
        {
            return dal.DeleteModuleType(ModuleTypeID);
        }

        /// <summary>
        /// 得到一个模块分类实体
        /// </summary>
        /// <param name="ModuleTypeID">模块分类ID</param>
        /// <returns></returns>
        public RedGlovePermission.Model.RGP_ModuleType GetModuleTypeModel(int ModuleTypeID)
        {
            return dal.GetModuleTypeModel(ModuleTypeID);
        }

        /// <summary>
        /// 获得模块分类数据列表
        /// </summary>
        /// <param name="strWhere">Where条件</param>
        /// <param name="strOrder">排序条件</param>
        /// <returns></returns>
        public DataSet GetModuleTypeList(string strWhere)
        {
            return dal.GetModuleTypeList(strWhere);
        }

        #endregion

        #region 模块操作
        /// <summary>
        /// 判断模块是否存在
        /// </summary>
        /// <param name="ModuleName">模块名称</param>
        /// <returns></returns>
        public bool ModuleExists(string ModuleTag)
        {
            return dal.ModuleExists(ModuleTag);
        }

        /// <summary>
        /// 更新时判断模块是否存在
        /// </summary>
        /// <param name="ModuleID">模块ID</param>
        /// <param name="ModuleName">模块名称</param>
        /// <returns></returns>
        public bool UpdateExists(int ModuleID, string ModuleTag)
        {
            return dal.UpdateExists(ModuleID, ModuleTag);
        }

        /// <summary>
        /// 增加一个模块
        /// </summary>
        /// <param name="model">模块实体类</param>
        /// <returns></returns>
        public int CreateModule(RedGlovePermission.Model.RGP_Modules model)
        {
            return dal.CreateModule(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="model">模块实体类</param>
        /// <returns></returns>
        public int UpdateModule(RedGlovePermission.Model.RGP_Modules model)
        {
            return dal.UpdateModule(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <param name="ModuleID">模块ID</param>
        /// <returns></returns>
        public bool DeleteModule(int ModuleID)
        {
            return dal.DeleteModule(ModuleID);
        }

        /// <summary>
        /// 得到一个模块实体
        /// </summary>
        /// <param name="ModuleID">模块ID</param>
        /// <returns></returns>
        public RedGlovePermission.Model.RGP_Modules GetModuleModel(int ModuleID)
        {
            return dal.GetModuleModel(ModuleID);
        }

        /// <summary>
        /// 获得模块数据列表
        /// </summary>
        /// <param name="strWhere">Where条件</param>
        /// <param name="strOrder">排序条件</param>
        /// <returns></returns>
        public DataSet GetModuleList(string strWhere)
        {
            return dal.GetModuleList(strWhere);
        }

        /// <summary>
        /// 获取模块ID
        /// </summary>
        /// <param name="ModuleTag">模块标识</param>
        /// <returns></returns>
        public int GetModuleID(string ModuleTag)
        {
            return dal.GetModuleID(ModuleTag);
        }

        /// <summary>
        /// 模块是否关闭
        /// </summary>
        /// <param name="ModuleTag">模块标识</param>
        /// <returns></returns>
        public bool IsModule(string ModuleTag)
        {
            return dal.IsModule(ModuleTag);
        }

        #endregion

        #region 模块授权

        /// <summary>
        /// 增加模块权限
        /// </summary>
        /// <param name="list">权限列表</param>
        /// <returns></returns>
        public bool CreateAuthorityList(ArrayList list)
        {
            return dal.CreateAuthorityList(list);
        }

        /// <summary>
        /// 更新模块权限
        /// </summary>
        public bool UpdateAuthorityList(ArrayList list)
        {
            return dal.UpdateAuthorityList(list);
        }

        /// <summary>
        /// 删除指定模块的权限列表
        /// </summary>
        /// <param name="ModuleID">模块ID</param>
        /// <returns></returns>
        public bool DeleteAuthority(int ModuleID)
        {
            return dal.DeleteAuthority(ModuleID);
        }

        /// <summary>
        /// 获得指定模块的权限列表
        /// </summary>
        /// <param name="ModuleID">模块ID</param>
        /// <returns></returns>
        public DataSet GetAuthorityList(int ModuleID)
        {
            return dal.GetAuthorityList(ModuleID);
        }

        /// <summary>
        /// 获得指定模块的权限列表
        /// </summary>
        /// <param name="ModuleID">模块ID</param>
        /// <returns></returns>
        public DataSet GetAuthorityNameList(int ModuleID)
        {
            return dal.GetAuthorityNameList(ModuleID);
        }

        #endregion
    }
}

