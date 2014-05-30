using System;
namespace RedGlovePermission.Model
{
	/// <summary>
	/// 实体类RGP_UserAuthorityList 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class RGP_RoleAuthorityList
	{
		public RGP_RoleAuthorityList()
		{}

		private int _id;
        private int _userid;
		private int _roleid;
		private int _moduleid;
		private string _authoritytag;
        private bool _flag;
		/// <summary>
		/// 编号
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserID
        {
            set { _userid = value; }
            get { return _userid; }
        }
		/// <summary>
		/// 角色ID
		/// </summary>
		public int RoleID
		{
			set{ _roleid=value;}
			get{return _roleid;}
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
        /// <summary>
        /// 判断是正向还是反向操作，在给用户单独授权时做判段
        /// 1为允许，0为不禁止
        /// </summary>
        public bool Flag
        {
            set { _flag = value; }
            get { return _flag; }
        }

	}
}

