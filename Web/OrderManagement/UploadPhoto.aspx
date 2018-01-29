<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UploadPhoto.aspx.cs" Inherits="OrderManagement_UploadPhoto" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
  
    <script type="text/javascript" src="../jQuery/jquery.form.js"></script>

    <script src="../jQuery/Jcrop/js/jquery.Jcrop.js" type="text/javascript"></script>

    <link href="images/main.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        (function ($) {
            $.fn.extend({
                gallery: function () {
                    $(this).on("click", function () {
                        
                        var self = $(this),
                        link = self.parent().find("a");
                        
                        if (initedGallery.indexOf(self.find("img").attr("src")) == -1) {

                            initedGallery.push(self.find("img").attr("src"));
                        }

                            var bd = $("body");
                            html = "\
                            <div class='gallery-overlay'>\
                                <div class='gallery-photo'><span></span><img src='" + self.attr("href") + "'></div>\
                                <div class='gallery-thumb'></div>\
                                <a class='gallery-prev' href='javascript:void(0);' >&lt;</a>\
                                <a class='gallery-next' href='javascript:void(0);' >&gt;</a>\
                                <a class='gallery-close' href='javascript:void(0);'>&times;</a>\
                            </div>\
                        ";

                            bd.css("overflow-y", "hidden").append(html);
                            $(".gallery-thumb").html("");

                            var overlay = $(".gallery-overlay"),

                            photo = $(".gallery-photo"),
                            photoImg = photo.find("img"),

                            thumb = $(".gallery-thumb"),
                            prev = $(".gallery-prev"),
                            next = $(".gallery-next"),
                            close = $(".gallery-close"),
                            str = "";
                            //浏览器缩放时候，重置
                            function toResize() {
                                var height = $(window).height();
                                overlay.height(height);
                                photo.css({ "height": height - 200 });
                                photoImg.css({ "max-height": height - 200 }); //解决safari下bug
                            }
                            toResize();
                            $(window).resize(function () {
                                toResize();
                            });

                            //生成缩略图列表
                            var str = ''
                            link.each(function () {
                                var href = $(this).attr("href"),
                                src = $(this).find("img").attr("src"),
                                act = "<a href='" + href + "'><img src='" + src + "'/></a>";
                                str += act;
                            });
                            thumb.append(str);

                            //图片切换
                            var thumbLink = thumb.find("a"),
                            len = thumbLink.length - 1,
                            index = link.index(this);
                            function switchPhoto(index) {
                                var _this = thumbLink.eq(index);
                                _this.addClass("selected").siblings().removeClass("selected");
                                photo.find("img").attr("src", _this.attr("href"));
                            }
                            switchPhoto(index);

                            thumb.on("click", "a", function () {
                                index = thumbLink.index(this);
                                switchPhoto(index);
                                return false;
                            });

                            //切换下一个
                            function switchPrev() {
                                index--;
                                if (index < 0) {
                                    index = len;
                                }
                                switchPhoto(index);
                            }
                            //切换上一个
                            function switchNext() {
                                index++;
                                if (index > len) {
                                    index = 0;
                                }
                                switchPhoto(index);
                            }

                            prev.on("click", function () {
                                switchPrev();
                            });
                            next.on("click", function () {
                                switchNext();
                            });
                            photo.on("click", "img", function () {
                                switchNext();
                            });

                            //关闭层
                            function closeOverlay() {
                                overlay.remove();
                                bd.css("overflow-y", "auto");
                            }
                            close.on("click", function () {
                                closeOverlay();
                            });

                            return false;
                       // }
                    });

                }
            });
        })(jQuery);

        function appendPic(data) {
            if (srcGallery.length == 9) {
                alertdialog('<%= GetGlobalResourceObject("Resource","PictureLimit").ToString() %>');
            } else {
                $(".img").hide();
                if (data != "init") {
                    srcGallery.push(data);
                }

                $(".img").html("");
                for (var i = 0 ; i < srcGallery.length; i++) {
                    if (i % 3 == 0) {
                        $(".img").append("<br/><br/>");
                    }
                    var imgsrc = srcGallery[i];
                    $(".img").append("<img height=\"20px\" width=\"20px\" onclick=\"RemovePic('" + imgsrc + "')\" src=\"images/delete.png\"><a style=\"margin-left:20px;margin-top:20px;\" href='" + imgsrc + "'><img height='100' width='100' src='" + imgsrc + "' alt=\"\"></a>");
                }
                $('.img a').gallery();
                $(".img").show();
            }

        }

        var listPosition = '';
        var srcGallery = new Array();
        var initedGallery = new Array();
        $(document).ready(function () {
            var photoList = '<%=Request["photoList"]%>';
           // var photoList = 'http://localhost:58826/uploadedFiles/20150802211904.jpg,http://localhost:58826/uploadedFiles/20150802211909.jpg';
            if (photoList != "") {
                srcGallery = photoList.split(",");
                appendPic("init");
            }
            $('.img a').gallery();
            $(".img").show();
        });

        var positionList = "";
        function setMember() {

            if (srcGallery.length > 0) {
                $("#photoList").val(srcGallery.toString());
            }
            
            art.dialog.list['PIC0016'].close();
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

        function uploadAttachment() {
            
            //jcrop_api.release();
            var fileNumber = Date.parse(new Date());
            fileNumber = fileNumber + Math.floor(Math.random() * (10000 + 1));
            dialog = art.dialog({
                id: 'PIC004',
                title: "<%= GetGlobalResourceObject("Resource","UploadPicture").ToString() %>",
                lock: true,
                background: '#000',
                opacity: 0.3

            });

            $.ajax({
                url: "AttachmentFile.aspx?fileid=" + fileNumber,
                success: function (data) {
                   
                    dialog.content(data);
                     
                },
                cache: false
            });
        }



        function onCropClick() {
            if ($("#picfile").val() == "")
            {
                alertdialog('<%= GetGlobalResourceObject("Resource","InputPictureAlert").ToString() %>');
                return false;
            }
        $.ajax({
                type: "POST",

                data: {action:'savepic',picfile:$("#picfile").val(), pPartStartPointX: $('#x').val(), pPartStartPointY: $('#y').val(), pPartWidth:  $('#w').val(), pPartHeight: $('#h').val()},
                url: "UploadPhoto.aspx",
                dataType: "text",
                success: function (data) {
                    jcrop_api.release();
                    //$("#previewpic").attr("src", data);
                    //                    alert(data.d);
                    appendPic(data)
                    $('#x').val("");
                    $('#y').val("");
                    $('#w').val("");
                    $('#h').val("");
                }
            });
        }

        function showCoords(c) {
            jQuery('#x').val(c.x);
            jQuery('#y').val(c.y);
            jQuery('#w').val(c.w);
            jQuery('#h').val(c.h);
        };

        Array.prototype.clear = function () {
            this.length = 0;
        }
        Array.prototype.insertAt = function (index, obj) {
            this.splice(index, 0, obj);
        }
        Array.prototype.removeAt = function (index) {
            this.splice(index, 1);
        }


        Array.prototype.remove = function (obj) {
            var index = this.indexOf(obj);
            if (index >= 0) {
                this.removeAt(index);
            }
        }




        function RemovePic(data) {
           
            $(".img").hide();
            srcGallery.remove(data);

            $(".img").html("");
            for (var i = 0 ; i < srcGallery.length; i++) {
                if (i % 3 == 0) {
                    $(".img").append("<br/><br/>");
                }
                var imgsrc = srcGallery[i];
                $(".img").append("<img height=\"20px\" width=\"20px\" onclick=\"RemovePic('" + imgsrc + "')\" src=\"images/delete.png\"><a style=\"margin-left:20px;margin-top:20px;\" href='" + imgsrc + "'><img height='100' width='100' src='" + imgsrc + "' alt=\"\"></a>");
            }
            $('.img a').gallery();
            $(".img").show();
        }


       

        $(function () {
         
        });
    </script>

    <style type="text/css">
        #dcommentbox td
        {
             width:30px;
        }
        
        .selectedPosition
        {
           background-color:#f9f3da;    
        }
       
        .expertnumber
        {
	        margin: 10px 0px 5px 0px;
	        height: 40px;
	        font-size: 16px;
	        font-family: 微软雅黑;
	        color: #555;
	        background-color: #f1f7e6;
             width:950px;
	        line-height: 39px;
	        padding-left: 15px;
	        margin-top: 5px;
        }

         /*
    * jquery gallery CSS
    * ZhaoHuanLei - 20140418
    */
    .gallery-overlay {width:100%;height:100%;position:fixed;_top:absolute;top:0;left:0;z-index:9999999999;filter:progid:DXImageTransform.Microsoft.gradient(enabled='true',startColorstr='#B2000000', endColorstr='#B2000000');background-color:rgba(0,0,0,.7);}
    :root .gallery-overlay {filter:none;}
    .gallery-close,
    .gallery-prev,
    .gallery-next {position:absolute;color:#fff;text-decoration:none;}
    .gallery-prev,
    .gallery-next {top:40%;font:bold 80px/80px simsun;}
    .gallery-prev {left:50px;}
    .gallery-next {right:50px;}
    .gallery-close {width:82px;height:77px;top:0;right:0;background:url(http://images.cnitblog.com/i/333689/201404/181538254946336.png) no-repeat;text-indent:-9999em;}
    .gallery-photo {width:100%;height:100%;position:absolute;top:50px;vertical-align:middle;text-align:center;}
    .gallery-photo span {height:100%;display:inline-block;vertical-align:middle;}
    .gallery-photo img {max-width:100%;max-height:100%;vertical-align:middle;cursor:pointer;}
    .gallery-thumb {width:100%;height:56px;position:absolute;bottom:50px;text-align:center;font-size:0;}
    .gallery-thumb a {width:50px;height:50px;overflow:hidden;margin:0 2px;display:inline-block;*zoom:1;border:3px solid transparent;opacity:.6;filter:alpha(opacity:60);}
    .gallery-thumb img {max-width:100px;max-height:100px;min-width:50px;min-height:50px;border:none;}
    .gallery-thumb .selected {border-color:#f60;opacity:1;filter:alpha(opacity:100);}
    </style>
</head>
<body>
   
    <form id="form1" runat="server">
    <input id="hsysuserid" name="hsysuserid" type="hidden" />
    <div style="width:100%;">
         <div class="expertnumber"><%= GetGlobalResourceObject("Resource","UploadPicture").ToString() %></div>
        <div style="width:400px; height:400px;margin-left:20px; float:left; border-style:solid;">
            <img id="viewpic" alt="" />
        </div>
        <div style="margin-left:40px; float:left;" id="preview">
            
        </div>
        <div class="img" style="display:none; z-index:999;">
            
        </div>
        <div style="clear:both; margin-top:20px;margin-left:40px;">
            <a class="btn01" href="javascript:void(0)"  onclick="uploadAttachment()"><%= GetGlobalResourceObject("Resource","UploadPicture").ToString() %></a>
            <a class="btn01" href="javascript:void(0)" onclick="onCropClick()"><%= GetGlobalResourceObject("Resource","SavePicture").ToString() %></a>
            <input type="hidden" id="picfile" name="picfile" value="" />
            <a href="javascript:void(0)" onclick="setMember();" class="btn01"><%= GetGlobalResourceObject("Resource","Return").ToString() %></a>
            <a href="javascript:void(0)" onclick="closeme()" class="btn01"><%= GetGlobalResourceObject("Resource","Cancel").ToString() %></a>
        </div>
        <input type="hidden"  id="x" name="x" />
        <input type="hidden"  id="y" name="y" />
        <input type="hidden"  id="w" name="w" />
        <input type="hidden"  id="h" name="h" />
     </div>
    </form>
    <script type="text/javascript" language="javascript">
      
</script>
</body>
</html>
