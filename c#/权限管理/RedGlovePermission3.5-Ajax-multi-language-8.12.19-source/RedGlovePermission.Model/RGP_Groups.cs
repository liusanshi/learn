using System;
namespace RedGlovePermission.Model
{
	/// <summary>
	/// 实体类RGP_Groups 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class RGP_Groups
	{
		public RGP_Groups()
		{}
		#region Model
		private int _groupid;
		private string _groupname;
		private int _grouporder;
		private string _groupdescription;
		/// <summary>
		/// 分组ID
		/// </summary>
		public int GroupID
		{
			set{ _groupid=value;}
			get{return _groupid;}
		}
		/// <summary>
		/// 组名称
		/// </summary>
		public string GroupName
		{
			set{ _groupname=value;}
			get{return _groupname;}
		}
		/// <summary>
		/// 排序
		/// </summary>
		public int GroupOrder
		{
			set{ _grouporder=value;}
			get{return _grouporder;}
		}
		/// <summary>
		/// 说明
		/// </summary>
		public string GroupDescription
		{
			set{ _groupdescription=value;}
			get{return _groupdescription;}
		}
		#endregion Model

	}
}

