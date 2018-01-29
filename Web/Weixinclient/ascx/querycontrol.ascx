<%@ Control Language="C#" AutoEventWireup="true" CodeFile="querycontrol.ascx.cs" Inherits="Weixin.RGPWEB.ascx.querycontrol" %>

<script src="/jQuery/SplitPage.js" type="text/javascript"></script>
<script src="js/jquery.charfirst.pinyin.js" type="text/javascript"></script>
<script src="js/sort.js" type="text/javascript"></script>
<script type="text/javascript" language="javascript">
    var userDoctorID = '<%=userDoctorID%>'
    var userHospitalID = '<%=userHospitalID%>'
    var userSellerID = '<%=userSellerID%>'
    $(document).ready(function () {

        var arr = document.getElementById("Sort-Sort").getElementsByTagName("li");
        var index = '<%=Request["sortList"]%>'
        if (index == "" || index == null)
        {
            index = 0;
        }

        $("#sortList").val(index)
        for (var i = 1; i < arr.length; i++) {
            var a = arr[i];
            if (index == i-1)
            {
                a.style.background = "#eee"
            }
            else {
                a.style.background = "";
            }         
        };

        selectAll = '<%= GetGlobalResourceObject("Resource", "PleaseSelect").ToString() %>';
        BindSeller();
        BindSmallClass();
        BindProductLine();
        $("#ddlSeller").change(function () {
           
            BindProcess();
            //var sellerid = $("#ddlSeller").value();
        });
        $("#ddlHosipital").change(function () {BindVersion(); });

        $("option").each(function () {
            $(this).attr("title", $(this).text());
        });

        $('.choosebox li div').click(function () {
            var thisToggle = $(this).is('.size_radioToggle') ? $(this) : $(this).prev();
            var checkBox = thisToggle.prev();
            checkBox.trigger('click');
            $('.size_radioToggle').removeClass('current');
            thisToggle.addClass('current');
        });
       
        if ('<%=Request["ddlOrderType"]%>' != "")
        {
            $.each('<%=Request["ddlOrderType"]%>'.split(','), function (i, val) {
               
                $("#"+val).attr("checked", true);
            });
           
        }
       
        if ('<%=Request["submitflg"]%>' == "0" || '<%=Request["submitflg"]%>' == '')
        {

	    if('<%=DispalyType%>' == '')
	    {
 		$.each('A,B,C,D'.split(','), function (i, val) {
               
                	$("#"+val).attr("checked", true);
            	});
	    }else{
		$.each('A'.split(','), function (i, val) {
               
                	$("#"+val).attr("checked", true);
            	});
	    }
           
        }

        $("#btnddlHosipital").click(function () {
           // $(this).attr("disabled", true);
            $("#containerHospital").show();
        })

        $("#ddlHosipital").click(function () {
            $(this).attr("disabled",true);
            $("#containerHospital").show();
        })
        $("#closeIndex").click(function () {
            //$("#ddlHosipital").attr("disabled", false);
            $("#containerHospital").hide();
        })
        
        
    });

    $.ajaxSetup({ cache: false });

     function BindSmallClass()
    {
         var ddlSmallClass = '<%=Request["ddlSmallClass"]%>';
        $("#ddlSmallClass").html("");
        $(" #ddlSmallClass").append("<option value='-1' selected='selected'>" + selectAll + "</option>");
        $.getJSON("../Handler/GetDataHandler.ashx?IsCN=<%=IsCN %>&ddlType=BindSmallClass", function (data) {
                for (var i = 0; i < data.length; i++) {                       
                    $("#ddlSmallClass").append($("<option></option>").val(data[i].Cname).html(data[i].Cname));
                };

                if (ddlSmallClass != "") {
                    $("#ddlSmallClass").find("option[value='" + ddlSmallClass + "']").attr("selected", true);
                }
        });
        
       
             
     }

    function BindProductLine()
    {
        var ddlProductLine = '<%=Request["ddlProductLine"]%>';
        $("#ddlProductLine").html("");
        $(" #ddlProductLine").append("<option value='-1' selected='selected'>" + selectAll + "</option>");
        $.getJSON("../Handler/GetDataHandler.ashx?IsCN=<%=IsCN %>&ddlType=ProductLine", function (data) {
                for (var i = 0; i < data.length; i++) {                       
                    $("#ddlProductLine").append($("<option></option>").val(data[i].Cname).html(data[i].Cname));
                };
                if (ddlProductLine != "") {

                    $("#ddlProductLine").find("option[value='" + ddlProductLine + "']").attr("selected", true);
                }
        });
               
     }

    function BindSeller() {
        var ddlsellerID = '<%=Request["seller"]%>';
        $("#ddlSeller").html(""); $("#ddlHosipital").html("");
        $("#ddlSeller").append("<option value='-1' selected='selected'>" + selectAll + "</option>");
        //$("#LoadingDialog").show();
        $.getJSON("../../Weixinclient/Handler/SellerHospitalDoctor.ashx?IsCN=<%=IsCN %>&ddlType=Seller", function (data) {

            for (var i = 0; i < data.length; i++) {
                $("#ddlSeller").append($("<option></option>").val(data[i].ID).html(data[i].Cname));
            };
            

            if (userSellerID != "") {
                $("#ddlSeller").val(userSellerID);
                $("#ddlSeller").attr("disabled", true);
            }
            else if (ddlsellerID != "" && ddlsellerID != null) {
                
                $("#ddlSeller").val(ddlsellerID);
            }
            else {               
                $("#ddlSeller").val(-1);
            }

            BindProcess();
        });

    }

    function BindSelectHospital(id,name)
    {
        $("#ddlHosipital").attr("disabled", false);
        $("#ddlHosipital").html("");
       // $("#ddlHosipital").append($("<option selected></option>").val(-1).html(""));
        $("#ddlHosipital").append($("<option selected></option>").val(id).html(name));
        $("#ddlHosipital").find("option[value='" + id + "']").attr("selected", true);
        $("#containerHospital").hide();
        //$("#ddlHosipital").attr("disabled", true);
        BindVersion();
        
    }
    function BindProcess() {
        var ddlhospitalID = '<%=Request["hospital"]%>';
        $("#ddlHosipital").html(""); $("#ddlDoctor").html("");
        $(".sort_box").html("");
            $("#ddlHosipital").append("<option value='-1' selected='selected'>" + selectAll + "</option>");
            var strId = $("#ddlSeller")[0].value;
            if (strId != -1) {          
                //$("#LoadingDialog").show();
                $.getJSON("../../Weixinclient/Handler/SellerHospitalDoctor.ashx?IsCN=<%=IsCN %>&ddlType=Process&ddlId=" + strId, function (data) {
                    
                    for (var i = 0; i < data.length; i++) {

                        if (data[i].ID == ddlhospitalID) {
                            $("#ddlHosipital").append($("<option selected></option>").val(data[i].ID).html(data[i].Cname));
                        } else {
                            $("#ddlHosipital").append($("<option></option>").val(data[i].ID).html(data[i].Cname));
                        }
                        $(".sort_box").append("<div class=\"sort_list\"><div class=\"num_logo\" ><img src=\"images/home-a.png\" alt=\"\"></div><div class=\"num_name\" onclick=\"BindSelectHospital('" + data[i].ID + "','" + data[i].Cname + "')\">" + data[i].Cname+"</div></div>");
                        if (userHospitalID != "") {
                            $("#ddlHosipital").val(userHospitalID);
                           // $("#ddlHosipital").attr("disabled", true);
                        }
                    };
                    ContructPY();
                    BindVersion();
                });

            } else {

                BindVersion();
            }
        }

        function BindVersion() {
          
           $("#ddlDoctor").html("");
           //$("#LoadingDialog").show();
           var ddlDoctorID = '<%=Request["doctor"]%>';
           var strId = $("#ddlHosipital")[0].value;

           if (strId != -1 && strId != 0) {

               $.getJSON("../../Weixinclient/Handler/SellerHospitalDoctor.ashx?IsCN=<%=IsCN %>&ddlType=Version&ddlId=" + strId, function (data) {

                    $("#ddlDoctor").append("<option value='-1' selected='selected'>" + selectAll + "</option>");
                    for (var i = 0; i < data.length; i++) {
                        if (data[i].ID == ddlDoctorID) {
                            $("#ddlDoctor").append($("<option selected></option>").val(data[i].ID).html(data[i].Cname));
                        } else {
                            $("#ddlDoctor").append($("<option></option>").val(data[i].ID).html(data[i].Cname));
                        }
                        if (userDoctorID != "") {
                            $("#ddlDoctor").val(userDoctorID);
                            $("#ddlDoctor").attr("disabled", true);
                        }
                    };
                    $("option").each(function () {
                        $(this).attr("title", $(this).text());
                    });
                    //$("#LoadingDialog").hide();
                });
            } else {
                $("#ddlDoctor").append("<option value='-1' selected='selected'>" + selectAll + "</option>");
                //$("#LoadingDialog").hide();
                $("option").each(function () {
                    $(this).attr("title", $(this).text());
                });
            }
        }
    function QueryData()
    {
      
        $("#ddlHosipital").attr("disabled", false);
      
        $("#submitflg").val("1");
        document.forms[0].submit();
    }
    </script>
