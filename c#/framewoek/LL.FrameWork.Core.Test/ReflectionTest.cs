using System;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using LL.FrameWork.Core.Reflection;

namespace LL.FrameWork.Core.Test
{
    [TestClass]
    public class UnitTest1
    {
        Type type = typeof(Test);
        Test t = new Test();

        [TestMethod]
        public void TestFieldInt()
        {
            FieldInfo fieldInt = type.GetField("fieldInt");

            Assert.AreEqual(0, (int)fieldInt.FastGetValue(t));

            t.fieldInt = 10;
            Assert.AreEqual(10, (int)fieldInt.FastGetValue(t));

            fieldInt.FastSetValue(t, 20);
            Assert.AreEqual(20, (int)fieldInt.FastGetValue(t));
        }

        [TestMethod]
        public void TestFieldObj()
        {
            FieldInfo fieldInt = type.GetField("fieldObj");

            Assert.IsNull(fieldInt.FastGetValue(t));

            object o = new object();
            t.fieldObj = o;
            Assert.AreEqual(o, fieldInt.FastGetValue(t));

            t.fieldObj = null;
            fieldInt.FastSetValue(t, o);
            Assert.AreEqual(o, fieldInt.FastGetValue(t));
        }

        [TestMethod]
        public void TestPropertyInt()
        {
            PropertyInfo property = type.GetProperty("PropertyInt");
            Assert.AreEqual(0, (int)property.FastGetValue(t));

            t.PropertyInt = 10;
            Assert.AreEqual(10, (int)property.FastGetValue(t));

            property.FastSetValue(t, 20);
            //Assert.AreEqual(20, (int)property.FastGetValue(t));
        }

        [TestMethod]
        public void TestPropertyObj()
        {
            PropertyInfo property = type.GetProperty("PropertyObj");
            Assert.IsNull(property.FastGetValue(t));

            object o = new object();
            t.PropertyObj = o;
            Assert.AreEqual(o, property.FastGetValue(t));

            t.PropertyObj = null;
            property.FastSetValue(t, o);
            Assert.AreEqual(o, property.FastGetValue(t));
        }

        [TestMethod]
        [ExpectedException(typeof(MethodAccessException))]
        public void TestPropertyObjGet()
        {
            PropertyInfo property = type.GetProperty("PropertyObjGet");
            Assert.IsNull(property.FastGetValue(t));

            property.FastSetValue(t, new object());
        }

        //PropertyIntGet
        [TestMethod]
        [ExpectedException(typeof(MethodAccessException))]
        public void TestPropertyIntGet()
        {
            PropertyInfo property = type.GetProperty("PropertyIntGet");
            Assert.AreEqual(0, property.FastGetValue(t));


            property.FastSetValue(t, 12);
        }

        [TestMethod]
        public void TestMethod()
        {
            MethodInfo method = type.GetMethod("Add");

            Assert.AreEqual(3, (int)method.FastInvoke(t, 1, 2));
        }

        [TestMethod]
        //[ExpectedException(typeof(MethodAccessException))]
        public void TestMethodObj()
        {
            MethodInfo method = type.GetMethod("TestMethod");

            Assert.AreEqual("2323", method.FastInvoke(t, ""));

            method = type.GetMethod("TestMethodPrivate", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod);
            method.FastInvoke(t, "", "");
        }

        [TestMethod]
        public void test_GetArgumentByType()
        {
            object obj = new object();
            var objs = new object[]{1,2,3L,4,"as","sd",null,obj};
            var types = new Type[]{typeof(Int32),typeof(Int64),typeof(string),typeof(string), typeof(object), typeof(object)};
            var resulr = new object[] { 1, 3L, "as", "sd", null, obj };

            int i = 0;
            foreach (var item in ReflectionHelp.GetArgumentByType(objs, types))
            {
                Assert.AreEqual(item, resulr[i]);                
                i++;
            }
        }

        [TestMethod]
        public void TestConstructor()
        {
            ConstructorInfo method = type.GetConstructor(Type.EmptyTypes);
            Assert.IsNotNull(method.FastCreate());
        }

        [TestMethod]
        //[ExpectedException(typeof(MemberAccessException))]
        public void TestConstructorArg()
        {
            ConstructorInfo method = type.GetConstructor(new Type[] { typeof(int) });
            Assert.IsNotNull(method.FastCreate(1));
            Assert.AreEqual(1, ((Test)method.FastCreate(1)).PropertyInt);

            method = type.GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(object) }, null);
            var o = new object();
            Assert.IsNotNull(method.FastCreate(o));
            Assert.ReferenceEquals(o, ((Test)method.FastCreate(o)).PropertyObj);
        }

        [TestMethod]
        public void TestConstructorArgs()
        {
            ConstructorInfo method = type.GetConstructor(new Type[] { typeof(int), typeof(object) });
            var o = new object();
            Assert.IsNotNull(method.FastCreate(1, o));
            Assert.AreEqual(1, ((Test)method.FastCreate(1, 0)).PropertyInt);
            Assert.ReferenceEquals(o, ((Test)method.FastCreate(1, o)).PropertyObj);
        }

        [ClassCleanup]
        public static void End()
        {
            FastReflectionFactory.SaveCache();
        }
    }

    public class Test
    {
        public Test() { }
        public Test(int i) { PropertyInt = i; }
        protected Test(object obj) { PropertyObj = obj; }
        public Test(int i, object obj)
        {
            PropertyInt = i;
            PropertyObj = obj;
        }

        public int PropertyInt { get; set; }
        public object PropertyObj { get; set; }
        public object PropertyObjGet { get; private set; }
        public int PropertyIntGet { get { return fieldInt; } }
        public int fieldInt;
        public object fieldObj;

        public int GetInt()
        {
            return 1;
        }

        public int Add(int a, int b)
        {
            return a + b;
        }

        public object TestMethod()
        {
            return "2323";
        }

        private object TestMethodPrivate()
        {
            return "private";
        }
    }
}
