<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WeixinOrderInput.aspx.cs" Inherits="WeixinOrderInput" %>
<%@ Register Src="~/Weixinclient/ascx/weixintop.ascx" TagName="weixintop" TagPrefix="uccontrol" %>
<%@ Register Src="~/Weixinclient/ascx/weixinbottom.ascx" TagName="weixinbottom" TagPrefix="uccontrol" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=no" />
    <script type="text/javascript" src="/jQuery/jquery-1.10.2.js"></script>
    <script src="/artDialog/jquery.artDialog.source.js?skin=green" type="text/javascript"></script>
    <script type="text/javascript" src="/OrderManagement/js/all.js?rnd=1"></script>
     <script type="text/javascript" src="js/weixin.js"></script>
    <script type="text/javascript" src="js/WeixinSplitPage.js"></script>
    <link rel="stylesheet" type="text/css" href="css/all.css"/> 

    <script type="text/javascript" language="javascript">
        var userDoctorID = '<%=userDoctorID%>'
        var userHospitalID = '<%=userHospitalID%>'
         $(document).ready(function () {

             
             selectAll = '<%= GetGlobalResourceObject("Resource","PleaseSelect").ToString() %>';
           
            BindProcess();

            $("#<%=ddlRequireTemplate.ClientID%>").change(function ()
            {
                
                $("#Require").html(this.value);
                //$.ajax(
                //    {
                //        url: "OrderInput.aspx?action=GetRequireTemplate&temlplate=" + this.value,                        
                //        success: function (data)
                //        {
                //            $("#Require").html(data);
                //        }
                //    }
                //    )
            }
            );
            $("[id*=ddlSeller]").change(function () { BindProcess(); });
            $("[id*=ddlHosipital]").change(function () { BindVersion(); });

            $("[id*=ddlSmallClass]").change(function () { BindItemName(); });

            if ($("#keyID").val() != "")
            {
                $("#txtModelNo").attr("readonly", true);
            }
         
            $("option").each(function () {
                $(this).attr("title", $(this).text());
            });
            
            if ('<%=Request["ModelNo"]%>'=="")
            {
                AutoGenerateNo(true);
            }            
            
        });

         $.ajaxSetup({ cache: false });
        
       function BindProcess() {
           var ddlProcessID = '<%=ordreModel.HospitalID%>';
           $("[id*=ddlHosipital]").html(""); $("#ddlDoctor").html("");
           $("[id*=ddlHosipital]").append("<option value='-1' selected='selected'>" + selectAll + "</option>");
           var strId = $("[id*=ddlSeller]")[0].value;
            if (strId != -1) {
                //$("#LoadingDialog").show();
                $.getJSON("../Handler/GetDataHandler.ashx?IsCN=<%=IsCN %>&ddlType=Process&ddlId=" + strId, function (data) {

                    for (var i = 0; i < data.length; i++) {
                      
                        if (data[i].ID == ddlProcessID) {
                            $("select[name$=ddlHosipital]").append($("<option selected></option>").val(data[i].ID).html(data[i].Cname));
                        } else {
                            $("select[name$=ddlHosipital]").append($("<option></option>").val(data[i].ID).html(data[i].Cname));
                        }

                        if (userHospitalID != "") {
                            $("[id*=ddlHosipital]").val(userHospitalID);
                            $("[id*=ddlHosipital]").attr("disabled", true);
                        }
                    };
                   
                    BindVersion();
                });

            } else {
                
                BindVersion();
            }            
       }

        

        function BindVersion() {
            var ddlVersionID = '<%=ordreModel.DoctorId%>';
            $("[id*=ddlDoctor]").html("");
            //$("#LoadingDialog").show();

            var strId = $("[id*=ddlHosipital]")[0].value;

            if (strId != -1 && strId != 0) {
                $.getJSON("../Handler/GetDataHandler.ashx?IsCN=<%=IsCN %>&ddlType=Version&ddlId=" + strId, function (data) {
                    $("[id*=ddlDoctor]").append("<option value='-1' selected='selected'>" + selectAll + "</option>");
                    for (var i = 0; i < data.length; i++) {
                        if (data[i].ID == ddlVersionID) {
                            $("select[name$=ddlDoctor]").append($("<option selected></option>").val(data[i].ID).html(data[i].Cname));
                        } else {
                            $("select[name$=ddlDoctor]").append($("<option></option>").val(data[i].ID).html(data[i].Cname));
                        }
                        if (userDoctorID != "") {
                            $("[id*=ddlDoctor]").val(userDoctorID);
                            $("[id*=ddlDoctor]").attr("disabled", true);
                        }
                    };
                    $("option").each(function () {
                        $(this).attr("title", $(this).text());
                    }); 
                    //$("#LoadingDialog").hide();
                });
            } else {
                $("[id*=ddlDoctor]").append("<option value='-1' selected='selected'>" + selectAll + "</option>");
                //$("#LoadingDialog").hide();
                $("option").each(function () {
                    $(this).attr("title", $(this).text());
                }); 
            }
        }
        

        function OrderAdd() {

            var txtModelNo = $("#txtModelNo").val();
            if (txtModelNo == "") {
                alertdialog('<%= GetGlobalResourceObject("Resource","OrderBMNotEmpty").ToString() %>');
                return false;
            }

            if (CheckUniqueNo(txtModelNo) == false)
            {
                return false;
            }

            var ddlSeller = $("select[id*=ddlSeller]").val();
            if (ddlSeller == "-1") {
                alertdialog('<%= GetGlobalResourceObject("Resource","SellerNotEmpty").ToString() %>');
                return false;
            }

            var ddlHosipital = $("select[id*=ddlHosipital]").val(); 
            if (ddlHosipital == "-1") {
                alertdialog('<%= GetGlobalResourceObject("Resource","HospitalNotEmpty").ToString() %>');
                return false;
            }

            var ddlDoctor = $("select[id*=ddlDoctor]").val();
            if (ddlDoctor == "-1") {
                alertdialog('<%= GetGlobalResourceObject("Resource","DoctorNotEmpty").ToString() %>');
                return false;
            }

            var Require = $("#Require").val()
            if (Require.length > 200)
            {
                alertdialog('<%= GetGlobalResourceObject("Resource","RequireNotThan200").ToString() %>');
                return false;
            }

            if ($("#ProductList tr").length == 1)
            {
                alertdialog('<%= GetGlobalResourceObject("Resource","PleaseAddProduct").ToString() %>');
                return false;
            }

            $("[id*=ddlSeller]").attr("disabled", false);
            $("[id*=ddlHosipital]").attr("disabled", false);
            $("[id*=ddlDoctor]").attr("disabled", false);
            $("#haddinfo").val("1");

            document.forms[0].submit();
        }

      
        function RemoveProducts(index) {
            art.dialog({
                title: false,
                icon: 'question',
                content: '<%= GetGlobalResourceObject("Resource","ConfirmDeleteProduct").ToString() %>',
                ok: function () {
                    var date = Date.parse(new Date());
                    $("#ProductList tr:not(:first)").remove();
                    $.get("../Handler/GetDataHandler.ashx?t=date&IsCN=<%=IsCN %>&ddlType=RemoveProduct&index=" + index + "&number=" + "1", function (data) {
                    $("#ProductList").append(data);
                });
                },
                cancelVal: '<%= GetGlobalResourceObject("Resource","Cancel").ToString() %>',
                cancel: true
            });           
        }
        function backtolist() {
            window.history.go(-1);
        }

        function OthersAdd() {
            dialog = art.dialog({
                id: 'PIC005',
                title: '<%= GetGlobalResourceObject("Resource","OtherInput").ToString() %>',
                background: '#000',
                opacity: 0.3,
                close: function () {
                }
            });

            $.ajax({
                url: "addOthers.aspx?OtherList=" + $("#OtherList").val(),
                success: function (data) {
                    dialog.content(data);
                },
                cache: false
            });
        }

       

        function PhotoAdd() {

            dialog = art.dialog({
                id: 'PIC0016',
                title: '<%= GetGlobalResourceObject("Resource","UploadViewPicture").ToString() %>',
                width: 1000,
                height: 600,
                background: '#000',
                opacity: 0.3,

                close: function () {

                }

            });

            $.ajax({
                url: "UploadPhoto.aspx?photoList=" + $("#photoList").val(),
                success: function (data) {
                    dialog.content(data);
                },
                cache: false
            });
        }
        
        function ProductAdd(subid) {
            dialog = art.dialog({
                id: 'PIC0009',
                title: "<%= GetGlobalResourceObject("Resource","ProductInput").ToString() %>",
                width: 600,
                height: 300,
                background: '#000',
                opacity: 0.3,

                close: function () {

                }

            });

            $.ajax({
                url: "addProduct.aspx?subid=" + subid,
                success: function (data) {
                    
                    dialog.content(data);
                },
                cache: false
            });
        }

        function AutoGenerateNo(IsChecked)
        {
            $("#txtModelNo").focus();
            if (IsChecked) {
                $.ajax( {
                    url:"OrderInput.aspx",
                    data:{action:"autoNo"},
                    success:function(data)
                    {
                        $("#txtModelNo").val(data);
                      
                    }

                })
            }
            else {
                $("#txtModelNo").val("");
            }
        }

        function CheckUniqueNo(orderNo)
        {
            var checkUnique = true;
            if ($.trim(orderNo) != "" && $("#keyID").val()=="") {
                $.ajax({
                    url: "OrderInput.aspx",
		    async:false,
                    data: { action: "checkUnique", orderNo: orderNo },
                    success: function (data) {
                        if (data == "1")
                        {
                            alertdialog('<%= GetGlobalResourceObject("Resource","OrderBMNotSame").ToString() %>');
                            checkUnique = false;
                        }
                    }

                })
            }
            return checkUnique;
        }
    </script>
   </head>
    <body>
        <form runat="server">
    <uccontrol:weixintop ID="topControl" TopTitle="订单输入" runat="server" />
    <input id="haddinfo" name="haddinfo" type="hidden" />
    <input id="keyID" name="keyID" type="hidden" value="<%=ordreModel.ModelNo %>" />
    
    <input id="OtherList" name="OtherList" type="hidden" value="<%=strOtherList %>" />   

    <input id="photoList" name="photoList" type="hidden" value="<%=strPhotoList %>" />

    <div class="nowposition"><%= GetGlobalResourceObject("Resource","DoctorOrder").ToString() %> > <%= GetGlobalResourceObject("Resource","OrderManagement").ToString() %> > <b><%= GetGlobalResourceObject("Resource","AddOrder").ToString() %></b></div>   
    <div class="expertnumber"><a href="javascript:void(0)" class="btn01" style="font-size:13px;" onclick="OthersAdd();"><%= GetGlobalResourceObject("Resource","OrderOther").ToString() %></a>
        &nbsp;&nbsp;<a href="javascript:void(0)" style="font-size:13px;"  class="btn01"  onclick="PhotoAdd();"><%= GetGlobalResourceObject("Resource","UploadPicture").ToString() %></a>
        &nbsp;&nbsp;<a href="javascript:void(0)" style="font-size:13px;"  class="btn01"  onclick="ProductAdd(0);"><%= GetGlobalResourceObject("Resource","AddProduct").ToString() %></a>        
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="javascript:void(0)" style="font-size:13px;" class="btn01" onclick="OrderAdd();"><%= GetGlobalResourceObject("Resource","SaveOrder").ToString() %></a>
    </div>
    <table class="edittb" style="display:none;">
        <tr>
            <td style="width:100px;">
               <%= GetGlobalResourceObject("Resource","OrderBM").ToString() %>:
            </td>
            <td style="width:260px;">                
                <input type="checkbox" runat="server" id="autoOrderNo" style="display:none;"   onchange="AutoGenerateNo(this.checked)" value="1" /><asp:Label ID="autoLable" Visible="false" runat="server"><%= GetGlobalResourceObject("Resource","AutoGenerate").ToString() %>&nbsp;&nbsp;&nbsp;&nbsp;</asp:Label> <input id="txtModelNo" name="txtModelNo" readonly="readonly"   style="width: 150px;" value="<%=ordreModel.ModelNo %>" onblur="CheckUniqueNo(this.value)" type="text" maxlength="20" />
            </td>
            <td  style="width:100px;">
                <%= GetGlobalResourceObject("Resource","OrderCategory").ToString() %>:
            </td>
            <td  style="width:260px;">
                <select id="ddlOrderType" datatextfield="DictName" datavaluefield="Code" runat="server" name="ddlOrderType" style="width: 150px;" >
                </select>
                <%-- <script type="text/javascript">
                     if ('<%=ordreModel.OrderClass%>' != "") {
                         $("#ddlOrderType").val('<%=ordreModel.OrderClass%>');
                     }
                     
                </script>--%>
            </td  >
            <td style="width:100px;"><%= GetGlobalResourceObject("Resource","IsDanzuo").ToString() %>:</td>
            <td>
                 <select id="ddlSingle" name="ddlSingle" style="width: 150px;" >
                    <option value="Y" selected="selected" ><%= GetGlobalResourceObject("Resource","Danzuo").ToString() %></option>
                    <option value="N"><%= GetGlobalResourceObject("Resource","Lianzuo").ToString() %></option>                    
                </select>
                 <script type="text/javascript">
                     if ('<%=ordreModel.danzuo%>' != "") {
                         $("#ddlSingle").val('<%=ordreModel.danzuo%>');
                     }
                    
                </script>
            </td>            
        </tr>
       
        <tr>
             <!--业务员-->
            <td ><%= GetGlobalResourceObject("Resource","Seller").ToString() %>:</td>
            <td>
    <asp:DropDownList ID="ddlSeller" Width="150" runat="server"  >
    </asp:DropDownList>
            </td>
            <!--医院-->
            <td ><%= GetGlobalResourceObject("Resource","Hospital").ToString() %>:</td>
            <td>
    <asp:DropDownList ID="ddlHosipital" Width="150" runat="server">                               
    </asp:DropDownList>
            </td>
            <!--医生-->
            <td ><%= GetGlobalResourceObject("Resource","Doctor").ToString() %>:</td>
            <td>                                
    <div style=" float:left"><asp:DropDownList ID="ddlDoctor"  Width="150"  runat="server">
    </asp:DropDownList></div> <div id="LoadingDialog" style="display:none;position:relative; left:50px; top:-16px; width:15px; " >
                    <img src="../Images/Loading.gif" alt="" height="20px;" />
                </div>
            </td>
        </tr>
        <tr>
            <td><%= GetGlobalResourceObject("Resource","Patient").ToString() %>:</td>
            <td>
                <input id="txtpatient" name="txtpatient"  style="width: 150px;" value="<%=ordreModel.Patient %>" type="text" maxlength="25" />
            </td>
            <td><%= GetGlobalResourceObject("Resource","Age").ToString() %>:</td>
            <td>
                <input id="txtAge" name="txtAge"  style="width: 150px;" onkeydown="onlyNum()" value="<%=ordreModel.Age %>" type="text" maxlength="3" />
            </td>
            <td><%= GetGlobalResourceObject("Resource","Sex").ToString() %>:</td>
            <td>
                 <select id="ddlSex" name="ddlSex"  >
                    <option value="1" ><%= GetGlobalResourceObject("Resource","male").ToString() %></option>
                    <option value="0"><%= GetGlobalResourceObject("Resource","female").ToString() %></option>
                </select>
                 <script type="text/javascript">
                     if ('<%=ordreModel.Sex%>' == "") {
                         $("#ddlSex").val(1);
                     } else {
                         $("#ddlSex").val('<%=ordreModel.Sex%>');
                     }

                </script>
            </td>
        </tr>
         <tr>
            <td><%= GetGlobalResourceObject("Resource","DMSplit").ToString() %>:</td>
            <td><select id="ddlDivision" name="ddlDivision" style="width: 150px;" >
                    <option value="Y" selected="selected" ><%= GetGlobalResourceObject("Resource","DMSplit").ToString() %></option>
                    <option value="N"><%= GetGlobalResourceObject("Resource","NotSplit").ToString() %></option>                    
                </select>
                 <script type="text/javascript">
                     if ('<%=ordreModel.Fenge%>' != "") {
                         $("#ddlDivision").val('<%=ordreModel.Fenge%>');
                     }          
                </script></td>
             <td><%= GetGlobalResourceObject("Resource","DoctorRequire").ToString() %>:</td>
             <td colspan="3"><asp:DropDownList runat="server" ID="ddlRequireTemplate"></asp:DropDownList>&nbsp;&nbsp;&nbsp;&nbsp;<div style="margin-top:4px;"><textarea id="Require" name="Require" cols="60" ><%=ordreModel.Require %></textarea></div></td>
             
        </tr>
        </table>
    
    <div style="overflow:auto;width:100%; height:50%; ">
            <table class="tablestyle" id="ProductList">
                <tr>
                    <th style="width:100px;"><%= GetGlobalResourceObject("Resource","Operation").ToString() %></th>
                    <th style="width:50px;"><%= GetGlobalResourceObject("Resource","OrderNumber").ToString() %></th>
                    <th style="width:100px;"><%= GetGlobalResourceObject("Resource","Product").ToString() %></th>
                   
                    <th style="width:50px;"><%= GetGlobalResourceObject("Resource","Quantity").ToString() %></th>
                    <th style="width:300px;"><%= GetGlobalResourceObject("Resource","TeethPosition").ToString() %></th>
                    <th style="width:150px;"><%= GetGlobalResourceObject("Resource","Color").ToString() %></th>
                    <%--<th style="width:100px;">生产线</th>--%>
                    <th style="width:100px;"><%= GetGlobalResourceObject("Resource","NobleMetal").ToString() %></th> 
                    <th style="width:100px;"><%= GetGlobalResourceObject("Resource","ProductNo").ToString() %></th>          
                </tr>
                <asp:Repeater ID="repProductList" runat="server">
                    <ItemTemplate>
                        <tr class="<%# Container.ItemIndex % 2 == 0 ? "" : "dli0"%>">
                            <td align="center">
                                <a href='javascript:void(0)'  onclick='RemoveProducts("<%#DataBinder.Eval(Container.DataItem, "subId")%>");'><%= GetGlobalResourceObject("Resource","Delete").ToString() %></a>
                                &nbsp;&nbsp;<a href='javascript:void(0)'  onclick='ProductAdd("<%#DataBinder.Eval(Container.DataItem, "subId")%>");'><%= GetGlobalResourceObject("Resource","Edit").ToString() %></a>
                            </td>
                            <td align="center"><%#DataBinder.Eval(Container.DataItem, "subId")%></td>                   
                            <td align="center"><%#DataBinder.Eval(Container.DataItem, "ItemName")%></td>  
                            <td align="center"><%#DataBinder.Eval(Container.DataItem, "Qty")%></td>
                            <td style="padding:0px 0px 0px 0px;"><table style="width:100%;border-collapse:separate"  border="0" ><tr><td style="width:50%; text-align:right;"><%#DataBinder.Eval(Container.DataItem, "a_teeth")%></td><td><%#DataBinder.Eval(Container.DataItem, "b_teeth")%></td></tr><tr><td style="text-align:right;"><%#DataBinder.Eval(Container.DataItem, "c_teeth")%></td><td><%#DataBinder.Eval(Container.DataItem, "d_teeth")%></td></tr></table></td>
                            <td align="center"><%#DataBinder.Eval(Container.DataItem, "bColor")%></td>
                            <td align="center"><%#DataBinder.Eval(Container.DataItem, "Nobleclass")%></td>
                            <td align="center"><%#DataBinder.Eval(Container.DataItem, "ProductId")%></td>                       
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>        
        </div><div style="clear:both"></div>
            <uccontrol:weixinbottom ID="weixinbottom" runat="server" />
            </form>
       </body>
</html>