<div class="screening">
      <ul>
          <%if (DispalyType == "0") { %>
          <li class="Sort"><%=TranslatChinese("排序","Order")%></li>
          <li class="Brand"><%=TranslatChinese("筛选","Filter")%></li>
          <%}
              else if (DispalyType == "1") {%>
          <li class="Brand" style="width:100%;"><%=TranslatChinese("筛选","Filter")%></li>
          <%} else {%>
           <li class="Sort"><%=TranslatChinese("排序","Order")%></li>
          <li class="Brand"><%=TranslatChinese("筛选","Filter")%></li>
          <%} %>
      </ul>
    </div>
    <div class="Sort-eject Sort-height">
      
        <ul class="Sort-Sort" id="Sort-Sort">
            <li><input type="radio" name="ordercheck" value="0" <%if (Request["ordercheck"] == null || Request["ordercheck"] == "0"){ %>checked="checked"<%}else{ 

%>""<%} %> /><%=TranslatChinese("升序","Asc")%>
                &nbsp;&nbsp;&nbsp;&nbsp;<input type="radio" name="ordercheck" <%if (Request["ordercheck"] == "1"){ %>checked="checked"<%}else{ %>""<%} %> value="1" 

/><%=TranslatChinese("降序","Desc")%></li>
            <li onclick="Sorts(this,0)"><%=TranslatChinese("条码","Assist Code")%></li>
            <li onclick="Sorts(this,1)"><%=TranslatChinese("入厂时间","Indate")%></li>
            <li onclick="Sorts(this,2)"><%=TranslatChinese("出厂时间","Outdate")%></li>
            <%if (String.IsNullOrEmpty(DispalyType)) { %>
            <li onclick="Sorts(this,3)"><%=TranslatChinese("超期","Expire")%></li>
            <li onclick="Sorts(this,4)"><%=TranslatChinese("加急","Hurry")%></li>
            <li onclick="Sorts(this,5)"><%=TranslatChinese("试戴","TryPut")%></li>
            <li onclick="Sorts(this,6)"><%=TranslatChinese("放缓","Slow")%></li>
            <li onclick="Sorts(this,7)"><%=TranslatChinese("订单类别","Order Class")%></li>
            <%} %>
        </ul>
        <input type="hidden" id="sortList" name="sortList" value="0" />
    </div>
    <div class="Category-eject">
        <ul class="Category-w" id="Categorytw" >
            <li onclick="Categorytw(this)">
                <div class="choose" id="dress-size">
                    <%--<form action="" method="get">--%>
                        <div class="choosebox" style="height:300px; overflow:auto;">
                            <ul class="clearfix">
                                <li>
                                    <div class="choosebox-div" ><input type="radio" name="datetype" value="0" <%if(Request["datetype"]==null && !String.IsNullOrEmpty(DispalyType)  || Request["datetype"] == "0"){ %>checked="checked"<%}else{ %>""<%} %> class="size_radioToggle"><span class="value"><%= GetGlobalResourceObject("SystemResource", "inFactory").ToString() 

