<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%= (bool) (ViewData.Model == null) ? "No" : ViewData.Model ? "Yes" : "No" %>