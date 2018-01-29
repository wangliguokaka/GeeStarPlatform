<%@ Control Language="C#" AutoEventWireup="true" CodeFile="querycontrolsm.ascx.cs" Inherits="Weixin.RGPWEB.ascx.querycontrolsm" %>
<%@ Import Namespace="System.Data"  %>

<script type="text/javascript" language="javascript">
   
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
       
        BindSmallClass();
        

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

        $("#closeIndex").click(function () {
            $("#ddlHosipital").attr("disabled", false);
            $("#containerHospital").hide();
        })

        $("#SortTable input[type=checkbox]").click(function()
        {
            if ($(this).attr("id") == "checkall")
            {
                if ($(this).is(':checked') == true)
                {
                    
                    $("#SortTable input[type=checkbox]").prop("checked", true)
                    $('#headtable tr').find('td:gt(3)').show();
                    $('#contenttable tr').find('td:gt(3)').show();
                }
                else
                {
                    $("#SortTable input[type=checkbox]").prop("checked", false)
                    $('#headtable tr').find('td:gt(3)').hide();
                    $('#contenttable tr').find('td:gt(3)').hide();
                }
            }
            else
            {
                var clickID = parseInt($(this).attr("id")) + 4;
                if ($(this).is(':checked') == true) {
                    $('#headtable tr').find('td:eq(' + clickID + ')').show();
                    $('#contenttable tr').find('td:eq(' + clickID + ')').show();
                }
                else {
                    $('#headtable tr').find('td:eq(' + clickID + ')').hide();
                    $('#contenttable tr').find('td:eq(' + clickID + ')').hide();
                }
                
            }

            var columncount = $("#headtable td:visible").length

            $("#headtable").css("width", (columncount - 2) * 45 + 80 * 2 + 100);
            $("#contenttable").css("width", (columncount - 2) * 45 + 80 * 2 + 100);
            $("#contenttable").css("width", (columncount - 2) * 45 + 80 * 2 + 100);
            $("#divcontent").css("width", (columncount - 2) * 45 + 80 * 2 + 20 + 100);
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

    function QueryData()
    {
        $("#submitflg").val("1");
        document.forms[0].submit();
    }
    </script>
<div class="screening">
       <ul>        
          <li class="Brand" style="width:100%;"><%=TranslatChinese("筛选","Filter")%></li>        
      </ul>
    </div>
    <div class="Sort-eject Sort-height" style="height:400px; ">
      
        <ul class="Sort-Sort" id="Sort-Sort">
            <li>
                <table style="width:100%" id="SortTable">
                    <tr><td colspan="3"><input checked="checked" type="checkbox" id="checkall" />全选</td></tr>
                  <%for (int i = 0; i < dtSource.Rows.Count/3; i++){ %>
                    <tr style="border-bottom:1px dotted ; font-size:10px; height:35px;">
                        <%for (int k = 0; k < 3; k++)
                            { %>
                         <td style="border-left:1px dotted ;"><%if (dtSource.Rows[i * 3 + k][0] != null && dtSource.Rows[i * 3 + k][0].ToString()!="")
                                                                      { %><input checked="checked" type="checkbox" id="<%=i * 3 + k %>" > <%=dtSource.Rows[i * 3 + k][0] %></><%} %></td>
                        <%} %>                   
                    </tr>
                    <%} %>
                </table>
            </li>
        </ul>
        <input type="hidden" id="sortList" name="sortList" value="0" />
    </div>
    <div class="Category-eject" >
        <ul class="Category-w" id="Categorytw" >
            <li onclick="Categorytw(this)">
                <div class="choose" id="dress-size">
                    <%--<form action="" method="get">--%>
                        <div class="choosebox" style="height:300px; overflow:auto;">
                            <ul class="clearfix">
                                <li>
                                    <div class="choosebox-div" ><input type="radio" name="datetype" value="0" <%if(Request["datetype"]==null  || Request["datetype"] == "0"){ %>checked="checked"<%}else{ %>""<%} %> class="size_radioToggle"><span class="value"><%= GetGlobalResourceObject("SystemResource", "inFactory").ToString() 

%></span></div>
                                    <div class="choosebox-div" ><input type="radio" name="datetype" value="1" <%if(Request["datetype"] == "1"){ %>checked="checked"<%}

else{ %>""<%} %> class="size_radioToggle" ><span class="value"><%= GetGlobalResourceObject("SystemResource", "OutFacotry").ToString() %></span></div>

     

                                    <div class="clear" ></div>
                                </li>
                                <li >
                                    <i class="choosebox-i" ><%= GetGlobalResourceObject("SystemResource", "DateFrom").ToString() %>:</i>
                                    <input type="date" name="datefrom" value="<%=Request["submitflg"]==null && String.IsNullOrEmpty(Request["datefrom"])?DateTime.Now.ToString("yyyy-MM-")+"01":Request["datefrom"] %>" class="choosebox-data" >
                                </li>
                                <li >
                                    <i class="choosebox-i" ><%= GetGlobalResourceObject("SystemResource", "DateTo").ToString() %>:</i>
                                    <input type="date" name="dateto" value="<%=Request["submitflg"]==null && String.IsNullOrEmpty(Request["dateto"])?DateTime.Now.ToString("yyyy-MM-dd"):Request["dateto"] %>" class="choosebox-data" >
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
	    <div class="sort_list">
		    <div class="num_logo">
			    <img src="images/home-a.png"  alt="">
		    </div>
		    <div class="num_name" onclick="BindSelectHospital('123','张三1')">张三1</div>
	    </div>
	    <div class="sort_list">
		    <div class="num_logo">
			    <img src="images/home-a.png" alt="">
		    </div>
		    <div class="num_name">李四</div>
	    </div>
	    <div class="sort_list">
		    <div class="num_logo">
			    <img src="images/home-a.png" alt="">
		    </div>
		    <div class="num_name">王五</div>
	    </div>
	    <div class="sort_list">
		    <div class="num_logo">
			    <img src="images/home-a.png" alt="">
		    </div>
		    <div class="num_name">刘六</div>
	    </div>
	    <div class="sort_list">
		    <div class="num_logo">
			    <img src="images/home-a.png" alt="">
		    </div>
		    <div class="num_name">马七</div>
	    </div>
	    <div class="sort_list">
		    <div class="num_logo">
			    <img src="images/home-a.png" alt="">
		    </div>
		    <div class="num_name">黄八</div>
	    </div>
	    <div class="sort_list">
		    <div class="num_logo">
			    <img src="images/home-a.png" alt="">
		    </div>
		    <div class="num_name">莫九</div>
	    </div>
	    <div class="sort_list">
		    <div class="num_logo">
			    <img src="images/home-a.png" alt="">
		    </div>
		    <div class="num_name">陈十</div>
	    </div>
	    <div class="sort_list">
		    <div class="num_logo">
			    <img src="images/home-a.png" alt="">
		    </div>
		    <div class="num_name">a九</div>
	    </div>
	    <div class="sort_list">
		    <div class="num_logo">
			    <img src="images/home-a.png" alt="">
		    </div>
		    <div class="num_name">1十</div>
	    </div>
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