%></span></div>
                                    <div class="choosebox-div" ><input type="radio" name="datetype" value="1" <%if(Request["datetype"] == "1"){ %>checked="checked"<%}

else{ %>""<%} %> class="size_radioToggle" ><span class="value"><%= GetGlobalResourceObject("SystemResource", "OutFacotry").ToString() %></span></div>

                                     <div class="choosebox-div" ><input type="radio" name="datetype" value="2" <%if(Request["datetype"] == "2"){ %>checked="checked"<%}

else{ %>""<%} %> class="size_radioToggle" ><span class="value"><%= GetGlobalResourceObject("SystemResource", "PreOutFactory").ToString() %></span></div>

                                     <div class="choosebox-div" ><input type="radio" name="datetype" value="3" <%if(Request["datetype"] == "3"){ %>checked="checked"<%}

else{ %>""<%} %> class="size_radioToggle" ><span class="value"><%= GetGlobalResourceObject("SystemResource", "Try").ToString() %></span></div>

                                    <div class="clear" ></div>
                                </li>
                                <li >
                                    <i class="choosebox-i" ><%= GetGlobalResourceObject("SystemResource", "DateFrom").ToString() %>:</i>
                                    <input type="date" name="datefrom" value="<%=Request["submitflg"]==null && String.IsNullOrEmpty(Request["datefrom"])?DateTime.Now.ToString("yyyy-MM-")+"01":Request["datefrom"] %>" class="choosebox-data" >
                                </li>
                                <li >
                                    <i class="choosebox-i" ><%= GetGlobalResourceObject("SystemResource", "DateTo").ToString() %>:</i>
                                    <input type="date" name="dateto" value="<%=Request["dateto"] %>" class="choosebox-data" >
                                </li>
                                <%for (int k = 0; k < (dtCategory.Rows.Count - 1) / 4 + 1; k++)
                                         { %>
                                <li>
                                    <i class="choosebox-i"> <%=k==0? GetGlobalResourceObject("Resource", "OrderCategory").ToString():"" %><%=k==0?":":"" %></i>
                                     <%for (int i = k * 4; i < k * 4+4 && i<dtCategory.Rows.Count; i++)
                                         {%>
                                        <input name="ddlOrderType" type="checkbox" id="<%=dtCategory.Rows[i]["Code"] %>" value="<%=dtCategory.Rows[i]["Code"] %>" /><%=dtCategory.Rows[i]["DictName"] %>
                                        <%} %>
                                    <%--<select id="ddlOrderType" name="ddlOrderType" class="choosebox-text" >
                                        <option value="-1" selected="selected" ><%= GetGlobalResourceObject("SystemResource","PleaseSelect").ToString() %></option>
                                        <%for (int i = 0; i < dtCategory.Rows.Count; i++) {%>
                                        <option value="<%=dtCategory.Rows[i]["Code"] %>" ><%=dtCategory.Rows[i]["DictName"] %></option>
                                        <%} %>
                                    </select>--%>
                                </li>
                                <%} %>
                                 <li >
                                    <i class="choosebox-i" ><%= GetGlobalResourceObject("SystemResource","ProductType").ToString() %>:</i>
                                    <select class="choosebox-text" id="ddlSmallClass" name="ddlSmallClass" ><option ><%= GetGlobalResourceObject

