<%@ Control Language="C#" AutoEventWireup="true" CodeFile="querycontrolSC.ascx.cs" Inherits="Weixin.RGPWEB.ascx.querycontrolSC" %>

<script src="/jQuery/SplitPage.js" type="text/javascript"></script>
<script type="text/javascript" language="javascript">
    var userDoctorID = '<%=userDoctorID%>'
    var userHospitalID = '<%=userHospitalID%>'
    var userSellerID = '<%=userSellerID%>'
    $(document).ready(function () {

        if ('<%=Request["submitflg"]%>' == "0" || '<%=Request["submitflg"]%>' == '')
        {
	    }

    });

    $.ajaxSetup({ cache: false });

 

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

    <div class="Category-eject">
        <ul class="Category-w" id="Categorytw">
            <li onclick="Categorytw(this)">
                <div class="choose" id="dress-size">
                    <%--<form action="" method="get">--%>
                        <div class="choosebox" style="height:100px; overflow:hidden; margin-top:10px;">
                            <ul class="clearfix">
                                
                                <li >
                                    <i class="choosebox-i" ><%= GetGlobalResourceObject("SystemResource", "DateFrom").ToString() %>:</i>
                                    <input type="date" name="datefrom" id="datefrom" value="<%=Request["submitflg"]==null && String.IsNullOrEmpty(Request["datefrom"])?DateTime.Now.ToString("yyyy-MM-dd"):Request["datefrom"] %>" class="choosebox-data" >
                                </li>
                                <li >
                                    <i class="choosebox-i" ><%= GetGlobalResourceObject("SystemResource", "DateTo").ToString() %>:</i>
                                    <input type="date" name="dateto" id="dateto" value="<%=Request["dateto"] %>" class="choosebox-data" >
                                </li>
                            </ul>
                        </div>
                        <div style="text-align:center; margin-top:20px;"><input type="hidden" id="submitflg" name="submitflg" value="0" /> <button type="button" 

onclick="QueryData()" class="btn-img"><span><%= GetGlobalResourceObject("Resource","Search").ToString() %></span></button></div>
                    <%--</form>	--%>
                </div>        	
            
            </li>
        </ul>
    </div>
