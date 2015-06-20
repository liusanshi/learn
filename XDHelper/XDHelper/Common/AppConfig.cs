#define sendbox

namespace XDHelper.Common
{
    public static class AppConfig
    {

#if sendbox1
        public readonly string authorize_url = "https://oauth.tbsandbox.com/authorize";
        public readonly string token_url = "https://oauth.tbsandbox.com/token";
        public readonly static string redirect_uri = "http://localhost:6065/Default.aspx";
        public readonly string Appkey = "1023169427";
        public readonly string AppSecret = "sandbox26d1698cc1e03ec4037758f68";

        public readonly static int Token_LiftTime = 3;
        public readonly static double Code_LiftTime = 48d;
#else
        public readonly static string authorize_url = "https://oauth.taobao.com/authorize";
        public readonly static string token_url = "https://oauth.taobao.com/token";
        public readonly static string redirect_uri = "http://localhost:6065/Default.aspx";
        public readonly static string Appkey = "23169427";
        public readonly static string AppSecret = "e3c443926d1698cc1e03ec4037758f68";

        public readonly static int Token_LiftTime = 3;
        public readonly static double Code_LiftTime = 48d;
#endif
    }
}