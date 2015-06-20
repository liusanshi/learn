#define sendbox
using Jayrock.Json;
using Jayrock.Json.Conversion;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Top.Api.Util;
using Top.Tmc;
using XDHelper.Common;

namespace XDHelper
{
    public partial class _Default : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            var str = nestMessage.MSG;

            Response.Write("我的code：" + this.Request["code"] + "<br/>");
            Response.Write("access_token:" + AppServer.TokenManager.GetToken(this.Request["code"]));

        }

        static private class nestMessage
        {
            static nestMessage()
            {
                AddListen();
            }

            /// <summary>
            /// 消息
            /// </summary>
            public static string MSG { get; set; }

            private static void AddListen()
            {
                TmcClient client = new TmcClient(XDHelper.Common.AppConfig.Appkey, XDHelper.Common.AppConfig.AppSecret, "default");
                //沙箱测试消息服务： TmcClient tmcClient=new TmcClient(ws://mc.api.tbsandbox.com/, 沙箱appkey, 沙箱secret, "groupName");
                client.OnMessage += (s, e) =>
                {
                    try
                    {
                        Console.WriteLine(e.Message.Content);
                        Console.WriteLine(e.Message.Topic);
                        // 默认不抛出异常则认为消息处理成功  
                    }
                    catch (Exception exp)
                    {
                        Console.WriteLine(exp.StackTrace);
                        e.Fail(); // 消息处理失败回滚，服务端需要重发  
                    }
                };
                client.Connect("ws://mc.api.taobao.com/");
            }
        }
    }
}