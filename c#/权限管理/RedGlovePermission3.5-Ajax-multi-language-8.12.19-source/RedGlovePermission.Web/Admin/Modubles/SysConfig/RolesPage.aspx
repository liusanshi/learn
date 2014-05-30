<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RolesPage.aspx.cs" Inherits="RedGlovePermission.Web.Admin.Modubles.SysConfig.RolesPage" %>
<%@ Import Namespace="RedGlove.Core.Languages" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title><%=ResourceManager.GetString("RolesPage_Title_name")%></title>
    <link href="../../../Inc/Style/<%=Request.Cookies["UIStyle"].Value%>/css/indexControl.css" rel="stylesheet" type="text/css" />
    <link href="../../../Inc/Style/<%=Request.Cookies["UIStyle"].Value%>/css/GridView.css" rel="stylesheet" type="text/css" />

    <script src="../../../Inc/Script/pub.js" type="text/javascript"></script>
    
    <script type="text/javascript">
    
        //新增数据合法性检测
        function CheckAdd() {
            var ret = true;
            
            if ($id("txt_name").value == "") {
                alert('<%=ResourceManager.GetString("Pub_Msg_V_1")%>');
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
            <%=ResourceManager.GetString("RolesPage_Title_name")%></div>
    </div>
    <asp:UpdatePanel ID="CustomPanel1" runat="server">
        <ContentTemplate>
            <div id="title" class="childtoolbar">
            <div class="rowdiv" style="margin-left: 3px;">
                    <%=ResourceManager.GetString("RolesPage_Lab_5")%>:</div>
                <div class="rowdiv">
                    <asp:DropDownList ID="GroupList" runat="server" AutoPostBack="True" 
                        onselectedindexchanged="GroupList_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>
                <div class="rowdiv" style="margin-left: 3px;">
                    <%=ResourceManager.GetString("RolesPage_Lab_1")%>:</div>
                <div class="rowdiv">
                    <asp:TextBox ID="txt_Name" Width="120" MaxLength="30" CssClass="inputbox" runat="server"></asp:TextBox></div>
                <div class="rowdiv" style="margin-left: 3px;">
                    <%=ResourceManager.GetString("RolesPage_Lab_3")%>:</div>
                <div class="rowdiv" style="margin-left: 3px;">
                    <asp:TextBox ID="txt_Description" CssClass="inputbox" runat="server"></asp:TextBox>
                </div>
                <div class="rowdiv" style="margin-left: 5px; line-height: 30px;">
                    <asp:Button ID="btn_add" runat="server" class="button" Text=" 新 增 " OnClick="btn_add_Click" /></div>
            </div>
            <div class="gv">
                <asp:GridView ID="RolesLists" runat="server" DataKeyNames="RoleID" CssClass="Grid"
                    AllowSorting="True" OnRowCommand="RolesLists_RowCommand" OnRowDataBound="RolesLists_RowDataBound"
                    AllowPaging="True" OnPageIndexChanging="RolesLists_PageIndexChanging" PageSize="15"
                    AutoGenerateColumns="False" OnRowCancelingEdit="RolesLists_RowCancelingEdit"
                    OnRowEditing="RolesLists_RowEditing" 
                    OnRowUpdating="RolesLists_RowUpdating" onrowcreated="RolesLists_RowCreated">
                    <FooterStyle CssClass="GridFooter" />
                    <RowStyle CssClass="Row" />
                    <Columns>
                        <asp:BoundField DataField="RoleID" HeaderText="编号" ReadOnly="True">
                            <ItemStyle HorizontalAlign="Center" Wrap="false" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="名称">
                            <ItemTemplate>
                                <asp:Label ID="Lab_name" runat="server" Text='<%# ResourceManager.GetString(Eval("RoleName").ToString()) %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txt_name" Width="120" MaxLength="30" CssClass="inputbox" runat="server"
                                    Text='<%# Eval("RoleName") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            </asp:TemplateField>                       
                        <asp:TemplateField HeaderText="说明">
                            <ItemTemplate>
                                <asp:Label ID="Lab_Description" runat="server" Text='<%# Eval("RoleDescription") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txt_Description" Width="120" MaxLength="30" CssClass="inputbox"
                                    runat="server" Text='<%# Eval("RoleDescription") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                        </asp:TemplateField>  
                        <asp:TemplateField HeaderText="分组">
                            <ItemTemplate>
                                <asp:Label ID="Lab_GroupID" runat="server" Text='<%# ResourceManager.GetString(Eval("RoleGroupID").ToString()) %>'></asp:Label>  
                                <asp:Label ID="hid_GroupID" runat="server" Text='<%# Eval("RoleGroupID") %>' style="display:none;"></asp:Label>                              
                            </ItemTemplate>
                            <EditItemTemplate>
                            <asp:Label ID="Lab_GroupID" runat="server" Text='<%# Eval("RoleGroupID") %>' style="display:none;"></asp:Label>
                            <asp:Label ID="hid_GroupID" runat="server" Text='<%# Eval("RoleGroupID") %>' style="display:none;"></asp:Label>
                                <asp:DropDownList ID="GroupID" runat="server">
                                </asp:DropDownList>
                            </EditItemTemplate>
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                        </asp:TemplateField>                        
                        <asp:TemplateField HeaderText="编辑" ShowHeader="False">
                            <ItemTemplate>
                                <asp:LinkButton ID="btn_Edit" runat="server" CausesValidation="False" CommandName="Edit"
                                    Text='<%#ResourceManager.GetString("Pub_Lbtn_edit")%>' CommandArgument='<%# Eval("RoleID")%>'></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton ID="btn_update" runat="server" CausesValidation="True" CommandName="Update"
                                    Text='<%#ResourceManager.GetString("Pub_Lbtn_update")%>'></asp:LinkButton>
                                &nbsp;<asp:LinkButton ID="btn_cancel" runat="server" CausesValidation="False" CommandName="Cancel"
                                    Text='<%#ResourceManager.GetString("Pub_Lbtn_cancel")%>'></asp:LinkButton>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="删除" ShowHeader="False">
                            <ItemTemplate>
                                <asp:LinkButton ID="btn_del" runat="server" CausesValidation="False" CommandName="Del"
                                    Text='<%#ResourceManager.GetString("Pub_Lbtn_delete")%>' CommandArgument='<%# Eval("RoleID")%>'></asp:LinkButton>
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
    </form>
</body>
</html>
