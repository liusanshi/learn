using System;
using System.Collections.Generic;
using System.Reflection;

namespace LL.FrameWork.Web.MVC
{
    internal class ActionDescriptorCache : ReaderWriterCache<MethodInfo, ActionDescriptor>
    {
        public ActionDescriptorCache()
            : base(new MethodEqualityComparer())
        { }

        public ActionDescriptor GetDescriptor(MethodInfo methodInfo)
        {
            return base.FetchOrCreateItem(methodInfo, () => new ActionDescriptor(methodInfo));
        }

        class MethodEqualityComparer : IEqualityComparer<MethodInfo>
        {
            public bool Equals(MethodInfo x, MethodInfo y)
            {
                if (x == null || y == null) return false;
                return x == y;
            }

            public int GetHashCode(MethodInfo obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}
