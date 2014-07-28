<%@ Page Language="C#" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>获取客户端IP</title>

    <script runat="server">
        private static string getIp()
        {
            if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
                return System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].Split(new char[] { ',' })[0];
            else
                return System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        }
    
    </script>
</head>
<body>



    HTTP_X_FORWARDED_FOR|REMOTE_ADDR :  <%=getIp() %>
    <br/>
    Request.UserHostAddress :  <% =Request.UserHostAddress %>

    <form id="form1" runat="server">
        <div>
        </div>
    </form>
</body>
</html>
