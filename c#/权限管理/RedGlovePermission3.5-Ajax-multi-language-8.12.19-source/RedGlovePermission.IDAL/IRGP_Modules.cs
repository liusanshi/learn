using System;
using System.Collections;
using System.Data;

namespace RedGlovePermission.IDAL
{
    public interface IRGP_Modules
	{

        #region 模块分类

        /// <summary>
        /// 判断模块分类是否存在
        /// </summary>
        /// <param name="ModuleTypeName">模块分类名称</param>
        /// <returns></returns>
        bool ModuleTypeExists(string ModuleTypeName);

        /// <summary>
        /// 增加一个模块分类
        /// </summary>
        /// <param name="model">模块分类实体类</param>
        /// <returns></returns>
        int CreateModuleType(RedGlovePermission.Model.RGP_ModuleType model);

        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="model">模块分类实体类</param>
        /// <returns></returns>
        bool UpdateModuleType(RedGlovePermission.Model.RGP_ModuleType model);

        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <param name="ModuleTypeID">模块分类ID</param>
        /// <returns></returns>
        int DeleteModuleType(int ModuleTypeID);

        /// <summary>
        /// 得到一个模块分类实体
        /// </summary>
        /// <param name="ModuleTypeID">模块分类ID</param>
        /// <returns></returns>
        RedGlovePermission.Model.RGP_ModuleType GetModuleTypeModel(int ModuleTypeID);

        /// <summary>
        /// 获得模块分类数据列表
        /// </summary>
        /// <param name="strWhere">Where条件</param>
        /// <param name="strOrder">排序条件</param>
        /// <returns></returns>
        DataSet GetModuleTypeList(string strWhere);

        #endregion

        #region 模块操作
        /// <summary>
        /// 判断模块是否存在
        /// </summary>
        /// <param name="ModuleName">模块名称</param>
        /// <returns></returns>
        bool ModuleExists(string ModuleTag);

        /// <summary>
        /// 更新时判断模块是否存在
        /// </summary>
        /// <param name="ModuleID">模块ID</param>
        /// <param name="ModuleName">模块名称</param>
        /// <returns></returns>
        bool UpdateExists(int ModuleID, string ModuleTag);

        /// <summary>
        /// 增加一个模块
        /// </summary>
        /// <param name="model">模块实体类</param>
        /// <returns></returns>
        int CreateModule(RedGlovePermission.Model.RGP_Modules model);

        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="model">模块实体类</param>
        /// <returns></returns>
        int UpdateModule(RedGlovePermission.Model.RGP_Modules model);

        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <param name="ModuleID">模块ID</param>
        /// <returns></returns>
        bool DeleteModule(int ModuleID);

        /// <summary>
        /// 得到一个模块实体
        /// </summary>
        /// <param name="ModuleID">模块ID</param>
        /// <returns></returns>
        RedGlovePermission.Model.RGP_Modules GetModuleModel(int ModuleID);

        /// <summary>
        /// 获得模块数据列表
        /// </summary>
        /// <param name="strWhere">Where条件</param>
        /// <param name="strOrder">排序条件</param>
        /// <returns></returns>
        DataSet GetModuleList(string strWhere);

        /// <summary>
        /// 获取模块ID
        /// </summary>
        /// <param name="ModuleTag">模块标识</param>
        /// <returns></returns>
        int GetModuleID(string ModuleTag);

        /// <summary>
        /// 模块是否关闭
        /// </summary>
        /// <param name="ModuleTag">模块标识</param>
        /// <returns></returns>
        bool IsModule(string ModuleTag);

        #endregion

        #region 模块授权

        /// <summary>
        /// 增加模块权限
        /// </summary>
        /// <param name="list">权限列表</param>
        /// <returns></returns>
        bool CreateAuthorityList(ArrayList list);

        /// <summary>
        /// 更新模块权限
        /// </summary>
        bool UpdateAuthorityList(ArrayList list);

        /// <summary>
        /// 删除指定模块的权限列表
        /// </summary>
        /// <param name="ModuleID">模块ID</param>
        /// <returns></returns>
        bool DeleteAuthority(int ModuleID);

        /// <summary>
        /// 获得指定模块的权限列表
        /// </summary>
        /// <param name="ModuleID">模块ID</param>
        /// <returns></returns>
        DataSet GetAuthorityList(int ModuleID);

        /// <summary>
        /// 获得指定模块的权限列表
        /// </summary>
        /// <param name="ModuleID">模块ID</param>
        /// <returns></returns>
        DataSet GetAuthorityNameList(int ModuleID);

        #endregion
    }
}

