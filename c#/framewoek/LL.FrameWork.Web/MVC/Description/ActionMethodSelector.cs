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
        public MethodInfo FindActionMethod(ControllerContext controllerContext, string actionName)
        {
            List<MethodInfo> matchingAliasedMethods = this.GetMatchingAliasedMethods(controllerContext, actionName);
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
        internal List<MethodInfo> GetMatchingAliasedMethods(ControllerContext controllerContext, string actionName)
        {
            IEnumerable<MethodInfo> source =
                from methodInfo in this.AliasedMethods
                let attrs = ReflectedAttributeCache.GetActionNameSelectorAttributes(methodInfo)
                where attrs.All((ActionNameSelectorAttribute attr) => attr.IsValidName(controllerContext, actionName, methodInfo))
                select methodInfo;
            return source.ToList<MethodInfo>();
        }
        private static bool IsMethodDecoratedWithAliasingAttribute(MethodInfo methodInfo)
        {
            return methodInfo.IsDefined(typeof(ActionNameSelectorAttribute), true);
        }
        private static bool IsValidActionMethod(MethodInfo methodInfo)
        {
            return !methodInfo.IsSpecialName && !methodInfo.GetBaseDefinition().DeclaringType.IsAssignableFrom(typeof(ControllerBase));
        }
        private void PopulateLookupTables()
        {
            MethodInfo[] methods = this.ControllerType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod);
            MethodInfo[] array = Array.FindAll<MethodInfo>(methods, new Predicate<MethodInfo>(ActionMethodSelector.IsValidActionMethod));
            this.AliasedMethods = Array.FindAll<MethodInfo>(array, new Predicate<MethodInfo>(ActionMethodSelector.IsMethodDecoratedWithAliasingAttribute));
            this.NonAliasedMethods = array.Except(this.AliasedMethods).ToLookup((MethodInfo method) => method.Name, StringComparer.OrdinalIgnoreCase);
        }
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
