using System;
namespace RedGlovePermission.Model
{
	/// <summary>
	/// 实体类RGP_Modules 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class RGP_Modules
	{
		public RGP_Modules()
		{}
		#region Model
		private int _moduleid;
		private int _moduletypeid;
		private string _modulename;
		private string _moduletag;
		private string _moduleurl;
		private bool _moduledisabled;
		private int _moduleorder;
		private string _moduledescription;
        private bool _ismenu;
		/// <summary>
		/// 模块ID
		/// </summary>
		public int ModuleID
		{
			set{ _moduleid=value;}
			get{return _moduleid;}
		}
		/// <summary>
		/// 模块类型
		/// </summary>
		public int ModuleTypeID
		{
			set{ _moduletypeid=value;}
			get{return _moduletypeid;}
		}
		/// <summary>
		/// 模块名称
		/// </summary>
		public string ModuleName
		{
			set{ _modulename=value;}
			get{return _modulename;}
		}
		/// <summary>
		/// 模块标识
		/// </summary>
		public string ModuleTag
		{
			set{ _moduletag=value;}
			get{return _moduletag;}
		}
		/// <summary>
		/// 模块地址
		/// </summary>
		public string ModuleURL
		{
			set{ _moduleurl=value;}
			get{return _moduleurl;}
		}
		/// <summary>
		/// 是否禁用
		/// </summary>
		public bool ModuleDisabled
		{
			set{ _moduledisabled=value;}
			get{return _moduledisabled;}
		}
		/// <summary>
		/// 排序
		/// </summary>
		public int ModuleOrder
		{
			set{ _moduleorder=value;}
			get{return _moduleorder;}
		}
		/// <summary>
		/// 说明
		/// </summary>
		public string ModuleDescription
		{
			set{ _moduledescription=value;}
			get{return _moduledescription;}
		}
        /// <summary>
        /// 是否显示在导航菜单中
        /// </summary>
        public bool IsMenu
        {
            set { _ismenu = value; }
            get { return _ismenu; }
        }
		#endregion Model

	}
}

