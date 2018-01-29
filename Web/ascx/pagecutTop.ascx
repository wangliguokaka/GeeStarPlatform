<%@ Control Language="C#" AutoEventWireup="true" CodeFile="pagecutTop.ascx.cs" Inherits="RGPWEB.ascx.pagecutTop" %>

<script src="/js/SplitPage.js" type="text/javascript"></script>

<div class="dPage" style="">
    <div style="height: 15px; line-height: 15px;">
        <div style="float: left;">
            <%--共 <span id="sTotalRecords"><%=iRowAllCount%></span>条--%>
            页码：<span id="sPage"><%=iPageIndex == 0 || bPageClear ? 1: iPageIndex %>/<%=iPageNum %></span>页
            <%
                string firstdisabled = "";
                string lastdisabled = "";
                string godisabled = "";
                if (iPageIndex == iPageNum || iPageNum == 0)
                {
                    lastdisabled = "style=\"text-decoration:none; color:#999999; cursor:default\"";
                }
                if (iPageIndex == 1)
                {
                    firstdisabled = "style=\"text-decoration:none; color:#999999; cursor:default\"";
                }
            %>
            <a href="javascript:void(0)" <%=firstdisabled %> onclick="btnFirst_click(); return false;">
                第一页</a> <a href="javascript:void(0)" <%=firstdisabled %> onclick="btnPre_click(); return false;">
                    上一页</a> <a href="javascript:void(0)" <%=lastdisabled %> onclick="btnNext_click(); return false;">
                        下一页</a> <a href="javascript:void(0)" <%=lastdisabled %> onclick="btnLast_click(); return false;">
                            最后一页</a> 转到： </div>
        <div style="float: left;">
           <input id="txtGo" name="txtGo" style="width: 24px; height: 13px; border: 1px solid #444444;"
                type="text" /></div><div style="float: left;padding-left:10px;">
            <input class="button" type="button" style="width: 30px; height: 18px; border: 1px solid #444444;
                font-size: 10px" name="Submit" <%=godisabled %> onclick="btnGo_click(); return false;"
                value="GO" /></div>
        
    </div>
    <input id="hPageID" name="hPageID" type="hidden" value="<%=iPageIndex==0 || bPageClear?1:iPageIndex%>"
        style="width: 1px" />
    <input id="hPageNum" name="hPageNum" type="hidden" value="<%=iPageNum %>" style="width: 1px" />
</div>
