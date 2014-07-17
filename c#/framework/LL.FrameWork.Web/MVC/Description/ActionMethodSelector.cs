using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LL.FrameWork.Web.MVC
{
    internal sealed class ActionMethodSelector
    {
        public Type ControllerType
        {
            get;
            private set;
        }
        public MethodInfo[] AliasedMethods
        {
            get;
            private set;
        }
        public ILookup<string, MethodInfo> NonAliasedMethods
        {
            get;
            private set;
        }
        public ActionMethodSelector(Type controllerType)
        {
            this.ControllerType = controllerType;
            this.PopulateLookupTables();
        }
        private AmbiguousMatchException CreateAmbiguousMatchException(List<MethodInfo> ambiguousMethods, string actionName)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (MethodInfo current in ambiguousMethods)
            {
                string text = Convert.ToString(current, CultureInfo.CurrentCulture);
                string fullName = current.DeclaringType.FullName;
                stringBuilder.AppendLine();
                stringBuilder.AppendFormat(CultureInfo.CurrentCulture, "{0} on type {1}", new object[]
				{
					text,
					fullName
				});
            }
            string message = string.Format(CultureInfo.CurrentCulture, "The current request for action '{0}' on controller type '{1}' is ambiguous between the following action methods:{2}", new object[]
			{
				actionName,
				this.ControllerType.Name,
				stringBuilder
			});
            return new AmbiguousMatchException(message);
        }
        /// <summary>
        /// 查找指定动作的方法
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public MethodInfo FindActionMethod(ControllerContext controllerContext, string actionName)
        {
            if (controllerContext.RouteData.UsePageUrlRoute)
            {
                var data = controllerContext.RouteData.PageUrlData;
                if (data == null) return null;
                return data.Item2;
            }
            List<MethodInfo> matchingAliasedMethods = this.GetMatchingAliasedMethods(controllerContext, actionName);
            matchingAliasedMethods.AddRange(this.NonAliasedMethods[actionName]);
            List<MethodInfo> list = ActionMethodSelector.RunSelectionFilters(controllerContext, matchingAliasedMethods);
            switch (list.Count)
            {
                case 0:
                    // 如果Action的名字是submit并且是POST提交，则需要自动寻找Action
                    // 例如：多个提交都采用一样的方式：POST /Ajax/Product/submit
                    if (actionName.IsSame("submit") && controllerContext.HttpContext.Request.HttpMethod.IsSame("POST"))
                    {
                        return FindSubmitAction(controllerContext);
                    }
                    return null;
                case 1:
                    return list[0];
                default:
                    throw this.CreateAmbiguousMatchException(list, actionName);
            }
        }
        /// <summary>
        /// 查找别名匹配的方法
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="actionName"></param>
        /// <returns></returns>
        internal List<MethodInfo> GetMatchingAliasedMethods(ControllerContext controllerContext, string actionName)
        {
            IEnumerable<MethodInfo> source =
                from methodInfo in this.AliasedMethods
                let attrs = ReflectedAttributeCache.GetActionNameSelectorAttributes(methodInfo)
                where attrs.All((ActionNameSelectorAttribute attr) => attr.IsValidName(controllerContext, actionName, methodInfo))
                select methodInfo;
            return source.ToList<MethodInfo>();
        }
        /// <summary>
        /// 查找自动Submit的方法
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public MethodInfo FindSubmitAction(ControllerContext controllerContext)
        {
            string[] keys = controllerContext.HttpContext.Request.Form.AllKeys;
            List<MethodInfo> matchingAliasedMethods = new List<MethodInfo>(0);
            string actionName = string.Empty;
            foreach (var item in keys)
            {
                matchingAliasedMethods = GetMatchingAliasedMethods(controllerContext, item);
                if (matchingAliasedMethods.Any())
                {
                    actionName = item;
                    break;
                }
            }
            matchingAliasedMethods.AddRange(this.NonAliasedMethods[actionName]);
            List<MethodInfo> list = ActionMethodSelector.RunSelectionFilters(controllerContext, matchingAliasedMethods);
            switch (list.Count)
            {
                case 0:
                    return null;
                case 1:
                    return list[0];
                default:
                    throw this.CreateAmbiguousMatchException(list, actionName);
            }
        }

        //public MethodInfo 

        /// <summary>
        /// 方法是否声明了动作别名特性
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <returns></returns>
        private static bool IsMethodDecoratedWithAliasingAttribute(MethodInfo methodInfo)
        {
            return methodInfo.IsDefined(typeof(ActionNameSelectorAttribute), true);
        }
        /// <summary>
        /// 验证方法
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <returns></returns>
        private static bool IsValidActionMethod(MethodInfo methodInfo)
        {
            return !methodInfo.IsSpecialName && !methodInfo.GetBaseDefinition().DeclaringType.IsAssignableFrom(typeof(ControllerBase));
        }
        /// <summary>
        /// 初始化元数据
        /// </summary>
        private void PopulateLookupTables()
        {
            MethodInfo[] methods = this.ControllerType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod);
            MethodInfo[] array = Array.FindAll<MethodInfo>(methods, new Predicate<MethodInfo>(ActionMethodSelector.IsValidActionMethod));
            this.AliasedMethods = Array.FindAll<MethodInfo>(array, new Predicate<MethodInfo>(ActionMethodSelector.IsMethodDecoratedWithAliasingAttribute));
            this.NonAliasedMethods = array.Except(this.AliasedMethods).ToLookup((MethodInfo method) => method.Name, StringComparer.OrdinalIgnoreCase);
        }
        /// <summary>
        /// 筛选动作的执行者
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="methodInfos"></param>
        /// <returns></returns>
        private static List<MethodInfo> RunSelectionFilters(ControllerContext controllerContext, List<MethodInfo> methodInfos)
        {
            List<MethodInfo> list = new List<MethodInfo>();
            List<MethodInfo> list2 = new List<MethodInfo>();
            foreach (MethodInfo methodInfo in methodInfos)
            {
                ICollection<ActionMethodSelectorAttribute> actionMethodSelectorAttributes = ReflectedAttributeCache.GetActionMethodSelectorAttributes(methodInfo);
                if (actionMethodSelectorAttributes.Count == 0)
                {
                    list2.Add(methodInfo);
                }
                else
                {
                    if (actionMethodSelectorAttributes.All((ActionMethodSelectorAttribute attr) => attr.IsValidForRequest(controllerContext, methodInfo)))
                    {
                        list.Add(methodInfo);
                    }
                }
            }
            if (list.Count <= 0)
            {
                return list2;
            }
            return list;
        }
    }
}
