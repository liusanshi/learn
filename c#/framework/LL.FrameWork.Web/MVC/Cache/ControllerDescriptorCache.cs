using System;
using System.Collections.Generic;
using System.Reflection;

namespace LL.FrameWork.Web.MVC
{
    internal class ControllerDescriptorCache : ReaderWriterCache<Type, ControllerDescriptor>
    {
        public ControllerDescriptorCache()
            : base(new TypeEqualityComparer())
        { }

        public ControllerDescriptor GetDescriptor(Type type)
        {
            return base.FetchOrCreateItem(type, () => new ControllerDescriptor(type));
        }

        class TypeEqualityComparer : IEqualityComparer<Type>
        {
            public bool Equals(Type x, Type y)
            {
                if (x == null || y == null) return false;
                return x == y;
            }

            public int GetHashCode(Type obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}
