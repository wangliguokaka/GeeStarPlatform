<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="mobile.aspx.cs" Inherits="Web.mobile" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>义齿订单查询</title>
    <link href="style.css" rel="stylesheet" type="text/css" />
    <meta name="viewport" content="width=device-width,target-densitydpi=high-dpi,initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0, user-scalable=no" />
</head>
<body style="margin: 0px; font-size: 14px; font-family: 微软雅黑">
    <form id="form1" runat="server">
    <div id="top">
        <asp:Label ID="Label5" runat="server" Text="Label"></asp:Label></div>
    <ul id="menu">
        <li><a href="#">真伪</a></li>
        <li><a href="#">详细</a></li>
        <li><a href="#">合格证</a></li>
        <li><a href="#">说明书</a></li>
        <li><a href="#">我们</a></li>
    </ul>
    <div id="neirong">
        <ol style="background: none">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" style="margin: 2em auto">
                <tr>
                    <td width="30%">
                        <img src="images/doc.jpg" id="doc">
                    </td>
                    <td width="20em">
                        &nbsp;
                    </td>
                    <td style="line-height: 1.6em" width="58%">
                        <span class="right">姓名：<asp:Label ID="Label3" runat="server" Width="83px"></asp:Label>
                            <br />
                            <asp:Label ID="Label6" runat="server" Text="信息真实有效"></asp:Label></span> 订单号：
                        <span class="names">
                            <asp:Label ID="Label1" runat="server" Width="135px" ForeColor="#00CCFF"></asp:Label></span>
                        <br />
                        医院名称：<asp:Label ID="Label2" runat="server" Width="134px" ForeColor="#00CCFF"></asp:Label>
                        <br />
                        医师姓名：<asp:Label ID="Label4" runat="server" Width="125px" ForeColor="#00CCFF"></asp:Label>
                        <br />
                        生成日期：<asp:Label ID="Label7" runat="server" Width="145px" ForeColor="#00CCFF"></asp:Label>
                        <br />
                        保修期：<asp:Label ID="Label13" runat="server" Width="151px" ForeColor="#00CCFF"></asp:Label>
                    </td>
                </tr>
            </table>
        </ol>
        <ol>
            <asp:Repeater ID="rptRepeater1" runat="server">
                <HeaderTemplate>
                    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="tab">
                        <tr>
                            <th>
                                种类名称
                            </th>
                            <th>
                                牙位
                            </th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td style="width: 70%">
                            <%# Eval("itemname").ToString()%>
                        </td>
                        <td>
                            <table border="0" cellspacing="0" cellpadding="0" width="100%">
                                <tr>
                                    <td style="border-right: 1px solid #b7b5b5; text-align: right;">
                                        <%# Eval("a_teeth").ToString()%>
                                        &nbsp;
                                    </td>
                                    <td style="text-align: left;">
                                        <%# Eval("b_teeth").ToString()%>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="border: none; border-right: 1px solid #b7b5b5; text-align: right;">
                                        <%# Eval("c_teeth").ToString()%>
                                        &nbsp;
                                    </td>
                                    <td style="border: none; text-align: left;">
                                        <%# Eval("d_teeth").ToString()%>
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
            <asp:Repeater ID="Repeater1" runat="server">
                <HeaderTemplate>
                    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="tab" style="border-top: 0.5em solid #efedee">
                        <tr>
                            <th>
                                主体材料
                            </th>
                            <th>
                                供应商
                            </th>
                            <th>
                                批号
                            </th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <%# Eval("Name").ToString()%>
                            &nbsp;
                        </td>
                        <td>
                            <%# Eval("provider").ToString()%>
                            &nbsp;
                        </td>
                        <td>
                            <%# Eval("batchNo").ToString()%>
                            &nbsp;
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </ol>
        <ol style="padding: 0.9em 0.8em">
            <asp:Label ID="lblgdgood" runat="server" Text=""></asp:Label>
            <asp:Label ID="lblhdgood" runat="server" Text=""></asp:Label>
        </ol>
        <ol style="padding: 0.9em 0.8em">
            <asp:Label ID="lblgdshuoming" runat="server" Text=""></asp:Label>
            <asp:Label ID="lblhgshuoming" runat="server" Text=""></asp:Label>
        </ol>
        <ol style="padding: 0.9em 0.8em">
            <asp:Label ID="Label8" runat="server" Text=""></asp:Label>
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <div id="bottom">
                相关服务由 大连吉星远大科技有限公司 提供技术支持</div>
        </ol>
    </div>
    <script>
        var Tags = document.getElementById('menu').getElementsByTagName('a');
        var TagsCnt = document.getElementById('neirong').getElementsByTagName('ol');
        var len = Tags.length;
        var flag = 0; //修改默认值
        for (i = 0; i < len; i++) {
            Tags[i].value = i;
            Tags[i].onmouseover = function () { changeNav(this.value) };
            TagsCnt[i].className = 'undis';
        }
        Tags[flag].className = 'topC1';
        TagsCnt[flag].className = 'dis';
        function changeNav(v) {
            Tags[flag].className = 'topC0';
            TagsCnt[flag].className = 'undis';
            flag = v;
            Tags[v].className = 'topC1';
            TagsCnt[v].className = 'dis';
        }
    </script>
    </form>
</body>
</html>
