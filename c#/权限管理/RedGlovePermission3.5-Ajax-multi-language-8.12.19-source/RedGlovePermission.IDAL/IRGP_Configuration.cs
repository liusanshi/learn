using System;
using System.Data;

namespace RedGlovePermission.IDAL
{
    public interface IRGP_Configuration
    {       
        /// <summary>
        /// 判断配置项是否存在
        /// </summary>
        /// <param name="ItemName">配置项名称</param>
        /// <returns></returns>
        bool Exists(string ItemName);

        /// <summary>
        /// 创建新配置
        /// </summary>
        /// <param name="ItemName">配置名称</param>
        /// <param name="ItemValue">配置值</param>
        /// <returns></returns>
        bool CreateItem(string ItemName, string ItemValue);

        /// <summary>
        /// 更新配置
        /// </summary>
        /// <param name="id">配置项ID</param>
        /// <param name="ItemName">配置名称</param>
        /// <param name="ItemValue">配置值</param>
        /// <returns></returns>
        bool UpdateItem(int id, string ItemName, string ItemValue);

        /// <summary>
        /// 更新配置值
        /// </summary>
        /// <param name="id">配置项ID</param>
        /// <param name="ItemValue">配置值</param>
        /// <returns></returns>
        bool UpdateItem(int id, string ItemValue);

        /// <summary>
        /// 删除配置项
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        bool DeleteItem(int id);

        /// <summary>
        /// 获取配置项的址
        /// </summary>
        /// <param name="ItemName">配置项名称</param>
        /// <returns></returns>
        string GetItemValue(string ItemName);

        /// <summary>
        /// 获取配置的列表
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        DataSet GetItemList(string strWhere);
    }
}
