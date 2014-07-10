using System;
using System.Web.UI;
using System.CodeDom;

namespace LL.FrameWork.Web.MVC
{
    /// <summary>
    /// 一个基于“System.Web.UI.Page”的类
    /// </summary>
    [FileLevelControlBuilder(typeof(ViewPageControlBuilder))]
    public class MyBasePage : Page
    {
        private string _requestUrlEncodeRawUrl;
        /// <summary>
        /// 当前页面的请求地址。
        /// 已经过UrlEncode()处理，用于构造URL中的一部分。
        /// </summary>
        public string RequestUrlEncodeRawUrl
        {
            get
            {
                if (_requestUrlEncodeRawUrl == null)
                    _requestUrlEncodeRawUrl = Server.UrlEncode(HttpContextHelper.RequestRawUrl);
                return _requestUrlEncodeRawUrl;
            }
        }

        public virtual void SetModel(object model)
        {
        }
    }


    internal sealed class ViewPageControlBuilder : FileLevelPageControlBuilder
    {
        public string PageBaseType
        {
            get;
            set;
        }

        public override void ProcessGeneratedCode(
            CodeCompileUnit codeCompileUnit,
            CodeTypeDeclaration baseType,
            CodeTypeDeclaration derivedType,
            CodeMemberMethod buildMethod,
            CodeMemberMethod dataBindingMethod)
        {
            // 如果分析器找到一个有效的类型，就使用它。
            if (!string.IsNullOrEmpty(PageBaseType))
            {
                derivedType.BaseTypes[0] = new CodeTypeReference(PageBaseType);
            }
        }
    }
}
