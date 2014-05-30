<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConfigPage.aspx.cs" Inherits="RedGlovePermission.Web.Admin.Modubles.SysConfig.ConfigPage" %>

<%@ Import Namespace="RedGlove.Core.Languages" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title><%=ResourceManager.GetString("ConfigPage_Title_Name")%></title>
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
            <%=ResourceManager.GetString("ConfigPage_Title_Name")%></div>
    </div>
    <asp:UpdatePanel ID="CustomPanel1" runat="server">
        <ContentTemplate>
            <div class="gv">
                <asp:GridView ID="ConfigList" runat="server" DataKeyNames="ItemID" CssClass="Grid"
                    AllowSorting="True" OnRowDataBound="ConfigList_RowDataBound"
                    AllowPaging="True" OnPageIndexChanging="ConfigList_PageIndexChanging" PageSize="15"
                    AutoGenerateColumns="False" OnRowCancelingEdit="ConfigList_RowCancelingEdit"
                    OnRowEditing="ConfigList_RowEditing" 
                    OnRowUpdating="ConfigList_RowUpdating" ShowHeader="False" 
                    onrowcreated="ConfigList_RowCreated">
                    <FooterStyle CssClass="GridFooter" />
                    <RowStyle CssClass="Row" />
                    <Columns>
                     
                        <asp:TemplateField HeaderText="编辑" ShowHeader="False">
                            <ItemTemplate>
                                <asp:LinkButton ID="btn_Edit" runat="server" CausesValidation="False" CommandName="Edit"
                                    Text='<%#ResourceManager.GetString("Pub_Lbtn_edit")%>' CommandArgument='<%# Eval("ItemID")%>'></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton ID="btn_update" runat="server" CausesValidation="True" CommandName="Update"
                                    Text='<%#ResourceManager.GetString("Pub_Lbtn_update")%>'></asp:LinkButton>
                                &nbsp;<asp:LinkButton ID="btn_cancel" runat="server" CausesValidation="False" CommandName="Cancel"
                                    Text='<%#ResourceManager.GetString("Pub_Lbtn_cancel")%>'></asp:LinkButton>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ItemID" HeaderText="编号" ReadOnly="True" 
                            Visible="False">
                            <ItemStyle HorizontalAlign="Center" Wrap="false" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ItemName" HeaderText="配置项" ReadOnly="True">
                            <ItemStyle HorizontalAlign="Center" Wrap="false" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="配置值">
                            <ItemTemplate>
                                <asp:Label ID="Lab_value" runat="server" Text='<%# Eval("ItemValue") %>'></asp:Label>
                            </ItemTemplate>                            
                            <EditItemTemplate>
                                <asp:TextBox ID="txt_value" Width="120" MaxLength="30" CssClass="inputbox" runat="server"
                                    Text='<%# Eval("ItemValue") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="ItemDescription" HeaderText="说明" ReadOnly="True">
                            <ItemStyle Wrap="false" />
                        </asp:BoundField>                        
                     
                    </Columns>
                    <HeaderStyle CssClass="HeadingCell" />
                    <AlternatingRowStyle BorderStyle="None" CssClass="AlternatingRow" />
                </asp:GridView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
