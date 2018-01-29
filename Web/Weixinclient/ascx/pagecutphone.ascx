<%@ Control Language="C#" AutoEventWireup="true" CodeFile="pagecutphone.ascx.cs" Inherits="RGPWEB.ascx.pagecutphone" %>

<script src="/Weixinclient/js/WeixinSplitPage.js" type="text/javascript"></script>
        <%--共 <span id="sTotalRecords"><%=iRowAllCount%></span>条--%>
       <%--页码：<span id="sPage"><%=iPageIndex == 0 || bPageClear ? 1: iPageIndex %>/<%=iPageNum %></span>页--%>


        <%
            string firstdisabled = "";
            string lastdisabled = "";
            string godisabled = "";
            if (iPageIndex == iPageNum) 
            {
                lastdisabled = "style=\"text-decoration:none; color:#999999; cursor:default\"";
            }
            if (iPageIndex == 1)
            {
                firstdisabled = "style=\"text-decoration:none; color:#999999; cursor:default\"";
            }
         %>
        <%--<a href="#" <%=firstdisabled %> onclick="btnFirst_click(); return false;">第一页</a> --%>
<table class="tbpage">
<tr>
<td>
        <a href="#" <%=firstdisabled %> onclick="btnPre_click(); return false;">上一页</a> 
</td>
<td>
        <a href="#" <%=lastdisabled %> onclick="btnNext_click(); return false;">下一页</a> <span style=" float:right; margin-right:10px; text-align:right;"><%=iPageNum==0?0:iPageIndex==0 || bPageClear?1:iPageIndex%>/<%=iPageNum %></span> 
</td>
</tr>

</table>
        <%--<a href="#" <%=lastdisabled %> onclick="btnLast_click(); return false;">最后一页</a>--%>
      <%-- 转到：<input id="txtGo" name="txtGo" style="width: 24px; height:13px;" type="text" />--%>
        <%--<input class="button" type="button" name="Submit"<%=godisabled %> onclick="btnGo_click(); return false;" value="GO" />--%>
    
    <input id="hPageID" name="hPageID" type="hidden" value="<%=iPageIndex==0 || bPageClear?1:iPageIndex%>"  style="width: 1px" />
    <input id="hPageNum" name="hPageNum" type="hidden" value="<%=iPageNum %>" style="width: 1px" />

