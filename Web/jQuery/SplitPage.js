// JScript 文件

    var bAjax=false;
    var bAjaxLoad =true;
    var iPageNumTemp=0;
    var _strPad="";
    function action_return(){};
    var target;
    var _isCache=true;
    function $FF(obj){
    if(obj=="sPage"){
    return document.getElementById(obj);
    }else{
  return document.getElementById(_strPad+obj);
  }
}

function _$(obj){
  return (document.getElementById(_strPad+obj)?document.getElementById(_strPad+obj).value:"");
}
    function AjaxLoad(){
   
        if((!document.getElementById("hPageNum"))&&document.getElementById("ctl00_ContentPlaceHolder1_hPageNum")){_strPad="ctl00_ContentPlaceHolder1_";}
        if(_$("hPageNum")=="0") {$FF("hPageNum").value="1";}
        if(iPageNumTemp>0){$FF("hPageNum").value=iPageNumTemp;}
        document.getElementById("sPage").innerHTML = _$("hPageID") + "/" +_$("hPageNum");
    //}
    }
    window.onload=AjaxLoad;
    
    function setFirstValue(strValue){
    if(!bAjaxLoad || _$("hPageNum")>1){return;}
    if(strValue<=0){strValue=1;}
    if($FF("hPageNum")){
    $FF("hPageNum").value=strValue;
    AjaxLoad();
    } else {
    iPageNumTemp=strValue;
    }
    bAjaxLoad=false;
    }
    
    function pageClear(){
    bAjax=false;
    bAjaxLoad =true;
    iPageNumTemp=0;
    $FF("hPageNum").value=1;
    $FF("hPageID").value=1;
    if($FF("txtGo")){
    $FF("txtGo").value="";
    }
    if($FF("sPage")){
    $FF("sPage").innerHTML="1/1";
    }
    //alert(document.forms[0].action);
    //document.forms[0].action=document.forms[0].action.replace(/&sPageID=\d/g,"&sPageID=1");
     //document.forms[0].action=document.forms[0].action.replace(/&sPageID=\\(d+)/g,"&sPageID=1");
        if(document.forms[0].action.indexOf("&")>-1)
        {
            document.forms[0].action=document.forms[0].action.substring(0,document.forms[0].action.indexOf("&"))+"&sPageID=1";
        }
        else
        {
            document.forms[0].action=document.forms[0].action.replace(/&sPageID=\d/g,"&sPageID=1");
        }
    //alert(document.forms[0].action);
    }
    
    
    function setAjaxFlag(strTartget,fReturn,isCache){target=strTartget;action_return=fReturn;bAjax=true;_isCache=isCache;}
    //下一页按钮
    function btnNext_click(){
        var PageID = parseInt(_$("hPageID"));
        var PageNum=parseInt(_$("hPageNum"));
        if(PageID<PageNum)
        {   
            PageID++;
            $FF("hPageID").value = PageID;
            //document.getElementById("sPage").innerText = PageID + "/" +PageNum;
            if(PageID+1==PageNum){}
        } else if(PageID>=PageNum){return;}
        btnPage_submit();
    }
    
    //上一页按钮
    function btnPre_click(){
        var PageID = parseInt(_$("hPageID"));
        if(PageID>1)
        {   PageID--;
            $FF("hPageID").value = PageID;
            //document.getElementById("sPage").innerText = PageID + "/" +_$("hPageNum");
            if(PageID==2){}
        } else if(PageID<=1){return;}
        btnPage_submit();
    }
    
    //首页按钮
    function btnFirst_click(){
        if(parseInt(_$("hPageID")) <=1){return;}
        $FF("hPageID").value = 1;
       $FF("sPage").innerHTML = 1 + "/" +_$("hPageNum");
        btnPage_submit();
    }
    
    //尾页按钮
    function btnLast_click(){
        var sPageNum=parseInt(_$("hPageNum"));
        if(_$("hPageID") >= sPageNum){return;}
       $FF("hPageID").value = sPageNum;
       $FF("sPage").innerHTML = sPageNum + "/" +_$("hPageNum");
        btnPage_submit();
    }
    
    //GO按钮
    function btnGo_click(){
        var sPageID=_$("txtGo");
        if (/^\d+$/.test(sPageID) == false){ return; }
        if(parseInt(sPageID)>parseInt(_$("hPageNum"))) {btnLast_click();return;}
        if(parseInt(sPageID)<=0) {btnFirst_click();return;}
        
        $FF("hPageID").value = sPageID;
        $FF("sPage").innerHTML = sPageID + "/" +_$("hPageNum");
        btnPage_submit();
    }
    
    function btnPage_submit(){
        var url = window.location.search;
        var strAction = document.forms[0].action;
        if(strAction.indexOf("_btnPage")<0 ) {
            if(strAction.indexOf("?")>=0){
	           
	            if(strAction.indexOf("&sPageID")>=0 || strAction.indexOf("?sPageID")>=0)
	            {
	                //strAction = strAction.replace(/sPageID=\d+&/, "");
	                //strAction = strAction.replace(/sPageNum=\d+&/, "");
	                //strAction = strAction.replace(/sPageID=\d+/, "");
	                strAction = strAction.replace(/&?sPageNum=\d+&/, "").replace(/sPageID=\d+&?/, "");
	             strAction+="&sPageNum="+_$("hPageNum")+"&sPageID="+_$("hPageID");
	            }
	            else
	            {
	                strAction += "&sPageNum=" + _$("hPageNum") + "&sPageID=" + _$("hPageID");
	            }
		    }else{
			    strAction+="?sPageNum=" + _$("hPageNum") +"&sPageID="+_$("hPageID");
		    }
		} else {strAction = strAction.substring(0,strAction.indexOf("sPageID=")+8)+_$("hPageID")}
		//$FF("hPageFlag").value=_$("hPageID");
		if(bAjax){
		    var strGet = new Array();
		    AjaxLoad();
		    strGet=strAction.split("?");
		    var Ajax = new oAjax(strGet[0],target,action_return,_isCache);
		    Ajax.Get(strGet[1]+"&sPageNum="+_$("hPageNum"));
		} else {
		    document.forms[0].action=strAction;
            document.forms[0].submit();
        }
    
    }
    
