using System;
namespace RedGlovePermission.Model
{
	/// <summary>
	/// 实体类RGP_ModuleAuthorityList 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class RGP_ModuleAuthorityList
	{
		public RGP_ModuleAuthorityList()
		{}
		#region Model
		private int _id;
		private int _moduleid;
		private string _authoritytag;
		/// <summary>
		/// 
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 模块ID
		/// </summary>
		public int ModuleID
		{
			set{ _moduleid=value;}
			get{return _moduleid;}
		}
		/// <summary>
		/// 权限标识
		/// </summary>
		public string AuthorityTag
		{
			set{ _authoritytag=value;}
			get{return _authoritytag;}
		}
		#endregion Model

	}
}

