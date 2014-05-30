using System;
namespace RedGlovePermission.Model
{
	/// <summary>
	/// 实体类Users 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class Users
	{
		public Users()
		{}
		#region Model
		private int _userid;
		private string _username;
		private string _password;
		private string _question;
		private string _answer;
		private int _roleid;
		private int _usergroup;
		private DateTime _createtime;
		private DateTime _lastlogintime;
		private int _status;
		private bool _isonline;
		private bool _islimit;
		/// <summary>
		/// 用户ID
		/// </summary>
		public int UserID
		{
			set{ _userid=value;}
			get{return _userid;}
		}
		/// <summary>
		/// 登录名，用户Email
		/// </summary>
		public string UserName
		{
			set{ _username=value;}
			get{return _username;}
		}
		/// <summary>
		/// 密码
		/// </summary>
		public string Password
		{
			set{ _password=value;}
			get{return _password;}
		}
		/// <summary>
		/// 重置密码的问题
		/// </summary>
		public string Question
		{
			set{ _question=value;}
			get{return _question;}
		}
		/// <summary>
		/// 重置密码的答案
		/// </summary>
		public string Answer
		{
			set{ _answer=value;}
			get{return _answer;}
		}
		/// <summary>
		/// 角色
		/// </summary>
		public int RoleID
		{
			set{ _roleid=value;}
			get{return _roleid;}
		}
		/// <summary>
		/// 用户组
		/// </summary>
		public int UserGroup
		{
			set{ _usergroup=value;}
			get{return _usergroup;}
		}
		/// <summary>
		/// 帐户创建时间
		/// </summary>
		public DateTime CreateTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		/// <summary>
		/// 上一次登录的时间
		/// </summary>
		public DateTime LastLoginTime
		{
			set{ _lastlogintime=value;}
			get{return _lastlogintime;}
		}
		/// <summary>
		/// 用户状态
		/// </summary>
		public int Status
		{
			set{ _status=value;}
			get{return _status;}
		}
		/// <summary>
		/// 是否在线
		/// </summary>
		public bool IsOnline
		{
			set{ _isonline=value;}
			get{return _isonline;}
		}
		/// <summary>
		/// 是否受权限限制，0为受限制
		/// </summary>
		public bool IsLimit
		{
			set{ _islimit=value;}
			get{return _islimit;}
		}
		#endregion Model

	}
}

