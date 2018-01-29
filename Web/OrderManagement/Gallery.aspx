<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Gallery.aspx.cs" Inherits="OrderManagement_Gallery" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>画廊</title>
    <style>
    /*
    * jquery gallery CSS
    * ZhaoHuanLei - 20140418
    */
    .gallery-overlay {width:100%;height:100%;position:fixed;_top:absolute;top:0;left:0;z-index:99;filter:progid:DXImageTransform.Microsoft.gradient(enabled='true',startColorstr='#B2000000', endColorstr='#B2000000');background-color:rgba(0,0,0,.7);}
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
<body style="height:2000px;">


<h1>画廊</h1>
<div class="img" style="display:none;">
    <div><a href="http://images.cnitblog.com/i/333689/201403/181012241467455.jpg"><img src="http://images.cnitblog.com/i/333689/201403/181012064744754.jpg" alt=""></a></div>
    <div><a href="http://images.cnitblog.com/i/333689/201403/181012428021756.jpg"><img src="http://images.cnitblog.com/i/333689/201403/181012349904375.jpg" alt=""></a></div>
    <div><a href="http://images.cnitblog.com/i/333689/201403/181012573656772.jpg"><img src="http://images.cnitblog.com/i/333689/201403/181012512096320.jpg" alt=""></a></div>
    <div><a href="http://images.cnitblog.com/i/333689/201403/181013163811731.jpg"><img src="http://images.cnitblog.com/i/333689/201403/181013035524683.jpg" alt=""></a></div>
    
</div>

<input type="button" value="upload" onclick="appendPic()" />


<script src="http://code.jquery.com/jquery-1.11.0.min.js"></script>
<script>
    function appendPic() {
        $(".img").hide();
        $(".img").append("<div><a href=\"http://images.cnitblog.com/i/333689/201403/181013442711411.jpg\"><img src=\"http://images.cnitblog.com/i/333689/201403/181013354124216.jpg\" alt=\"\"></a></div>");
        $('.img a').gallery();
        $(".img").show();
    }
    /*
    * jquery gallery JS
    * ZhaoHuanLei - 20140418
    */
    var srcGallery = new Array();
    (function ($) {
        $.fn.extend({
            gallery: function () {
                $(this).on("click", function () {
                
                    var self = $(this),
                    link = self.parent().parent().find("a");
                    if (srcGallery.indexOf(self.find("img").attr("src")) == -1) {


                        srcGallery.push(self.find("img").attr("src"));
                        var bd = $("body");
                        html = "\
                            <div class='gallery-overlay'>\
                                <div class='gallery-photo'><span></span><img src='" + self.attr("href") + "'></div>\
                                <div class='gallery-thumb'></div>\
                                <a class='gallery-prev' href='javascript:;' title='上一个'>&lt;</a>\
                                <a class='gallery-next' href='javascript:;' title='下一个'>&gt;</a>\
                                <a class='gallery-close' href='javascript:;' title='关闭'>&times;</a>\
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
                    }
                });
             
            }
        });
    })(jQuery);
</script>
<script>
    $(function () {
        //$('.img a').gallery();
        //$('.img a').gallery();
        //$('.img a').gallery();
        $('.img a').gallery();
        $(".img").show();
    });
</script>
</body>
</html>
