<%@ Page Title="" Language="C#" MasterPageFile="~/Weixinclient/App_Master/all_master.Master" AutoEventWireup="true" CodeFile="AnalysisPie.aspx.cs" Inherits="Weixinclient_AnalysisPie" %>
<%@ Register Src="~/Weixinclient/ascx/pagecutphone.ascx" TagName="pageweixincut" TagPrefix="uc1" %>
<%@ Register Src="~/Weixinclient/ascx/querycontrolSC.ascx" TagName="querycontrolSC" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link rel="stylesheet" type="text/css" href="css/layer.css">
    <link href="css/bootstrap.min.css" rel="stylesheet"/>
   <script type="text/javascript" src="js/layer.js"></script>
    <script type="text/javascript" src="js/bootstrap.min.js"></script>
   <script type="text/javascript">
       $(function () {
           $.ajaxSettings.async = false;
            BindFilter();
           
            $(".modal-header div select").bind("change", function ()
            {
                //var datefrom = $("#datefrom").val()
                //var dateto = $("#dateto").val()  
                //var filter = "&datefrom=" + datefrom + "&dateto=" + dateto;
                //if ($(this).attr("id") == "ddlClass")
                //{
                //    $("#ddlProduct").html("");
                //    $.getJSON("Handler/GetDataHandler.ashx?ddlType=GetProductAnalysis&SmallClass=" + escape($("#ddlClass").find("option:selected").text()) + "&k=" + new Date() + filter, function (data) {
                //        for (var i = 0; i < data.length; i++) {
                //            if (i == 0) {
                //                $("#ddlProduct").append($("<option selected></option>").val(data[i].Value).html(data[i].Name));
                //            }
                //            else {
                //                $("#ddlProduct").append($("<option></option>").val(data[i].Value).html(data[i].Name));
                //            }
                //        }

                //    });
                //}
               

                SetPieChart("areaContainer", "地区占比分析表", $("#ddlArea").find("option:selected").text(), $("#ddlArea").val());
                SetPieChart("classContainer", "产品小类分析表", $("#ddlClass").find("option:selected").text(), $("#ddlClass").val());
                SetPieChart("productContainer", "产品销售分析表", $("#ddlProduct").find("option:selected").text(), $("#ddlProduct").val());
            })

           

            $(".modal-header div select").change();
        });

        function SetPieChart(divid,title,name,rate)
        {
            $("#" + divid).empty();
            
          // Build the chart
            Highcharts.chart(divid, {
            chart: {
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false,
            type: 'pie'
                },
                title: {
                    text: title
                },
                tooltip: {
                        pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
                },
                plotOptions: {
                        pie: {
                            allowPointSelect: true,
                            cursor: 'pointer',
                            dataLabels: {
                                enabled: false,
                                format:'{series.name}: <b>{point.percentage:.1f}%</b>'
                            },
                            showInLegend: true,
                           
                            }
                },
                series: [{
                    name: '占比',
                    colorByPoint: true,
                    data: [{
                        name: name,
                        y: parseFloat(rate),
                        color: "#feb028"
                    }, {
                        name: '其他',
                        y: (100 - parseFloat(rate)),
                        sliced: true,
                        selected: true,
                        color: "#00c1f6"
                    }]
                }]
             });
        }

        
        function BindFilter()
        {
            var datefrom = $("#datefrom").val()
           
            var dateto = $("#dateto").val()
            if ('<%=Request["submitflg"]%>' == null || '<%=Request["submitflg"]%>' == '') {
                datefrom = new Date().getFullYear() + "-" + (new Date().getMonth() + 1) + "-"+new Date().getDate();
            }
            var filter = "&datefrom=" + datefrom + "&dateto=" + dateto;
            $("#ddlArea").html("");            
            $.getJSON("Handler/GetDataHandler.ashx?ddlType=GetAreaAnalysis&k=" + new Date() + filter, function (data) {
                for (var i = 0; i < data.length; i++)
                {
                    if (i == 0)
                    {
                        $("#ddlArea").append($("<option selected></option>").val(data[i].Value).html(data[i].Name));
                    }
                    else {
                        $("#ddlArea").append($("<option></option>").val(data[i].Value).html(data[i].Name));
                    }
                }
                    
            });

            $("#ddlClass").html("");
            $.getJSON("Handler/GetDataHandler.ashx?ddlType=GetClassAnalysis&k=" + new Date() + filter, function (data) {
                for (var i = 0; i < data.length; i++) {
                    if (i == 0) {
                        $("#ddlClass").append($("<option selected></option>").val(data[i].Value).html(data[i].Name));
                    }
                    else {
                        $("#ddlClass").append($("<option></option>").val(data[i].Value).html(data[i].Name));
                    }
                }

            });

            $("#ddlProduct").html("");
            $.getJSON("Handler/GetDataHandler.ashx?ddlType=GetProductAnalysis&SmallClass=" + escape($("#ddlClass").find("option:selected").text()) + "&k=" + new Date() + filter, function (data) {
                for (var i = 0; i < data.length; i++) {
                    if (i == 0) {
                        $("#ddlProduct").append($("<option selected></option>").val(data[i].Value).html(data[i].Name));
                    }
                    else {
                        $("#ddlProduct").append($("<option></option>").val(data[i].Value).html(data[i].Name));
                    }
                }

            });
        }
       function SwitchFilter(filterID)
       {
           $(".modal-header div").hide();
           $("#" + filterID).show();
       }
    </script>
