﻿<%@ Control Language="C#" ClassName="BlogBody" Inherits="ViewUserControlBase<BlogEntity>" %>
<%--<%@ OutputCache Duration="6000" VaryByParam="*" %>--%>

<%= Model.Text %>
<%= DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") %>