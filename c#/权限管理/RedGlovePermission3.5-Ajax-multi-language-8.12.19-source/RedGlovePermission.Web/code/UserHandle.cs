using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RedGlovePermission.Web
{
     /// <summary>
    /// 用户权限，功能处理类
    /// </summary>
    public class UserHandle
    {
        public UserHandle(){}

        /// <summary>
        /// 初始化模块权限
        /// </summary>
        /// <param name="ModuleTag">模块标识</param>
        public static void InitModule(string ModuleTag)
        {
            RedGlovePermission.BLL.RGP_Modules bll = new RedGlovePermission.BLL.RGP_Modules();
            RedGlovePermission.BLL.RGP_Roles Rolebll = new RedGlovePermission.BLL.RGP_Roles();
            //判断模块是否启用
            if (bll.IsModule(ModuleTag))
            {
                ArrayList Mlists = new ArrayList();//模块权限
                ArrayList Ulists = new ArrayList();//用户的模块权限

                //读取模块权限
                int id = bll.GetModuleID(ModuleTag);
                DataSet MALds = bll.GetAuthorityList(id);

                for (int i = 0; i < MALds.Tables[0].Rows.Count; i++)
                {
                    Mlists.Add(MALds.Tables[0].Rows[i]["AuthorityTag"].ToString());
                }
                SessionBox.RemoveModuleList();              //先清空Session中的列表
                SessionBox.CreateModuleList(Mlists);        //登记新的列表

                //读取用户角色拥有的该模块权限
                DataSet RALds = Rolebll.GetRoleAuthorityList(SessionBox.GetUserSession().RoleID, id);
                for (int i = 0; i < RALds.Tables[0].Rows.Count; i++)
                {
                    Ulists.Add(RALds.Tables[0].Rows[i]["AuthorityTag"].ToString());
                }
                SessionBox.RemoveAuthority();
                SessionBox.CreateAuthority(Ulists);
            }
            else
            {
                throw new Exception("此功能不存在");
            }
        }

        /// <summary>
        /// 校验用户是否对模块有该权限
        /// </summary>
        /// <param name="tag">权限标识</param>
        /// <returns></returns>
        public static bool ValidationHandle(string ModuleTag)
        {
            bool ret = false;
            if (!SessionBox.GetUserSession().IsLimit) //判断用户是否受权限限制
            {
                ArrayList Mlist = SessionBox.GetModuleList();
                ArrayList Ulist = SessionBox.GetAuthority();

                for (int i = 0; i < Mlist.Count; i++)
                {
                    if (Mlist[i].ToString() == ModuleTag)//是否在模块存在
                    {
                        for (int j = 0; j < Ulist.Count; j++)
                        {
                            if (Ulist[j].ToString() == ModuleTag)//是否在用户权限表中
                            {
                                ret = true;
                            }
                        }
                    }
                }
            }
            else
            {
                ret = true;
            }
            return ret;
        }

        /// <summary>
        /// 判断是否有模块访问权限
        /// </summary>
        /// <param name="ModuleID">模块ID</param>
        /// <param name="AuthorityTag">权限标识</param>
        /// <returns></returns>
        public static bool ValidationModule(int ModuleID, string AuthorityTag)
        {
            RedGlovePermission.BLL.RGP_Roles bll = new RedGlovePermission.BLL.RGP_Roles();
            RedGlovePermission.Model.RGP_RoleAuthorityList model= new RedGlovePermission.Model.RGP_RoleAuthorityList();
            model.UserID = 0;
            model.RoleID = SessionBox.GetUserSession().RoleID;
            model.ModuleID = ModuleID;
            model.AuthorityTag = AuthorityTag;
            return bll.RoleAuthorityExists(model);
        }

    }
}
