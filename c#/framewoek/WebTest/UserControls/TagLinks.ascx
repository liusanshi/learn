<%@ Control Language="C#" ClassName="TagLinks" Inherits="ViewUserControlBase<List<BlogLink>>" %>
<%--<%@ OutputCache Duration="6000" VaryByParam="*" %>--%>

<p><b>推荐排行榜</b></p>
<ul>
<%  foreach( var link in Model ) {%>
	<li><a href="<%= link.Href %>" target="_blank"><%= link.Text %></a></li>
<% } %>
</ul>