<section >
   <script src="js/highcharts.js"></script>
    <uc2:querycontrolSC ID="querycontrolSC" runat="server" />
     <div  id="SettingModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" style="margin-top:45px;" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    
                  <div id="AreaFilter"><label style="height:30px; width:100px;">地区：</label><select id="ddlArea" name="ddlArea" style="border:1px solid #ededed;height:30px; 

width:50%;"></select></div>
                  <div id="ClassFilter" style="display:none;"><label style="height:30px; width:100px;">产品小类：</label><select id="ddlClass" name="ddlClass" 

style="border:1px solid #ededed;height:30px; width:50%;"></select></div>
                  <div id="ProductFilter" style="display:none;"><label style="height:30px; width:100px;">产品名称：</label><select id="ddlProduct" name="ddlProduct" 

style="border:1px solid #ededed;height:30px; width:50%;"></select></div>

                </div>
                <div class="modal-body">
                    <ul id="myTab" class="nav nav-tabs">
	                    <li class="active"><a href="#RoleA" data-toggle="tab" onclick="SwitchFilter('AreaFilter')">地区</a></li>
	                    <li><a href="#RoleB" data-toggle="tab"  onclick="SwitchFilter('ClassFilter')">产品小类</a></li>
	                    <li><a href="#RoleC" data-toggle="tab"  onclick="SwitchFilter('ProductFilter')">产品销售</a></li>
                       
                    </ul>
                    <div id="myTabContent" class="tab-content">
	                    <div class="tab-pane  in active" id="RoleA" >
                             <div id="areaContainer" style=" height: 260px; width: 260px; margin: auto"></div>
	                    </div>
	                    <div class="tab-pane " id="RoleB" >
                             <div id="classContainer" style=" height: 260px; width:260px; margin: auto"></div>		 
	                    </div>
	                    <div class="tab-pane " id="RoleC" >
                             <div id="productContainer" style=" height: 260px; width: 260px; margin: auto"></div>
	                    </div>
	                   
                    </div>
                </div>
            </div>
        </div>
    </div>
   <ul  style="display:none;">
       <uc1:pageweixincut ID="pagecutID" runat="server" />
    </ul>
    <!--earch-list end-->
  
  </section>
  <!--section   end-->
</asp:Content>
