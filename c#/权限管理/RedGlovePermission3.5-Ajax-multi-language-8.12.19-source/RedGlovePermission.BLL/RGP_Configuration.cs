using System;
using System.Data;
using System.Collections.Generic;

using RedGlovePermission.Lib;
using RedGlovePermission.Model;
using RedGlovePermission.DALFactory;
using RedGlovePermission.IDAL;

namespace RedGlovePermission.BLL
{
    public class RGP_Configuration
    {
        private readonly IRGP_Configuration dal = DataAccess.CreateRGP_Configuration();

        public RGP_Configuration() { }

        /// <summary>
        /// 判断配置项是否存在
        /// </summary>
        /// <param name="ItemName">配置项名称</param>
        /// <returns></returns>
        public bool Exists(string ItemName)
        {
            return dal.Exists(ItemName);
        }

        /// <summary>
        /// 创建新配置
        /// </summary>
        /// <param name="ItemName">配置名称</param>
        /// <param name="ItemValue">配置值</param>
        /// <returns></returns>
        public bool CreateItem(string ItemName, string ItemValue)
        {
            return dal.CreateItem(ItemName, ItemValue);
        }

        /// <summary>
        /// 更新配置
        /// </summary>
        /// <param name="id">配置项ID</param>
        /// <param name="ItemName">配置名称</param>
        /// <param name="ItemValue">配置值</param>
        /// <returns></returns>
        public bool UpdateItem(int id, string ItemName, string ItemValue)
        {
            return dal.UpdateItem(id, ItemName, ItemValue);
        }

        /// <summary>
        /// 更新配置值
        /// </summary>
        /// <param name="id">配置项ID</param>
        /// <param name="ItemValue">配置值</param>
        /// <returns></returns>
        public bool UpdateItem(int id, string ItemValue)
        {
            return dal.UpdateItem(id, ItemValue);
        }

        /// <summary>
        /// 删除配置项
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public bool DeleteItem(int id)
        {
            return dal.DeleteItem(id);
        }

        /// <summary>
        /// 获取配置项的址
        /// </summary>
        /// <param name="ItemName">配置项名称</param>
        /// <returns></returns>
        public string GetItemValue(string ItemName)
        {
            return dal.GetItemValue(ItemName);
        }

        /// <summary>
        /// 获取配置的列表
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataSet GetItemList(string strWhere)
        {
            return dal.GetItemList(strWhere);
        }
    }
}
