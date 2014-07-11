<%@ Page Language="C#" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Fish Li 的博客</title>
    <link type="text/css" rel="Stylesheet" href="/css/StyleSheet.css" />
</head>
<body>

<% HTML.RenderPartial("~/UserControls/PageHeader.ascx", null); %>

<div id="main">
	<div id="left">
		<div id="blog-body"><b>Loading......</b></div>
		
		<p><br /></p>
		<hr />
		<p><br /></p>
		
		<div id="blog-comments-placeholder"><b>Loading......</b></div>
	</div>
	<div id="right"><b>Loading......</b></div>
</div>

    <% HTML.RenderPartial("~/UserControls/PageEnd.ascx", null); %>
<%--</body>
</html>
--%>