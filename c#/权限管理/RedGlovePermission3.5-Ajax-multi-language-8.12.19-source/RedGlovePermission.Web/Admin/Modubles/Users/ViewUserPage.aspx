<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewUserPage.aspx.cs" Inherits="RedGlovePermission.Web.Admin.Modubles.Users.ViewUserPage" %>

<%@ Import Namespace="RedGlove.Core.Languages" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%=ResourceManager.GetString("ViewUser_Title_Name")%></title>
    <link href="../../../Inc/Style/<%=Request.Cookies["UIStyle"].Value%>/css/indexControl.css"
        rel="stylesheet" type="text/css" />
    <link href="../../../Inc/Style/<%=Request.Cookies["UIStyle"].Value%>/css/GridView.css"
        rel="stylesheet" type="text/css" />

    <script src="../../../Inc/Script/pub.js" type="text/javascript"></script>

</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="divToolBar" class="topBar">
        <div class="toolBar">
        </div>
        <div class="moduleName">
            <%=ResourceManager.GetString("ViewUser_Title_Name")%></div>
    </div>
    <asp:UpdatePanel ID="CustomPanel1" runat="server">
        <ContentTemplate>
            <div class="gv">
                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <table width="300" border="0" cellpadding="0" cellspacing="3">
                                <tr>
                                    <td>
                                        <%=ResourceManager.GetString("ViewUser_Lab_1")%>
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table border="0" cellpadding="0" cellspacing="5">
                                <tr>
                                    <td style="text-align:right;">
                                        <%=ResourceManager.GetString("ListUsers_Lab_1")%>：
                                    </td>
                                    <td style="white-space:nowrap;">
                                        <asp:Label ID="lab_name" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align:right;">
                                        <%=ResourceManager.GetString("ListUsers_Lab_3")%>：
                                    </td>
                                    <td>
                                        <asp:Label ID="Lab_group" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align:right;">
                                        <%=ResourceManager.GetString("ListUsers_Lab_2")%>：
                                    </td>
                                    <td>
                                        <asp:Label ID="Lab_role" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align:right;">
                                        <%=ResourceManager.GetString("ListUsers_Lab_4")%>：
                                    </td>
                                    <td>
                                        <asp:Label ID="Lab_state" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align:right;">
                                        <%=ResourceManager.GetString("ListUsers_Lab_5")%>：
                                    </td>
                                    <td>
                                        <asp:Label ID="Lab_time1" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align:right;white-space:nowrap;">
                                        <%=ResourceManager.GetString("ListUsers_Lab_6")%>：
                                    </td>
                                    <td style="white-space:nowrap;">
                                        <asp:Label ID="Lab_time2" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <input id="Button1" type="button" value='<%=ResourceManager.GetString("Pub_Btn_return")%>' class="button" onclick=" history.go(-1);" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
