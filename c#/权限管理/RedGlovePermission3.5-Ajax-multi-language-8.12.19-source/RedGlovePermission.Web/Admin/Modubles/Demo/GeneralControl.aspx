<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GeneralControl.aspx.cs"
    Inherits="RedGlovePermission.Web.Admin.Modubles.Demo.GeneralControl" %>
<%@ Import Namespace="RedGlove.Core.Languages" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%=ResourceManager.GetString("GeneralControl_Title_Name")%></title>
    <link href="../../../Inc/Style/Stylesheet1.css" rel="stylesheet" type="text/css" />
    <style>
        tr
        {
            height: 24px;
            line-height: 24px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div style="margin:10px;">
        <table width="200" border="0" cellpadding="0" cellspacing="1">
            <tr>
                <td bgcolor="#669900">
                    <div align="center">
                        <span style="color: #ffffff; height: 24px; line-height: 24px;"><%=ResourceManager.GetString("GeneralControl_Lab_1")%></span></div>
                </td>
                <td bgcolor="#669900">
                    <div align="center">
                        <span style="color: #ffffff; height: 24px; line-height: 24px;"><%=ResourceManager.GetString("GeneralControl_Lab_2")%></span></div>
                </td>
            </tr>
            <tr>
                <td bgcolor="#f1f1f1">
                    <div align="center">
                        <%=ResourceManager.GetString("AItem1")%>
                    </div>
                </td>
                <td bgcolor="#f1f1f1">
                    <div align="center">
                        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
                    </div>
                </td>
            </tr>
            <tr>
                <td bgcolor="#FFFFFF">
                    <div align="center">
                        <%=ResourceManager.GetString("AItem2")%></div>
                </td>
                <td bgcolor="#FFFFFF">
                    <div align="center">
                        <asp:Label ID="Label2" runat="server" Text="Label"></asp:Label></div>
                </td>
            </tr>
            <tr>
                <td bgcolor="#f1f1f1">
                    <div align="center">
                        <%=ResourceManager.GetString("AItem3")%>
                    </div>
                </td>
                <td bgcolor="#f1f1f1">
                    <div align="center">
                        <asp:Label ID="Label3" runat="server" Text="Label"></asp:Label>
                    </div>
                </td>
            </tr>
            <tr>
                <td bgcolor="#FFFFFF">
                    <div align="center">
                        <%=ResourceManager.GetString("AItem4")%></div>
                </td>
                <td bgcolor="#FFFFFF">
                    <div align="center">
                        <asp:Label ID="Label4" runat="server" Text="Label"></asp:Label></div>
                </td>
            </tr>
            <tr>
                <td bgcolor="#f1f1f1">
                    <div align="center">
                        <%=ResourceManager.GetString("AItem5")%>
                    </div>
                </td>
                <td bgcolor="#f1f1f1">
                    <div align="center">
                        <asp:Label ID="Label5" runat="server" Text="Label"></asp:Label>
                    </div>
                </td>
            </tr>
            <tr>
                <td bgcolor="#FFFFFF">
                    <div align="center">
                        <%=ResourceManager.GetString("AItem6")%></div>
                </td>
                <td bgcolor="#FFFFFF">
                    <div align="center">
                        <asp:Label ID="Label6" runat="server" Text="Label"></asp:Label>
                    </div>
                </td>
            </tr>
            <tr>
                <td bgcolor="#f1f1f1">
                    <div align="center">
                        <%=ResourceManager.GetString("AItem7")%>
                    </div>
                </td>
                <td bgcolor="#f1f1f1">
                    <div align="center">
                        <asp:Label ID="Label7" runat="server" Text="Label"></asp:Label>
                    </div>
                </td>
            </tr>
            <tr>
                <td bgcolor="#ffffff">
                    <div align="center">
                        <%=ResourceManager.GetString("AItem8")%>
                    </div>
                </td>
                <td bgcolor="#ffffff">
                    <div align="center">
                        <asp:Label ID="Label8" runat="server" Text="Label"></asp:Label>
                    </div>
                </td>
            </tr>
            <tr>
                <td bgcolor="#f1f1f1">
                    <div align="center">
                        <%=ResourceManager.GetString("AItem9")%>
                    </div>
                </td>
                <td bgcolor="#f1f1f1">
                    <div align="center">
                        <asp:Label ID="Label9" runat="server" Text="Label"></asp:Label>
                    </div>
                </td>
            </tr>
            <tr>
                <td bgcolor="#ffffff">
                    <div align="center">
                        <%=ResourceManager.GetString("AItem10")%>
                    </div>
                </td>
                <td bgcolor="#ffffff">
                    <div align="center">
                        <asp:Label ID="Label10" runat="server" Text="Label"></asp:Label>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
