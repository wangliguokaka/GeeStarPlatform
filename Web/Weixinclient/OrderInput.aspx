<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Weixinclient/App_Master/all_master.Master" CodeFile="OrderInput.aspx.cs" Inherits="Weixinclient_OrderInput" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="/artDialog/jquery.artDialog.source.js?skin=green" type="text/javascript"></script>  
    <script type="text/javascript" src="http://res.wx.qq.com/open/js/jweixin-1.0.0.js"></script>
    <script type="text/javascript"  language="javascript">
        var userDoctorID = '<%=userDoctorID%>'
        var userHospitalID = '<%=userHospitalID%>'
        var userSellerID = '<%=userSellerID%>'
        $(document).ready(function () {
            selectAll = '<%= GetGlobalResourceObject("Resource","PleaseSelect").ToString() %>';
            BindSeller();
            $("#ddlSeller").change(function () {
                BindProcess();
                //var sellerid = $("#ddlSeller").value();
            });
            $("#ddlHosipital").change(function () { BindVersion(); });

            $("option").each(function () {
                $(this).attr("title", $(this).text());
            });

            AutoGenerateNo();

            BindProductFunction(0);

            $("#ddlRequireTemplate").change(function ()
            {          
                $("#Require").val($("#Require").val()+this.value);
            }
            );
            //BindSmallClass(0);
            //$("#PMLI0 #ddlSmallClass").change(function () { BindItemName(0); });

            //$("#PMLI0 .orderinput-close").bind("click",function () {
            //    var selectedValue ='';
            //    $("#order-bg,.order-mask").css("display", "none");
            //    $("#PMLI0 input[type='checkbox']:checked").each(function()
            //    {
            //        selectedValue = selectedValue+","+$(this).attr("id");
            //    })
            //    $("#PMLI0 #ProductColor").val(selectedValue.replace(",",""));
            //});

            //$("#PMLI0 .OrdersQqery-click").on("click","",function () {
            //    $("#PMLI0 #order-bg").css({
            //        display: "block", height: $(document).height()
            //    });
            //    var $box = $('#PMLI0 .order-mask');
            //    $box.css({
            //        display: "block",
            //    });
            //});

            //$("#PMLI0 .teethselect").bind("click",function () {
               
            //    $("#PMLI0 #teethorder-bg").css({
            //        display: "block", height: $(document).height()
            //    });
            //    var $box = $("#PMLI0 .teethorder-mask");
            //    $box.css({
            //        display: "block",
            //    });
            //});

            //$("#PMLI0 .teethinput-close").bind("click",function () {
            //    var selectedValue ='';
            //    $("#teethorder-bg,.teethorder-mask").css("display", "none");
            //    $("#PMLI0 input[type='checkbox']:checked").each(function()
            //    {
            //        selectedValue = selectedValue+","+$(this).attr("id");
            //    })
            //    $("#PMLI0 #ProductColor").val("123");
            //    //$("#PMLI"+bindIndex+" #ProductColor").val(selectedValue.replace(",",""));
            //});
        });

        function BindItemName(index) {
            $("#PMLI"+index+" #ddlItemName").html("");
            var strId = encodeURI($("#PMLI"+index+" #ddlSmallClass")[0].value, "utf-8");
             if (strId != -1) {
                 $.getJSON("../Handler/GetDataHandler.ashx?IsCN=<%=IsCN %>&ddlType=SmallClass&ddlId=" + strId, function (data) {
                     for (var i = 0; i < data.length; i++) {                       
                         $("#PMLI"+index+" #ddlItemName").append($("<option></option>").val(data[i].ID).html(data[i].Cname));
                     };
                 });
             }
        }

        function BindSmallClass(index)
        {
            $("#PMLI"+index+" #ddlSmallClass").html("");
            $("#PMLI"+index+" #ddlItemName").html("");
            $("#PMLI"+index+" #ddlSmallClass").append("<option value='-1' selected='selected'>" + selectAll + "</option>");
            $.getJSON("../Handler/GetDataHandler.ashx?IsCN=<%=IsCN %>&ddlType=BindSmallClass", function (data) {
                    for (var i = 0; i < data.length; i++) {                       
                        $("#PMLI"+index+" #ddlSmallClass").append($("<option></option>").val(data[i].Cname).html(data[i].Cname));
                    };
                });
             
        }

        $.ajaxSetup({ cache: false });

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

    function BindProcess() {
        var ddlhospitalID = '<%=Request["hospital"]%>';
        $("#ddlHosipital").html(""); $("#ddlDoctor").html("");
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

                    if (userHospitalID != "") {
                        $("#ddlHosipital").val(userHospitalID);
                        $("#ddlHosipital").attr("disabled", true);
                    }
                };

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
    </script>

    <script type="text/javascript">
        var x ;
        var y ;
        var localIds; 
        var serverId; 
        
        wx.config({
            debug: false, // 开启调试模式,调用的所有api的返回值会在客户端alert出来，若要查看传入的参数，可以在pc端打开，参数信息会通过log打出，仅在pc端时才会打印。
            appId:'<%=Session["APPID"]%>', // 必填，公众号的唯一标识
            timestamp:<%=timeStamp%>, // 必填，生成签名的时间戳
            nonceStr:'Wm3WZYTPz0wzccnW', // 必填，生成签名的随机串
            signature: '<%=signalticket%>', // 必填，签名，见附录1
            jsApiList:['checkJsApi','getLocation','onMenuShareTimeline','chooseImage','uploadImage','hideOptionMenu'] // 必填，需要使用的JS接口列表，所有JS接口列表见附录2
        });


        var images = {
            localId: [],
            serverId: []
        };
        function ChooseImage () {
            $("#OP").trigger("click");
            wx.chooseImage({
                success: function (res) {
                    $("#picurl").html("");
                    images.localId = res.localIds;   
              
                    for(var i = 0;i< Math.floor(res.localIds.length/3);i++)
                    {
                        $("#picurl").append("<div style=\"text-align:center;\"><img id=\"imghead\""+i*3+" src=\""+res.localIds[i*3]+"\" style=\"margin:10px 10px;width:80px; height:80px;\" /><img id=\"imghead\""+i*3+1+" src=\""+res.localIds[i*3+1]+"\" style=\"margin:10px 10px;width:80px; height:80px;\" /><img id=\"imghead\""+i*3+2+" src=\""+res.localIds[i*3+2]+"\" style=\"margin:10px 10px;width:80px; height:80px;\" /></div> ");
                    }
                    if(res.localIds.length%3==1)
                    {
                        $("#picurl").append("<div style=\"text-align:center;\"><img id=\"imghead\""+(res.localIds.length-1)+" src=\""+res.localIds[(res.localIds.length-1)]+"\" style=\"margin:10px 10px;width:80px; height:80px;\" /><img  src=\"\" style=\"margin:10px 10px;width:80px; height:80px;\" /><img  src=\"\" style=\"margin:10px 10px;width:80px; height:80px;\" /></div> ");
                    }

                    if(res.localIds.length%3==2)
                    {
                        $("#picurl").append("<div style=\"text-align:center;\"><img id=\"imghead\""+(res.localIds.length-2)+" src=\""+res.localIds[(res.localIds.length-2)]+"\" style=\"margin:10px 10px;width:80px; height:80px;\" /><img id=\"imghead\""+(res.localIds.length-1)+" src=\""+res.localIds[(res.localIds.length-1)]+"\" style=\"margin:10px 10px;width:80px; height:80px;\" /><img  src=\"\" style=\"margin:10px 10px;width:80px; height:80px;\" /></div> ");
                    }
                
                    $("img").on("click",function(){
                        PreviewImages(res.localIds);
                    })
                    //  PreviewImages(res.localIds);
                    //var img = document.getElementById('imghead');
                    //alert('已选择 ' + res.localIds.length + ' 张图片');
                    UploadImage();
                    $("#uploadimage").val(images.serverId);
                }
            });
        };

        function UploadImage() {

            if (images.localId.length == 0) {
                alert('请先使用 chooseImage 接口选择图片');
                return;
            }
            var i = 0, length = images.localId.length;
            images.serverId = [];
            function upload() {
                wx.uploadImage({
                    localId: images.localId[i],
                    success: function (res) {
                        i++;
                        //alert('已上传：' + i + '/' + length);
                        images.serverId.push(res.serverId);
                        $("#uploadimage").val(images.serverId);
                        if (i < length) {
                            upload();
                        }
                    },
                    fail: function (res) {
                        alert(JSON.stringify(res));
                    }
                });
            }
            upload();
        };

        function PreviewImages(localIds) {
            wx.previewImage({
                current: localIds[0],
                urls: localIds
            });
        };

        wx.ready(function(){        
            //ChooseImage();
           
        });

        function AutoGenerateNo(IsChecked)
        {
            $("#txtModelNo").focus();
           
            $.ajax( {
                url:"OrderInput.aspx",
                data:{action:"autoNo"},
                success:function(data)
                {
                    $("#txtModelNo").val(data);
                      
                }

            })
           
        }

        function OrderAdd() {
           
            var txtModelNo = $("#txtModelNo").val()

            if (txtModelNo == "") {
                art.dialog('<%= GetGlobalResourceObject("Resource","OrderBMNotEmpty").ToString() %>');
                return false;
            }           
           
            var ddlSeller = $("select[id*=ddlSeller]").val();
   
            if (ddlSeller == "-1") {
                art.dialog('<%= GetGlobalResourceObject("Resource","SellerNotEmpty").ToString() %>');
                return false;
            }
          
            var ddlHosipital = $("select[id*=ddlHosipital]").val(); 
            if (ddlHosipital == "-1") {
                art.dialog('<%= GetGlobalResourceObject("Resource","HospitalNotEmpty").ToString() %>');
                return false;
            }
           
            var ddlDoctor = $("select[id*=ddlDoctor]").val();
            if (ddlDoctor == "-1") {
                art.dialog('<%= GetGlobalResourceObject("Resource","DoctorNotEmpty").ToString() %>');
                return false;
            }
            var isChecked =true;
            $("[id*=ddlItemName]").each(function()
            {
                if($(this).val() == "" || $(this).val()== null)
                {
                    art.dialog('<%= GetGlobalResourceObject("Resource","PleaseSelectProduct").ToString() %>');
                    isChecked = false;
                    return false;
                }
            })
            if(isChecked == false)
            {
                return false;
            }
            $("[id*=ProductCount]").each(function()
            {
                if($(this).val() == "")
                {
                    art.dialog('<%= GetGlobalResourceObject("Resource","InputProductQuantity").ToString() %>');
                    isChecked = false;
                    return false;
                }
                else if(parseInt($(this).val()) <= 0)
                {
                    art.dialog('<%= GetGlobalResourceObject("Resource","InputProductQuantityMoreThan0").ToString() %>');
                    isChecked = false;
                    return false;
                }
            })
            if(isChecked == false)
            {
                return false;
            }

            var Require = $("#Require").val()
            if (Require.length > 200)
            {
                art.dialog('<%= GetGlobalResourceObject("Resource","RequireNotThan200").ToString() %>');
                return false;
            }
           

            var AccessoryList = "";
            $("#OAURL input").each(function()
            {
                if($(this).val()!="" &&  !isNaN($(this).val()))
                {
                    AccessoryList = AccessoryList +":"+ $(this).attr("id")+","+$(this).val()
                }
            })
           
            AccessoryList = AccessoryList.replace(":", "");
          
            $("#AccessoryList").val(AccessoryList);
          <%--  var Require = $("#Require").val()
            if (Require.length > 200)
            {
                art.dialog('<%= GetGlobalResourceObject("Resource","RequireNotThan200").ToString() %>');
                return false;
            }--%>
         
           <%-- if ($("#ProductList tr").length == 1)
            {
                art.dialog('<%= GetGlobalResourceObject("Resource","PleaseAddProduct").ToString() %>');
                return false;
            }--%>
           
            $("[id*=ddlSeller]").attr("disabled", false);
            $("[id*=ddlHosipital]").attr("disabled", false);
            $("[id*=ddlDoctor]").attr("disabled", false);
            $("#haddinfo").val("1");
           
            document.forms[0].submit();
        }
        var productIndex = 0;
        function AddProduct()
        {
            productIndex = productIndex+1;
            //防止闭包
            //replace("inactives","").replace("ul","ul style=\"display:none;\"") 此处替换为了解决闭包问题
            $("<li id=PMLI"+productIndex+">"+$("#PMLI"+(productIndex-1)).html().replace("inactives","").replace("ul","ul style=\"display:none;\"")+"</li>").insertAfter($("#PMLI"+(productIndex-1)));
            //$("<li id=PMLI"+productIndex+">"+$("#PMLI"+(productIndex-1)).html().replace("inactives","").replace("ul","ul style=\"display:none;\"")+"</li>").insertAfter($("#PMLI"+(productIndex-1)));
            BindProductFunction(productIndex)

            
        }

        function BindProductFunction(bindIndex)
        {
            //bindIndex = bindIndex.tostring
            BindSmallClass(bindIndex);
           // $(document).on("change",$("#PMLI"+bindIndex+" #ddlSmallClass"),function () { BindItemName(bindIndex); });
            $("#PMLI"+bindIndex+" #ddlSmallClass").change(function () { BindItemName(bindIndex); });

            $("#PMLI"+bindIndex+" .orderinput-close").bind("click",function () {
                var selectedValue ='';
                var hidSelectedValue ='';
                $("#order-bg,.order-mask").css("display", "none");
                $("#PMLI"+bindIndex+" input[type='checkbox']:checked").each(function()
                {
                    selectedValue = selectedValue+","+$(this).attr("id");
                    hidSelectedValue = hidSelectedValue+":"+$(this).attr("id");
                })
                
                $("#PMLI"+bindIndex+" #ProductColor").val(selectedValue.replace(",",""));
                $("#PMLI"+bindIndex+" #hidProductColor").val(hidSelectedValue.replace(":",""));
            });

            $("#PMLI"+bindIndex+" .OrdersQqery-click").bind("click",function () {
                $("#PMLI"+bindIndex+" #order-bg").css({
                    display: "block", height: $(document).height()
                });
                var $box = $("#PMLI"+bindIndex+" .order-mask");
                $box.css({
                    display: "block",
                });
            });

            $("#PMLI"+bindIndex+" .teethselect").bind("click",function () {
                $("#PMLI"+bindIndex+" #teethorder-bg").css({
                    display: "block", height: $(document).height()
                });
                var $box = $("#PMLI"+bindIndex+" .teethorder-mask");
                $box.css({
                    display: "block",
                });
            });

            $("#PMLI"+bindIndex+" .teethinput-close").bind("click",function () {
                var selectedValue ='';
                $("#teethorder-bg,.teethorder-mask").css("display", "none");
            
                SetTeethPosition(bindIndex);
                //$("#PMLI"+bindIndex+" #ProductColor").val(selectedValue.replace(",",""));
            });

            $("#PMLI"+bindIndex+" #dcommentbox td").click(function () {
                if (typeof($(this).attr("id")) != "undefined") {
                    if ($(this).hasClass("selectedPosition")) {
                        $(this).removeClass("selectedPosition");
                    }
                    else {
                        $(this).addClass("selectedPosition");
                    }
                }               
            });

            $("#PMLI"+bindIndex+" #dcommentbox td").each(function (index, element) {
                $(this).removeClass("selectedPosition");
            });
            
        }

        var positionList = "";
        function SetTeethPosition(bindIndex) {
            $("#PMLI"+bindIndex+" #righttop").val("");
            $("#PMLI"+bindIndex+" #lefttop").val("");
            $("#PMLI"+bindIndex+" #rightbottom").val("");
            $("#PMLI"+bindIndex+" #leftbottom").val("");
            var count = 0;
            $("#PMLI"+bindIndex+" #dcommentbox td").each(function (index, element) {
               
                if ($(this).hasClass("selectedPosition")) {
                    count = count+1;
                    if ($(this).attr("id").toString().indexOf("righttop") > -1) {
                        $("#PMLI"+bindIndex+" #righttop").val($("#PMLI"+bindIndex+" #righttop").val()  + $(this).html());
                    }
                    else if ($(this).attr("id").toString().indexOf("lefttop") > -1) {
                        $("#PMLI"+bindIndex+" #lefttop").val($("#PMLI"+bindIndex+" #lefttop").val()  + $(this).html());
                    }
                    else if ($(this).attr("id").toString().indexOf("rightbottom") > -1) {
                        $("#PMLI"+bindIndex+" #rightbottom").val($("#PMLI"+bindIndex+" #rightbottom").val()  + $(this).html());
                    }
                    else {
                        $("#PMLI"+bindIndex+" #leftbottom").val($("#PMLI"+bindIndex+" #leftbottom").val()  + $(this).html());
                    }
                    positionList = positionList + "," + $(this).attr("id");
                }
            })

            $("#PMLI"+bindIndex+" #ProductCount").val(count);
            //TrimDot("righttop");
            //TrimDot("lefttop");
            //TrimDot("rightbottom");
            //TrimDot("leftbottom");
            //$("#PositionList").val(positionList);
            
            //$("#productNumber").val($(".selectedPosition").length);
            //art.dialog.list['PIC0021'].close();
            //dialog.close();
        }

        function TrimFistDot(textID) {
            var stringValue = $("#" + textID).val().toString();
            if (stringValue == "") {
            } else {
                $("#" + textID).val(stringValue.substr(1, stringValue.length-1));
            }

        }

        function TrimDot(textID) {
            var stringValue = $("#" + textID).val().toString();
            stringValue = stringValue.replace(/,/gm,'');
            $("#" + textID).val(stringValue);
        }
        function RemoveProduct(thisobj)
        {
            if($("[id*=PMLI]").length == 1)
            {
                art.dialog('<%=IsCN?"订单至少需要添加一个产品":"One Product is necessary"%>');
                return false;
            }

            art.dialog({
                async:false,
                title: false,
                icon: 'question',
                content: '<%= GetGlobalResourceObject("Resource","ConfirmDeleteProduct").ToString() %>',
                ok: function () {
                    var productLi = $(thisobj).parent().parent().parent().parent().parent();                    
                    productLi.remove();
                    $("[id*=PMLI]").each(function(){
                        productIndex = parseInt($(this).attr("id").replace("PMLI",""));                        
                    })
                },
                cancelVal: '<%= GetGlobalResourceObject("Resource","Cancel").ToString() %>',
                cancel: true
            });  
            return false;
        }

    </script>