("SystemResource","PleaseSelect").ToString() %></option></select>
                                </li>
                                 <li >
                                    <i class="choosebox-i" ><%= GetGlobalResourceObject("Resource","ProductLine").ToString() %>:</i>
                                    <select id="ddlProductLine" name="ddlProductLine" class="choosebox-text" ></select>
                                </li>
                                <li >
                                    <i class="choosebox-i" ><%= GetGlobalResourceObject("Resource","Seller").ToString() %>:</i>
                                    <select id="ddlSeller" name="seller" class="choosebox-text" ></select>
                                </li>
                                <li >
                                    <i class="choosebox-i" ><%= GetGlobalResourceObject("Resource","STHospital").ToString() %>:</i>
                                    <select ID="ddlHosipital" name="hospital" class="choosebox-text" style="color:gray;" ></select><input type="button" id="btnddlHosipital" style="width:30px; height:25px" value="..." />
                                </li>
                                <li >
                                    <i class="choosebox-i" ><%= GetGlobalResourceObject("Resource","Doctor").ToString() %>:</i>
                                    <select ID="ddlDoctor" name="doctor"  class="choosebox-text"></select>
                                </li>
                                <li >
                                    <i class="choosebox-i" ><%= GetGlobalResourceObject("Resource","Patient").ToString() %>:</i>
                                    <input  type="text" name="patient" class="choosebox-text" >
                                </li>
                            </ul>
                        </div>
                        <div style="text-align:center; margin-top:20px;"><input type="hidden" id="submitflg" name="submitflg" value="0" /> <button type="button" 

