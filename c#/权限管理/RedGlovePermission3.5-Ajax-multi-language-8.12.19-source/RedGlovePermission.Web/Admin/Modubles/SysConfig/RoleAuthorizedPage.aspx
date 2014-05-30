<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RoleAuthorizedPage.aspx.cs"
    Inherits="RedGlovePermission.Web.Admin.Modubles.SysConfig.RoleAuthorizedPage" %>

<%@ Import Namespace="RedGlove.Core.Languages" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>
        <%=ResourceManager.GetString("RoleAuthorizedPage_Title_name")%></title>
    <link href="../../../Inc/Style/<%=Request.Cookies["UIStyle"].Value%>/css/indexControl.css"
        rel="stylesheet" type="text/css" />
    <link href="../../../Inc/Style/<%=Request.Cookies["UIStyle"].Value%>/css/GridView.css"
        rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="divToolBar" class="topBar">
        <div class="toolBar">
        </div>
        <div class="moduleName">
            <%=ResourceManager.GetString("RoleAuthorizedPage_Title_name")%></div>
    </div>
    <asp:UpdatePanel ID="CustomPanel1" runat="server">
        <ContentTemplate>
            <div class="gv">
                <div style="float: left;">
                    <div style="height:30px; line-height:30px;">
                        <asp:DropDownList ID="GroupList" runat="server" AutoPostBack="True" OnSelectedIndexChanged="GroupList_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:Label ID="Rid" runat="server" Text="" Style="display: none;"></asp:Label>
                    </div>
                    <div style="margin-top: 5px;">
                        <asp:GridView ID="RoleView" Width="100px" runat="server" DataKeyNames="RoleID" AllowSorting="True"
                            OnRowCommand="RoleView_RowCommand" AutoGenerateColumns="False" ShowHeader="False"
                            GridLines="None">
                            <Columns>
                                <asp:BoundField DataField="RoleID" HeaderText="编号" Visible="false">
                                    <ItemStyle HorizontalAlign="Center" Wrap="false" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="名称">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbtn_name" CommandArgument='<%# Eval("RoleID")%>' CommandName="EditView"
                                            runat="server" Text='<%# ResourceManager.GetString(Eval("RoleName").ToString()) %>'
                                            ToolTip='<%# Eval("RoleDescription") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" Width="100px" Height="24px" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
                <div style="float: left; margin-left: 10px;">
                    <div style="height:30px; line-height:30px;">
                        <div class="rowdiv">
                            <asp:DropDownList ID="ModuleTypeList" runat="server" Enabled="false" AutoPostBack="True"
                                OnSelectedIndexChanged="ModuleTypeList_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                        <div class="rowdiv" style="line-height:26px; margin-left:5px;">
                            <asp:Button ID="btn_AllSave" runat="server" class="button" Text=" 保存全部 " OnClick="btn_AllSave_Click" />
                        </div>
                    </div>
                    <div style="margin-top: 5px;">
                        <asp:GridView ID="ModuleView" Width="100px" runat="server" DataKeyNames="ModuleID"
                            CssClass="Grid" AllowSorting="True" AutoGenerateColumns="False" OnRowDataBound="ModuleView_RowDataBound"
                            OnSelectedIndexChanging="ModuleView_SelectedIndexChanging" OnRowCreated="ModuleView_RowCreated">
                            <FooterStyle CssClass="GridFooter" />
                            <RowStyle CssClass="Row" />
                            <Columns>
                                <asp:TemplateField HeaderText="模块名称">
                                    <ItemTemplate>
                                        <asp:Label ID="lab_ID" runat="server" Text='<%# Eval("ModuleID")%>' Style="display: none"></asp:Label>
                                        <asp:LinkButton ID="lbtn_name" CommandArgument='<%# Eval("ModuleID")%>' CommandName="EditView"
                                            runat="server" Text='<%# ResourceManager.GetString(Eval("ModuleName").ToString()) %>'
                                            ToolTip='<%# Eval("ModuleDescription") %>'></asp:LinkButton>
                                        <asp:Label ID="lab_Verify" runat="server" Text="" Style="display: none"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Wrap="False" />
                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="权限列表">
                                    <ItemTemplate>
                                        <asp:CheckBoxList ID="AuthorityList" runat="server" RepeatDirection="Horizontal"
                                            RepeatLayout="Flow">
                                        </asp:CheckBoxList>
                                    </ItemTemplate>
                                    <HeaderStyle Wrap="False" />
                                    <ItemStyle Wrap="False" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btn_update" runat="server" CommandArgument='<%# Eval("ModuleID")%>'
                                            CausesValidation="False" CommandName="Select" Text='<%#ResourceManager.GetString("Pub_Lbtn_update")%>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <HeaderStyle Wrap="False" />
                                    <ItemStyle Wrap="False" />
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle CssClass="HeadingCell" />
                            <AlternatingRowStyle BorderStyle="None" CssClass="AlternatingRow" />
                        </asp:GridView>
                    </div>
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
