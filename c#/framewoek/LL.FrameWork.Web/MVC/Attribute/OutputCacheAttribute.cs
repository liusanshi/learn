using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace LL.FrameWork.Web.MVC
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class OutputCacheAttribute : ActionFilterAttribute
    {
        private OutputCacheParameters _cacheSettings
                = new OutputCacheParameters { Duration = 600, VaryByParam = "none" };

        [XmlAttribute]
        public string CacheProfile
        {
            get { return _cacheSettings.CacheProfile ?? String.Empty; }
            set { _cacheSettings.CacheProfile = value; }
        }

        internal OutputCacheParameters CacheSettings
        {
            get { return _cacheSettings; }
        }

        [XmlAttribute]
        public int Duration
        {
            get { return _cacheSettings.Duration; }
            set { _cacheSettings.Duration = value; }
        }

        [XmlAttribute]
        public OutputCacheLocation Location
        {
            get { return _cacheSettings.Location; }
            set { _cacheSettings.Location = value; }
        }

        [XmlAttribute]
        public bool NoStore
        {
            get { return _cacheSettings.NoStore; }
            set { _cacheSettings.NoStore = value; }
        }

        [XmlAttribute]
        public string SqlDependency
        {
            get { return _cacheSettings.SqlDependency ?? String.Empty; }
            set { _cacheSettings.SqlDependency = value; }
        }

        [XmlAttribute]
        public string VaryByContentEncoding
        {
            get { return _cacheSettings.VaryByContentEncoding ?? String.Empty; }
            set { _cacheSettings.VaryByContentEncoding = value; }
        }

        [XmlAttribute]
        public string VaryByCustom
        {
            get { return _cacheSettings.VaryByCustom ?? String.Empty; }
            set { _cacheSettings.VaryByCustom = value; }
        }

        [XmlAttribute]
        public string VaryByHeader
        {
            get { return _cacheSettings.VaryByHeader ?? String.Empty; }
            set { _cacheSettings.VaryByHeader = value; }
        }

        [XmlAttribute]
        public string VaryByParam
        {
            get { return _cacheSettings.VaryByParam ?? String.Empty; }
            set { _cacheSettings.VaryByParam = value; }
        }

        internal void SetResponseCache(HttpContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            using (OutputCachedPage page = new OutputCachedPage(_cacheSettings))
            {
                page.ProcessRequest(context);
            }
        }

        private sealed class OutputCachedPage : Page
        {
            private OutputCacheParameters _cacheSettings;

            public OutputCachedPage(OutputCacheParameters cacheSettings)
            {
                this.ID = Guid.NewGuid().ToString();
                _cacheSettings = cacheSettings;
            }

            protected override void FrameworkInitialize()
            {
                base.FrameworkInitialize();
                InitOutputCache(_cacheSettings);
            }
        }

        #region ActionFilterAttribute Member

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }
            using (OutputCacheAttribute.OutputCachedPage outputCachedPage = new OutputCacheAttribute.OutputCachedPage(this._cacheSettings))
            {
                outputCachedPage.ProcessRequest(HttpContext.Current);
            }
        }

        #endregion
    }
}