onclick="QueryData()" class="btn-img"><span><%= GetGlobalResourceObject("Resource","Search").ToString() %></span></button></div>
                    <%--</form>	--%>
                </div>        	
            
            </li>
        </ul>
        <div id="containerHospital">
   <div id="closeIndex" style="text-align:center; position:fixed; right:30px;font-size:12px;" >关闭</div>
            <div id="letter" ></div>
    <div class="sort_box">
	    
    </div>
    <div class="initials">
	    <ul>
		    <li><img src="images/068.png"></li>
	    </ul>
    </div>

</div>
     
    </div>
   
 <style type="text/css">
     #containerHospital 
     {
         display:none;
         position:absolute;
         overflow:auto;
         background-color:white;
        z-index:9999;
        width:100%;
        top:0px;
        bottom:60px;
     }
     #letter{
    width: 100px;
    height: 100px;
    border-radius: 5px;
    font-size: 75px;
  
    text-align: center;
    line-height: 100px;
    background: rgba(145,145,145,0.6);
    position: fixed;
    left: 50%;
    top: 50%;
    margin:-50px 0px 0px -50px;
    z-index: 99;
    display: none;
}
#letter img{
    width: 50px;
    height: 50px;
    float: left;
    margin:25px 0px 0px 25px;
}
.sort_box{
    width: 100%;
    padding-top: 5px;
    overflow: hidden;
}
.sort_list{
    padding:5px 60px 5px 80px;
    position: relative;
    height: 30px;
    line-height: 20px;
    border-bottom:1px solid #ddd;
}


.sort_list .num_logo{
    width: 20px;
    height: 20px;
    border-radius: 10px;
    overflow: hidden;
    position: absolute;
    top: 5px;
    left: 20px;
}
.sort_list .num_logo img{
    width: 20px;
    height: 20px;
    border-image-repeat:stretch;
}
.sort_list .num_name{
  
}


.sort_letter{
    height: 30px;
    line-height: 30px;
    padding-left: 20px;
  
    font-size: 14px;
    border-bottom:1px solid #ddd;
}
.initials{
    position: fixed;
    top: 96px;
    bottom:160px;
    right: 0px;
   
    width: 15px;
    padding-right: 15px;
    text-align: center;
    font-size: 12px;
    z-index: 99;
    background: rgba(145,145,145,0);
}
.initials li img{
    width: 14px;
}
</style>