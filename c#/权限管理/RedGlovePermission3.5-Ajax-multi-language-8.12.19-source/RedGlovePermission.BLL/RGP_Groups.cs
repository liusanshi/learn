using System;
using System.Data;
using System.Collections.Generic;

using RedGlovePermission.Lib;
using RedGlovePermission.Model;
using RedGlovePermission.DALFactory;
using RedGlovePermission.IDAL;

namespace RedGlovePermission.BLL
{
	/// <summary>
	/// 业务逻辑类RGP_Groups 的摘要说明。
	/// </summary>
	public class RGP_Groups
	{
        private readonly IRGP_Groups dal = DataAccess.CreateRGP_Groups();

		public RGP_Groups()
		{}

        /// <summary>
        /// 判断分组是否存在
        /// </summary>
        /// <param name="GroupName">分组名称</param>
        /// <returns></returns>
        public bool Exists(string GroupName)
        {
            return dal.Exists(GroupName);
        }

        /// <summary>
        /// 增加一个分组
        /// </summary>
        /// <param name="model">分组实体类</param>
        /// <returns></returns>
        public bool CreateGroup(RedGlovePermission.Model.RGP_Groups model)
        {
            return dal.CreateGroup(model);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="model">分组实体类</param>
        /// <returns></returns>
        public bool UpdateGroup(RedGlovePermission.Model.RGP_Groups model)
        {
            return dal.UpdateGroup(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <param name="GroupID">分组ID</param>
        /// <returns></returns>
        public int DeleteGroup(int GroupID)
        {
            return dal.DeleteGroup(GroupID);
        }

        /// <summary>
        /// 得到一个分组实体
        /// </summary>
        /// <param name="GroupID">分组ID</param>
        /// <returns></returns>
        public RedGlovePermission.Model.RGP_Groups GetGroupModel(int GroupID)
        {
            return dal.GetGroupModel(GroupID);
        }

        /// <summary>
        /// 获得分组数据列表
        /// </summary>
        /// <param name="strWhere">Where条件</param>
        /// <param name="strOrder">排序条件</param>
        /// <returns></returns>
        public DataSet GetGroupList(string strWhere, string strOrder)
        {
            return dal.GetGroupList(strWhere, strOrder);
        }
    }
}

