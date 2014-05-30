using System;
namespace RedGlovePermission.Model
{
	/// <summary>
	/// 实体类RGP_AuthorityDir 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class RGP_AuthorityDir
	{
		public RGP_AuthorityDir()
		{}
		#region Model
		private int _authorityid;
		private string _authorityname;
		private string _authoritytag;
		private string _authoritydescription;
		private int _authorityorder;
		/// <summary>
		/// 权限ID
		/// </summary>
		public int AuthorityID
		{
			set{ _authorityid=value;}
			get{return _authorityid;}
		}
		/// <summary>
		/// 权限名称
		/// </summary>
		public string AuthorityName
		{
			set{ _authorityname=value;}
			get{return _authorityname;}
		}
		/// <summary>
		/// 权限标识
		/// </summary>
		public string AuthorityTag
		{
			set{ _authoritytag=value;}
			get{return _authoritytag;}
		}
		/// <summary>
		/// 说明
		/// </summary>
		public string AuthorityDescription
		{
			set{ _authoritydescription=value;}
			get{return _authoritydescription;}
		}
		/// <summary>
		/// 排序
		/// </summary>
		public int AuthorityOrder
		{
			set{ _authorityorder=value;}
			get{return _authorityorder;}
		}
		#endregion Model

	}
}

