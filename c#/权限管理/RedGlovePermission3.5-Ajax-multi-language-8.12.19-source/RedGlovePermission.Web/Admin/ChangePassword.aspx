<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs"
    Inherits="RedGlovePermission.Web.Admin.ChangePassword" %>

<%@ Import Namespace="RedGlove.Core.Languages" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>
        <%=ResourceManager.GetString("ChangePassword_Title_name") %></title>
    <link href="../Inc/Style/<%=Request.Cookies["UIStyle"].Value%>/css/indexControl.css" rel="stylesheet" type="text/css" />

    <script src="../Inc/Script/pub.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">

        function checksubmit() {
            var ret = true;

            if ($id("OldPassword").value == "") {
                alert('<%=ResourceManager.GetString("ChangePassword_Msg_V_1") %>');
                ret = false;
            }

            else if ($id("NewPassword").value == "") {
                alert('<%=ResourceManager.GetString("ChangePassword_Msg_V_2") %>');
                ret = false;
            }

            else if ($id("RePassword").value == "") {
                alert('<%=ResourceManager.GetString("ChangePassword_Msg_V_3") %>');
                ret = false;
            }

            else if ($id("NewPassword").value != $id("RePassword").value) {
                alert('<%=ResourceManager.GetString("ChangePassword_Msg_V_4").Replace("3", "\\n")%>');
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
    <div id="divToolBar" class="topBar">
        <div class="toolBar">
        </div>
        <div class="moduleName">
            <%=ResourceManager.GetString("ChangePassword_Title_name") %></div>
    </div>
    <asp:UpdatePanel ID="CustomPanel1" runat="server">
        <ContentTemplate>
            <table border="0" cellpadding="5" cellspacing="0" style="margin-top: 10px; margin-left: 10px;">
                <tr>
                    <td style="text-align:right;">
                        <%=ResourceManager.GetString("ChangePassword_Lab_1").Replace("1","&nbsp;")%>：
                    </td>
                    <td>
                        <asp:TextBox ID="OldPassword" runat="server" Style="width: 120px" CssClass="inputbox"
                            TextMode="Password"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align:right;">
                        <%=ResourceManager.GetString("ChangePassword_Lab_2").Replace("1", "&nbsp;")%>：
                    </td>
                    <td>
                        <asp:TextBox ID="NewPassword" runat="server" Style="width: 120px" CssClass="inputbox"
                            TextMode="Password"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align:right;">
                        <%=ResourceManager.GetString("ChangePassword_Lab_3")%>：
                    </td>
                    <td>
                        <asp:TextBox ID="RePassword" runat="server" Style="width: 120px" CssClass="inputbox"
                            TextMode="Password"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2" style="padding-top: 10px;">
                        <asp:Button ID="btn_save" class="button" runat="server" Text=" 确定修改 " OnClick="btn_save_Click" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
