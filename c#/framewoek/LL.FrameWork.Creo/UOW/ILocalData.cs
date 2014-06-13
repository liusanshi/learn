using System;
using System.Collections;

using NHibernate.Context;

namespace LL.FrameWork.Core.UOW
{
    public interface ILocalData
    {
        int Count { get; }
        object this[object key] { get; set; }
    }

    public static class Local
    {
        static readonly ILocalData _data = new LocalData();
        private static readonly object LocalDataHashtableKey = new object();

        public static ILocalData Data
        {
            get { return _data; }
        }

        private class LocalData : ILocalData
        {
            [ThreadStatic]
            private static Hashtable _localData = new Hashtable();

            public object this[object key]
            {
                get { return LocalHashtable[key]; }
                set { LocalHashtable[key] = value; }
            }

            public int Count
            {
                get { return LocalHashtable.Count; }
            }

            private static Hashtable LocalHashtable
            {
                get
                {
                    if (!RunningInWeb)
                    {
                        if (_localData == null)
                            _localData = new Hashtable();
                        return _localData;
                    }
                    else
                    {
                        var web_hashtable = ReflectiveHttpContext.HttpContextCurrentItems[LocalDataHashtableKey] as Hashtable;
                        if (web_hashtable == null)
                        {
                            web_hashtable = new Hashtable();
                            ReflectiveHttpContext.HttpContextCurrentItems[LocalDataHashtableKey] = web_hashtable;
                        }
                        return web_hashtable;
                    }
                }
            }


            public static bool RunningInWeb
            {
                get { return ReflectiveHttpContext.HttpContextCurrentGetter() != null; }
            }
        }
    }
}
