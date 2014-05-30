using System;
namespace RedGlovePermission.Model
{
	/// <summary>
	/// 实体类RGP_Roles 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class RGP_Roles
	{
		public RGP_Roles()
		{}
		#region Model
		private int _roleid;
		private int _rolegroupid;
		private string _rolename;
		private string _roledescription;
		/// <summary>
		/// 角色ID
		/// </summary>
		public int RoleID
		{
			set{ _roleid=value;}
			get{return _roleid;}
		}
		/// <summary>
		/// 分组ID
		/// </summary>
		public int RoleGroupID
		{
			set{ _rolegroupid=value;}
			get{return _rolegroupid;}
		}
		/// <summary>
		/// 角色名称
		/// </summary>
		public string RoleName
		{
			set{ _rolename=value;}
			get{return _rolename;}
		}
		/// <summary>
		/// 说明
		/// </summary>
		public string RoleDescription
		{
			set{ _roledescription=value;}
			get{return _roledescription;}
		}
		#endregion Model

	}
}

