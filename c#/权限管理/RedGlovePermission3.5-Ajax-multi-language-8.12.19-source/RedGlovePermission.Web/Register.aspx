<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="RedGlovePermission.Web.Register" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>用户注册</title>
    <link href="Inc/Style/Stylesheet1.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="CustomPanel1" runat="server">
        <ContentTemplate>
            <table width="500" border="0" align="center" cellpadding="0" cellspacing="2" style="margin-top: 50px;">
                <tr>
                    <td colspan="3" class="regTitle">
                        用户注册
                    </td>
                </tr>
                <tr>
                    <td width="82" style="height: 30px; line-height: 30px;">
                        <div align="right">
                            用&nbsp;&nbsp;户&nbsp;&nbsp;名&nbsp;&nbsp;</div>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_username" CssClass="inpubbox" runat="server"></asp:TextBox>
                    </td>
                    <td width="222">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <div align="right" style="height: 30px; line-height: 30px;">
                            密&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;码&nbsp;&nbsp;</div>
                    </td>
                    <td class="style1">
                        <asp:TextBox ID="txt_password" CssClass="inpubbox" runat="server" 
                            TextMode="Password"></asp:TextBox>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <div align="right" style="height: 30px; line-height: 30px;">
                            确认密码&nbsp;&nbsp;</div>
                    </td>
                    <td class="style1">
                        <asp:TextBox ID="txt_password2" CssClass="inpubbox" runat="server" 
                            TextMode="Password"></asp:TextBox>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <div align="right" style="height: 30px; line-height: 30px;">
                            安全问题&nbsp;&nbsp;</div>
                    </td>
                    <td class="style1">
                        <asp:TextBox ID="txt_question" CssClass="inpubbox" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <div align="right" style="height: 30px; line-height: 30px;">
                            答&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;案&nbsp;&nbsp;</div>
                    </td>
                    <td class="style1">
                        <asp:TextBox ID="txt_answer" CssClass="inpubbox" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <div align="right" style="height: 30px; line-height: 30px;">
                            验&nbsp;&nbsp;证&nbsp&nbsp;码&nbsp;&nbsp;</div>
                    </td>
                    <td class="style1">
                        <asp:TextBox ID="txt_verifycode" CssClass="inpubbox" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <div align="right">
                        </div>
                    </td>
                    <td colspan="2">
                        <asp:Button ID="btn_reg" runat="server" Text="现在注册" Height="37px" Width="94px" OnClick="btn_reg_Click" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
