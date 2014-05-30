<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="RedGlovePermission.Web.Error" %>
<%@ Import Namespace="RedGlove.Core.Languages" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%=ResourceManager.GetString("Pub_Msg_error00")%></title>

    <script type="text/javascript"> 
        function GetError()      
        {
            var s=<%=ret%>;
            switch(s)
            {
                case 1:
                    alert('<%=ResourceManager.GetString("Pub_Msg_error02")%>');
                    history.go(-1);
                    break;
                case 2:
                    window.parent.alert('<%=ResourceManager.GetString("Pub_Msg_error02")%>');
                    window.parent.location = 'admin/login.aspx';
                    break;
                case 3:
                    alert('<%=ResourceManager.GetString("Pub_Msg_error03")%>');
                    history.go(-1);
                    break;
                case 4:
                    alert('<%=ResourceManager.GetString("Pub_Msg_error04")%>');
                    history.go(-1);
                    break;
                case 5:
                    alert('<%=ResourceManager.GetString("Pub_Msg_error05")%>');
                    history.go(-1);                    
                    break;
            }
         }
    </script>

</head>
<body onload="GetError();">
    <form id="form1" runat="server">
    <div>
        <asp:Literal ID="Literal1" runat="server"></asp:Literal>
    </div>
    </form>
</body>
</html>
