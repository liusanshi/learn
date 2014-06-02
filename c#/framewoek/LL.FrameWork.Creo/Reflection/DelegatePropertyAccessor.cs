using System;
using System.Reflection;
using System.Reflection.Emit;

using Castle.DynamicProxy;

namespace LL.FrameWork.Core.Reflection
{
    public class DelegatePropertyAccessor : IPropertyAccessor
    {
        private Func<object, object> PropertyGet;
        private Action<object, object> PropertySet;

        //private IMethodInvoker PropertyGet;
        //private IMethodInvoker PropertySet;
        public DelegatePropertyAccessor(PropertyInfo property)
        {
            init(property);
        }

        void init(PropertyInfo property)
        {
            if (property == null)
            {
                throw new ArgumentException("Argument: property is null");
            }

            //PropertyGet = new DelegateMethodInvoker(property.GetGetMethod());
            //PropertySet = new DelegateMethodInvoker(property.GetSetMethod());

            var name = ReflectionHelp.GetMemberSignName(property);
            var type = property.DeclaringType;

            DynamicMethod method = new DynamicMethod(name + "_get", ReflectionHelp.ObjectType, new Type[] { ReflectionHelp.ObjectType });
            var il = method.GetILGenerator();
            ReflectionHelp.ILLdarg(il, 0);
            ReflectionHelp.ILCastclass(il, ReflectionHelp.ObjectType, type);
            il.Emit(OpCodes.Callvirt, property.GetGetMethod());
            ReflectionHelp.ILCastclass(il, property.PropertyType, ReflectionHelp.ObjectType);
            il.Emit(OpCodes.Ret);
            PropertyGet = (Func<object, object>)method.CreateDelegate(typeof(Func<object, object>));

            method = new DynamicMethod(name + "_set", ReflectionHelp.VoidType, new Type[] { ReflectionHelp.ObjectType, ReflectionHelp.ObjectType });
            il = method.GetILGenerator();
            ReflectionHelp.ILLdarg(il, 0);
            ReflectionHelp.ILCastclass(il, ReflectionHelp.ObjectType, type);
            ReflectionHelp.ILLdarg(il, 1);
            ReflectionHelp.ILCastclass(il, ReflectionHelp.ObjectType, property.PropertyType);
            il.Emit(OpCodes.Callvirt, property.GetSetMethod());
            il.Emit(OpCodes.Ret);

            PropertySet = (Action<object, object>)method.CreateDelegate(typeof(Action<object, object>));
        }

        public object Get(object Target)
        {
            return PropertyGet.Invoke(Target);
        }

        public void Set(object Target, object Value)
        {
            PropertySet.Invoke(Target, Value);
        }
        object IPropertyAccessor.Get(object Target)
        {
            return this.Get(Target);
        }

        void IPropertyAccessor.Set(object Target, object Value)
        {
            this.Set(Target, Value);
        }
    }

    public class DelegateProertyReflectionFactory : IFastReflectionFactory<PropertyInfo, IPropertyAccessor>
    {

        public IPropertyAccessor Create(PropertyInfo key)
        {
            return new DelegatePropertyAccessor(key);
        }
        IPropertyAccessor IFastReflectionFactory<PropertyInfo, IPropertyAccessor>.Create(PropertyInfo key)
        {
            return this.Create(key);
        }
    }
}
