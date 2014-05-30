<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="RedGlovePermission.Web.Admin.Login" %>

<%@ Import Namespace="RedGlove.Core.Languages" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>
        <%=ResourceManager.GetString("sitename") %><%=ResourceManager.GetString("Login_Title_name")%>
    </title>
    <link href="../Inc/Style/default/css/indexControl.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">

        if (window.top.length > 0) {
            window.top.location = "login.aspx";
        }
       
        if (window.external.dialogTop != undefined) {
            alert("<%=ResourceManager.GetString("Login_Msg_4") %>");
            window.close();
        }

        function $id(s) {
            return document.getElementById(s);
        }

        function checklogin() {
            var ret = true;
            if ($id("UserName").value == "") {
                alert('<%=ResourceManager.GetString("Login_Msg_V_1") %>');
                ret = false;
            }
            else if ($id("Password").value == "") {
            alert('<%=ResourceManager.GetString("Login_Msg_V_2") %>');
                ret = false;
            }
            return ret;
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="bodyhead">
        <table width="100%" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td height="300" valign="top">
                    <table width="377" border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="padding-left: 20px; padding-top: 20px;">
                                <span style="font-family: MS Reference Sans Serif; font-weight: bold; font-size: 32px;
                                    color: #fff;">RedGlove</span><br />
                                <span style="font-family: MS Reference Sans Serif; font-size: 24px; color: #fff;">Management
                                    system</span>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <asp:UpdatePanel ID="CustomPanel1" runat="server">
        <ContentTemplate>
            <table border="0" align="center" cellpadding="0" cellspacing="2" style="width: 260px;
                margin-top: 10px; margin-left: 120px;">
                <tr>
                    <td>
                        <div align="right">
                            <%=ResourceManager.GetString("Login_Lab_item1")%>：</div>
                    </td>
                    <td>
                        <asp:TextBox ID="UserName" runat="server" TabIndex="1" class="FormBase" onfocus="this.className='FormFocus';"
                            onblur="this.className='FormBase';" Width="150px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div align="right">
                            <%=ResourceManager.GetString("Login_Lab_item2").Replace("1", "&nbsp;")%>：</div>
                    </td>
                    <td>
                        <asp:TextBox ID="Password" runat="server" TabIndex="2" class="FormBase" onfocus="this.className='FormFocus';"
                            onblur="this.className='FormBase';" Width="150px" TextMode="Password"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div align="right">
                            <%=ResourceManager.GetString("Login_Lab_item3").Replace("1","&nbsp;")%>：</div>
                    </td>
                    <td>
                        <asp:DropDownList ID="Language" runat="server" Width="153px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td style=" padding-top:10px;">
                        <asp:Button ID="btn_login" TabIndex="3" Width="80px" runat="server" Text="登录" CssClass="planebutton"
                            OnClick="btn_login_Click" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