<section >
     <input type="hidden" id="productIndex" name="productIndex" />
    <input type="hidden" id="uploadimage" name="uploadimage" />
    <input id="haddinfo" name="haddinfo" type="hidden" />
    <input id="AccessoryList" name="AccessoryList" type="hidden" />
    <div style="height:40px; margin-top:5px; "><button type="button" onclick="OrderAdd()" class="button" ><span><%= GetGlobalResourceObject("Resource","SaveOrder").ToString() %></span></button>
        <button type="button" class="button" onclick="AddProduct()"><span><%= GetGlobalResourceObject("Resource","AddProduct").ToString() %></span></button>
        <button type="button" class="button" onclick="ChooseImage()" ><span><%= GetGlobalResourceObject("Resource","UploadPicture").ToString() %></span></button>
    </div>
    <div class="OrdersQqery">
		<ul >
			<li id="BILI"><span id="BI" class="inactive inactives"><a ><%= GetGlobalResourceObject("SystemResource","BI").ToString() %></a></span>
				<ul>
                    <div style="height:260px;overflow-y:auto;">
					    <li ><i class="OrdersQqery-i" ><%= GetGlobalResourceObject("SystemResource","AssistCode").ToString() %>：</i><input type="text" id="txtModelNo" name="txtModelNo" readonly="readonly" class="choosebox-text" ></li> 
                        <li ><i class="OrdersQqery-i" ><%= GetGlobalResourceObject("Resource","OrderCategory").ToString() %>：</i> <select id="ddlOrderType" datatextfield="DictName" datavaluefield="Code" runat="server" name="ddlOrderType" class="OrdersQqery-select"  ></select></li> 
					    <li ><i class="OrdersQqery-i" ><%= GetGlobalResourceObject("SystemResource","SalesMan").ToString() %>：</i><select id="ddlSeller" name="seller" class="OrdersQqery-select" ></select></li>
                        <li ><i class="OrdersQqery-i" ><%= GetGlobalResourceObject("Resource","STHospital").ToString() %>：</i><select ID="ddlHosipital" name="hospital" class="OrdersQqery-select" ></select></li>
                        <li ><i class="OrdersQqery-i" ><%= GetGlobalResourceObject("SystemResource","Doctor").ToString() %>：</i><select ID="ddlDoctor" name="doctor" class="OrdersQqery-select" ></select></li>
                        <li ><i class="OrdersQqery-i" ><%= GetGlobalResourceObject("SystemResource","Patient").ToString() %>：</i><input type="text" class="choosebox-text" name="txtpatient" placeholder="<%= GetGlobalResourceObject("SystemResource","PIPatientName").ToString() %>" ></li>
                        <li ><i class="OrdersQqery-i" ><%= GetGlobalResourceObject("SystemResource","Age").ToString() %>：</i><input type="number" class="choosebox-text" name="txtAge" placeholder="<%= GetGlobalResourceObject("SystemResource","PLAge").ToString() %>" ></li>
                        <li ><i class="OrdersQqery-i" ><%= GetGlobalResourceObject("SystemResource","Sex").ToString() %>：</i>
                          <div class="choosebox-div" ><input type="radio" name="ddlSex" value="1" class="size_radioToggle" checked ><i class="value"><%= GetGlobalResourceObject("SystemResource","M").ToString() %></i></div>
                          <div class="choosebox-div" ><input type="radio" name="ddlSex" value="0" class="size_radioToggle" ><i class="value"><%= GetGlobalResourceObject("SystemResource","F").ToString() %></i></div>
                          <div class="clear" ></div>
                        </li>
                        <li ><i class="OrdersQqery-i" ><%= GetGlobalResourceObject("SystemResource","Single").ToString() %>：</i><select class="OrdersQqery-select" id="ddlSingle" name="ddlSingle" >
                        <option value="Y" selected="selected" ><%= GetGlobalResourceObject("Resource","Danzuo").ToString() %></option>
                        <option value="N"><%= GetGlobalResourceObject("Resource","Lianzuo").ToString() %></option>                    
                        </select></li>
                        <li ><i class="OrdersQqery-i" ><%= GetGlobalResourceObject("SystemResource","Separate").ToString() %>：</i><select class="OrdersQqery-select" id="ddlDivision" name="ddlDivision" >
                        <option value="Y" selected="selected" ><%= GetGlobalResourceObject("Resource","DMSplit").ToString() %></option>
                        <option value="N"><%= GetGlobalResourceObject("Resource","NotSplit").ToString() %></option>                    
                        </select></li> 
                    </div>
				</ul>
			</li>
			<li id="PMLI0"><span id="PM"  class="inactive"><a><%= GetGlobalResourceObject("SystemResource","ProductMessage").ToString() %></a></span>
				<ul style="display: none">
                    <div style="height:280px;overflow-y:auto;">
                        <li><i class="OrdersQqery-i" style="width:180px;"><button type="button" onclick="RemoveProduct(this)" class="button" ><%=IsCN?"删除产品":"Delete" %></button></i></li>
					    <li ><i class="OrdersQqery-i" ><%= GetGlobalResourceObject("SystemResource","ProductType").ToString() %>：</i><select class="OrdersQqery-select" id="ddlSmallClass" name="ddlSmallClass" ><option ><%= GetGlobalResourceObject("SystemResource","PleaseSelect").ToString() %></option></select></li> 
					    <li ><i class="OrdersQqery-i" ><%= GetGlobalResourceObject("SystemResource","ProductName").ToString() %>：</i><select class="OrdersQqery-select" id="ddlItemName" name="ddlItemName"><option ><%= GetGlobalResourceObject("SystemResource","PleaseSelect").ToString() %></option></select></li>
                        <li style="height:80px; line-height:80px;"><i class="OrdersQqery-i" ><%= GetGlobalResourceObject("SystemResource","ToothPosition").ToString() %>：</i>
                            <div >
                                <table cellpadding="0" style="width:50%; text-align:center;" border="0" >
                                    <tr>
                                         <td rowspan="2"><%= GetGlobalResourceObject("Resource","Right").ToString() %></td>
                                            <td style="border-bottom:1px solid black ;border-right:1px solid black ; text-align:center;" ><input type="text" id="righttop" class="teethselect" name="righttop" style="text-align:right; margin-left:2%;width:96%;" value="" readonly="readonly" /></td>
                                            <td style="border-bottom:1px solid black;" ><input type="text" id="lefttop" value="" name="lefttop" class="teethselect" readonly="readonly" style="width:96%;margin-left:1%;" /></td>
                                            <td rowspan="2"><%= GetGlobalResourceObject("Resource","Left").ToString() %></td>
                                    </tr>
                                    <tr>
                                        <td style="border-right:1px solid black;" ><input type="text" value="" class=" teethselect" id="rightbottom" readonly="readonly" style="text-align:right;margin-left:2%;width:96%;" name="rightbottom" /></td>
                                        <td  ><input type="text" id="leftbottom"  name="leftbottom" value="" class="teethselect" readonly="readonly" style="width:96%;margin-left:1%;" /></td>
                                    </tr>
                                </table>
                            </div>
                        </li> 
					    <li ><i class="OrdersQqery-i" ><%= GetGlobalResourceObject("SystemResource","Count").ToString() %>：</i><input type="number" id="ProductCount" name="ProductCount" class="choosebox-text" placeholder="<%= GetGlobalResourceObject("SystemResource","PleaseSelectCount").ToString() %>" ></li>
                        <li ><i class="OrdersQqery-i" ><%= GetGlobalResourceObject("SystemResource","Color").ToString() %>：</i><input type="hidden" name="hidProductColor" id="hidProductColor" /> <input type="text" id="ProductColor" name="ProductColor" readonly="readonly" class="choosebox-text OrdersQqery-click" placeholder="<%= GetGlobalResourceObject("SystemResource","PleaseSelectColor").ToString() %>" ></li>
                    
                        <div id="order-bg">&nbsp;</div>
                        <div class="order-mask">
                           <div class="orderinput-close" >&nbsp;</div>
                            <div><%= GetGlobalResourceObject("SystemResource","PleaseSelectColor").ToString() %></div>
                            <div style="text-align:center; margin-left:15px; width:90%;">
                                <table style="text-align:left; width:100%;">
                                 <%if (DTColors != null)
                                    {
                                    for (int i = 0; i < DTColors.Rows.Count / 5; i++)
                                    { %>
                                    <tr>
                                        <td><input type="checkbox"  id="<%= DTColors.Rows[i * 5]["ColorType"]%>" /><%= DTColors.Rows[i * 5]["ColorType"]%></td>
                                        <td><input type="checkbox" id="<%= DTColors.Rows[i * 5 + 1]["ColorType"]%>" /><%= DTColors.Rows[i * 5 + 1]["ColorType"]%></td>
                                        <td><input type="checkbox" id="<%= DTColors.Rows[i * 5 + 2]["ColorType"]%>" /><%= DTColors.Rows[i * 5 + 2]["ColorType"]%></td>
                                        <td><input type="checkbox" id="<%= DTColors.Rows[i * 5 + 3]["ColorType"]%>" /><%= DTColors.Rows[i * 5 + 3]["ColorType"]%></td>
                                        <td><input type="checkbox" id="<%= DTColors.Rows[i * 5 + 4]["ColorType"]%>" /><%= DTColors.Rows[i * 5 + 4]["ColorType"]%></td>
                                    </tr>
                                <%}
                                }%>
                                <tr>
                                <%if (DTColors != null)
                                        {
                                            for (int i = (DTColors.Rows.Count - DTColors.Rows.Count % 5); i < DTColors.Rows.Count; i++)
                                            {%>
                                   <td><input type="checkbox" id="<%= DTColors.Rows[i]["ColorType"]%>" /><%= DTColors.Rows[i]["ColorType"]%></td>
                                <%}
                                        } %>
                                    </tr>
                                </table>
                            </div>                        
                        </div>
                        <div id="teethorder-bg">&nbsp;</div>
                        <div class="teethorder-mask">
                            <div class="teethinput-close" >&nbsp;</div>
                            <div><%= GetGlobalResourceObject("SystemResource","PleaseSelectToothPosition").ToString() %></div>
                            <div style="text-align:center; margin-left:5px;margin-top:20px; width:90%;">
                                <table id="dcommentbox" style="margin:auto;border-collapse: separate" class="tablestyle">
                                    <tr><td style="font-size:10px; border-style:none;"><%=IsCN?"上右":"RightTop" %></td><%for (int i = 8; i >= 1;i-- ) {%><td  id="righttop<%=i %>" ><%=i %></td><%} %></tr>
                                    <tr><td style="font-size:10px;border-style:none;"><%=IsCN?"上左":"LeftTop" %></td><%for (int i = 1; i <=8;i++ ) {%><td id="lefttop<%=i %>"><%=i %></td><%} %></tr>
                                    <tr><td style="font-size:10px;border-style:none;"><%=IsCN?"下右":"RightBottom" %></td><%for (int i = 8; i >= 1;i-- ) {%><td id="rightbottom<%=i %>"><%=i %></td><%} %></tr>
                                    <tr><td style="font-size:10px;border-style:none;"><%=IsCN?"下左":"LeftBottom" %></td><%for (int i = 1; i <=8;i++ ) {%><td id="leftbottom<%=i %>"><%=i %></td><%} %></tr>
                                </table>
                            </div>
                        </div>
                    </div>
				</ul>
			</li>
			<li id="OALI"><span id="OA"  class="inactive"><a ><%= GetGlobalResourceObject("SystemResource", "OrderAccessories").ToString()%></a></span>
				<ul id="OAURL" style="display: none">
                    <%for (int i = 0; i < DTAccessory.Rows.Count/2;i++ )
                      { %>
                        <li ><i class="OrdersQqery-i" style="width:100px;"><%= DTAccessory.Rows[i]["Accessory"]%>：</i><input type="number" id="<%=  DTAccessory.Rows[i]["Code"]%>" style="width:50px;"  class="choosebox-text" placeholder="" >
                            <i class="OrdersQqery-i" style="width:100px;"><%= DTAccessory.Rows[i+1]["Accessory"]%>：</i><input type="number" id="<%=  DTAccessory.Rows[i+1]["Code"]%>" style="width:50px;" class="choosebox-text" placeholder="" >
                        </li> 
                    <%} %>
                    <% if (DTAccessory.Rows.Count%2 == 1){%>
                        <li ><i class="OrdersQqery-i" style="width:100px;"><%= DTAccessory.Rows[DTAccessory.Rows.Count-1]["Accessory"]%>：</i><input type="number" id="<%=  DTAccessory.Rows[DTAccessory.Rows.Count-1]["Code"]%>" style="width:50px;"  class="choosebox-text" placeholder="" >
                            
                        </li> 
                    <%} %>
					<%--<li ><i class="OrdersQqery-i" ><%= GetGlobalResourceObject("SystemResource","Picture").ToString() %>：</i><input type="text" class="choosebox-text" placeholder="<%= GetGlobalResourceObject("SystemResource","PleaseSelectPicture").ToString() %>请选择照片" ></li> 
					<li ><i class="OrdersQqery-i" ><%= GetGlobalResourceObject("SystemResource","ToothShade").ToString() %>：</i><input type="text" class="choosebox-text" placeholder="<%= GetGlobalResourceObject("SystemResource","PleaseSelectToothShade").ToString() %>请选择比色牙" ></li>
                    <li ><i class="OrdersQqery-i" ><%= GetGlobalResourceObject("SystemResource","StudyModel").ToString() %>：</i><input type="text" class="choosebox-text" placeholder="<%= GetGlobalResourceObject("SystemResource","PleaseSelectStudyModel").ToString() %>请选择参考模" ></li> --%>
				</ul>
			</li>
            <li id="OPLI"><span id="OP"  class="inactive"><a ><%= GetGlobalResourceObject("SystemResource","Picture").ToString() %></a></span>
				<ul id="picurl" style="display: none">
					
				</ul>
			</li>
            <li id="RQLI"><span id="RQ"  class="inactive"><a ><%= GetGlobalResourceObject("Resource","DoctorRequire").ToString() %></a></span>
                <ul id="RQURL" style="display: none">
				    <li><i class="OrdersQqery-i" style="width:100px;"><%=IsCN?"选择模板":"Template" %>：</i><select class="OrdersQqery-select" id="ddlRequireTemplate"  ><option>&nbsp;</option><%for (int i = 0; i < dtRequireTemplate.Rows.Count;i++ ) {%><option><%=dtRequireTemplate.Rows[i]["dictname"] %></option><%} %></select></li>
                    <li style="height:80px;"><i class="OrdersQqery-i" style="width:10px;">&nbsp;</i><textarea id="Require" name="Require" cols="30" rows="4" ><%=ordreModel.Require%></textarea></li>
                </ul>
			</li>
		</ul>
	</div>
  </section>
  <!--section   end-->
</asp:Content>