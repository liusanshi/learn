using System;
using System.Collections.Generic;
using System.IO;

using Jayrock.Json;
using Top.Api.Util;
using Jayrock.Json.Conversion;

namespace XDHelper.Common
{
    /// <summary>
    /// 一个 token的管理单元
    /// </summary>
    public class Token
    {
        /// <summary>
        /// 创建一个 token管理单元
        /// </summary>
        /// <param name="code"></param>
        /// <param name="starttime"></param>
        public Token(string code, DateTime starttime)
        {
            Code = code;
            StartTime = starttime;
        }

        private string mtoken = null;

        /// <summary>
        /// code
        /// </summary>
        public string Code { get; private set; }
        /// <summary>
        /// 当前的token
        /// </summary>
        public string token
        {
            get
            {
                var now = DateTime.Now;

                if (mtoken == null || 
                    LifeStartTime.AddMinutes(LifeTime).CompareTo(now) <= 0)
                {
                    mtoken = Token.getToken(Code);
                    LifeStartTime = now;
                }

                return mtoken;
            }
        }
        /// <summary>
        /// 生命周期的时长 (分钟)
        /// </summary>
        public int LifeTime { get; set; }
        /// <summary>
        /// token 管理单元的起始时间
        /// </summary>
        public DateTime StartTime { get; private set; }
        /// <summary>
        /// token 管理单元的生命周期起始时间
        /// </summary>
        private DateTime LifeStartTime { get; set; }

        /// <summary>
        /// 获取token
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private static string getToken(string code)
        {
            Dictionary<string, string> props = new Dictionary<string, string>();
            props.Add("grant_type", "authorization_code");
            props.Add("code", code);
            props.Add("client_id", XDHelper.Common.AppConfig.Appkey);
            props.Add("client_secret", XDHelper.Common.AppConfig.AppSecret);
            props.Add("redirect_uri", XDHelper.Common.AppConfig.redirect_uri);
            props.Add("view", "web");
            string s = "";
            try
            {
                WebUtils webUtils = new WebUtils();
                s = webUtils.DoPost(XDHelper.Common.AppConfig.token_url, props);
            }
            catch (IOException ex)
            {

            }
            JsonObject o = JsonConvert.Import(s) as JsonObject;
            if (o != null)
            {
                return o["access_token"] as string;
            }
            return "";
        }
    }
}