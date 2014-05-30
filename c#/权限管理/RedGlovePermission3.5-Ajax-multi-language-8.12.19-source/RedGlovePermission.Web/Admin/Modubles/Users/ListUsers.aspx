<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListUsers.aspx.cs" Inherits="RedGlovePermission.Web.Admin.Modubles.Users.ListUsers" %>
<%@ Import Namespace="RedGlove.Core.Languages" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title><%=ResourceManager.GetString("ListUsers_Title_Name")%></title>
    <link href="../../../Inc/Style/<%=Request.Cookies["UIStyle"].Value%>/css/indexControl.css" rel="stylesheet" type="text/css" />
    <link href="../../../Inc/Style/<%=Request.Cookies["UIStyle"].Value%>/css/GridView.css" rel="stylesheet" type="text/css" />

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
            <%=ResourceManager.GetString("ListUsers_Title_Name")%></div>
    </div>
    <asp:UpdatePanel ID="CustomPanel1" runat="server">
        <ContentTemplate>
            <div id="title" class="childtoolbar">
                <div class="rowdiv" style="margin-left: 3px;">
                    <%=ResourceManager.GetString("ListUsers_Lab_7")%>:</div>
                <div class="rowdiv">
                    <asp:DropDownList ID="GroupList" runat="server" AutoPostBack="True" 
                        onselectedindexchanged="GroupList_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="gv">
                <asp:GridView ID="UserList" runat="server" DataKeyNames="UserID" CssClass="Grid"
                     OnRowCommand="UserList_RowCommand" OnRowDataBound="UserList_RowDataBound"
                    PageSize="15"　AutoGenerateColumns="False" 
                    onrowcreated="UserList_RowCreated" >
                    <FooterStyle CssClass="GridFooter" />
                    <RowStyle CssClass="Row" />
                    <Columns>
                        <asp:BoundField DataField="UserID" HeaderText="编号" ReadOnly="True">
                            <ItemStyle HorizontalAlign="Center" Wrap="false" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="用户名">
                            <ItemTemplate>
                                <asp:HyperLink ID="link_name" runat="server" NavigateUrl='<%# Eval("UserID", "ViewUserPage.aspx?uid={0}") %>'
                                    Text='<%# Eval("UserName") %>'></asp:HyperLink>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="用户角色">
                            <ItemTemplate>
                                <asp:Label ID="Lab_RoleName" runat="server" Text='<%# ResourceManager.GetString(Eval("RoleName").ToString()) %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="用户组">
                            <ItemTemplate>
                                <asp:Label ID="Lab_GroupName" runat="server" Text='<%# ResourceManager.GetString(Eval("GroupName").ToString()) %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="状态">
                            <ItemTemplate>
                                <asp:Label ID="Lab_state" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="创建时间" DataField="CreateTime" DataFormatString="{0:yyyy-MM-dd HH:mm:ss}">
                        <ItemStyle HorizontalAlign="Center" Wrap="false" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="最后一次登录时间" DataField="LastLoginTime" DataFormatString="{0:yyyy-MM-dd HH:mm:ss}">
                        <ItemStyle HorizontalAlign="Center" Wrap="false" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="编辑" ShowHeader="False">
                            <ItemTemplate>
                               <asp:HyperLink ID="link_view" runat="server" NavigateUrl='<%# Eval("UserID", "ViewUserPage.aspx?uid={0}") %>'
                                    Text='<%#ResourceManager.GetString("Pub_Lbtn_view")%>'></asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="删除" ShowHeader="False">
                            <ItemTemplate>
                                <asp:LinkButton ID="btn_del" runat="server" CausesValidation="False" CommandName="Del"
                                    Text='<%#ResourceManager.GetString("Pub_Lbtn_delete")%>' CommandArgument='<%# Eval("UserID")%>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>                        
                    </Columns>
                    <HeaderStyle CssClass="HeadingCell" />
                    <AlternatingRowStyle BorderStyle="None" CssClass="AlternatingRow" />
                </asp:GridView>
                <div id="GridViewMsg" style="padding: 5px;" runat="server">
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
