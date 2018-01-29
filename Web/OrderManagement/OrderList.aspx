<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OrderList.aspx.cs" 
    MasterPageFile="~/OrderManagement/OrderManagement.master" Inherits="OrderManagement_OrderList" %>

<%@ Register Src="~/ascx/pagecut.ascx" TagName="pagecut" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style type="text/css" media="screen">

        body {margin:0;padding:0;}
        
        html, body,form  { 
            margin: 0px 0px; 
            width: 100%; 
            height: 100%;
         }
       .scrollFrame {
           overflow:auto !important;
       }

        .right {margin-left:260px;}
        .left {position:relative;float:left; }

    </style>
    <input id="hpnumbers" name="hpnumbers" type="hidden" value="<%=hddpnumbers %>" />
    
    <script type="text/javascript" src="../zTree_v3/js/jquery.ztree.all-3.5.js"></script>
    <script type="text/javascript" src="js/Json.js"></script>
    <script type="text/javascript">      

        $(function () {
            var zNodes = '<%=zNodes%>'
            var zNodesJson = $.parseJSON(zNodes);   
            $.fn.zTree.init($("#treeDemo"), setting, zNodesJson);
            $("body").css("width", "100%");
            $("body").css("height", "100%");
        })

        var setting = {
            data: {
                simpleData: {
                    enable: true
                }
            }
            ,
            callback: {
                onClick: onClick
            },
            view:
            {
                selectedMulti: false
            }
        };

        function onClick(event, treeId, treeNode, clickFlag) {
            if (clickFlag == "0") {
               
                $("#selectedID").val("");
                $("#selectedLevel").val("");
            }
            else {
                $("#selectedID").val(treeNode.id);
                $("#selectedLevel").val(treeNode.level);
            }
        }
    </script>
    <input id="hDelete" name="hDelete" type="hidden" />
    <input id="userName" name="userName"  type="hidden" value="" />
    <input id="selectedID" type="hidden" name="selectedID" value="" />
    <input id="selectedLevel" type="hidden" name="selectedLevel" value="" />
        
    <div style="width:100%;height:100%;">
         <div class="zTreeDemoBackground left" style="height:90%; ">
		    <ul id="treeDemo" class="ztree" style="height:100%;"></ul>
	    </div>
        <div class="right" style="height:100%;">
                
                <iframe src="OrderListQuery.aspx" id="OrderListQuery" name="OrderListQuery"  frameborder="0" class="scrollFrame"  scrolling="yes" style=" overflow-y:auto !important; width:100%; height:100%;"></iframe>
           
        </div>
    </div>
   <script type="text/javascript" language="javascript">   
    function iFrameHeight() {   
        var ifm = document.getElementById("OrderListQuery");

        var subWeb = document.frames ? document.frames["OrderListQuery"].document : ifm.contentDocument;
        if (ifm != null && subWeb != null) {
            ifm.height = subWeb.body.scrollHeight;
            ifm.width = subWeb.body.scrollWidth;
        }
        if (document.frames) {
            ifm.style.removeAttribute("height");
        } else {
            ifm.height = "450";
            ifm.style.removeAttribute("height");
            alert(ifm.height)
        }
       
    }

    function SetCwinHeight() {
        var iframeid = document.getElementById("OrderListQuery"); //iframe id 
        iframeid.height = "10px";//先给一个够小的初值,然后再长高. 
        if (document.getElementById) {
            if (iframeid && !window.opera) {
                if (iframeid.contentDocument && iframeid.contentDocument.body.offsetHeight) {
                    iframeid.height = iframeid.contentDocument.body.offsetHeight;
                } else if (iframeid.Document && iframeid.Document.body.scrollHeight) {
                    iframeid.height = iframeid.Document.body.scrollHeight;
                }
            }
        }
    }

    var Sys = {};  
        var ua =navigator.userAgent.toLowerCase();  
        var s;  
        (s = ua.match(/msie([\d.]+)/)) ? Sys.ie = s[1] :  
        (s =ua.match(/firefox\/([\d.]+)/)) ? Sys.firefox = s[1] :  
        (s =ua.match(/chrome\/([\d.]+)/)) ? Sys.chrome = s[1] :  
        (s =ua.match(/opera.([\d.]+)/)) ? Sys.opera = s[1] :  
        (s =ua.match(/version\/([\d.]+).*safari/)) ? Sys.safari = s[1] :0;  
       
    if (Sys.opera || Sys.safari)  
        {  
        window.setInterval("reinitIframe()", 200);  
        }  
    function reinitIframe() //针对opera safari  
        {  
        var iframe = document.getElementByIdx_x_x_x("OrderListQuery");
            try{  
                var bHeight =iframe.contentWindow.document.body.scrollHeight;  
                var dHeight =iframe.contentWindow.document.documentElement.scrollHeight;  
                var height = Math.max(bHeight, dHeight);  
                iframe.height =  height;  
                }catch (ex){}  
            }  
           
        function iframeAutoFit(iframeObj)  
            {   
                setTimeout(function()  
                    {  
                        if(!iframeObj)   
                            return;  
                        iframeObj.height=(iframeObj.Document?iframeObj.Document.body.scrollHeight:iframeObj.contentDocument.body.offsetHeight)+30;//这里+30是有目的的，比如ie下会少那么一些像素  
                        },200)  
                    }  

</script>

</asp:Content>
