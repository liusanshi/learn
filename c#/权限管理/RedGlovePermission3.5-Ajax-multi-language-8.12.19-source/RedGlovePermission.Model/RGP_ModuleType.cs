using System;
namespace RedGlovePermission.Model
{
	/// <summary>
	/// 实体类RGP_ModuleType 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	public class RGP_ModuleType
	{
		public RGP_ModuleType()
		{}
		#region Model
		private int _moduletypeid;
		private string _moduletypename;
		private int _moduletypeorder;
		private string _moduletypedescription;
		/// <summary>
		/// 模块类型ID
		/// </summary>
		public int ModuleTypeID
		{
			set{ _moduletypeid=value;}
			get{return _moduletypeid;}
		}
		/// <summary>
		/// 模块类型名称
		/// </summary>
		public string ModuleTypeName
		{
			set{ _moduletypename=value;}
			get{return _moduletypename;}
		}
		/// <summary>
		/// 排序
		/// </summary>
		public int ModuleTypeOrder
		{
			set{ _moduletypeorder=value;}
			get{return _moduletypeorder;}
		}
		/// <summary>
		/// 说明
		/// </summary>
		public string ModuleTypeDescription
		{
			set{ _moduletypedescription=value;}
			get{return _moduletypedescription;}
		}
		#endregion Model

	}
}

