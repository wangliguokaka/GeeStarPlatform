/*xn_ba_js_4_banner*/

//自动播放与时间间隔
var xn_ba_js_4_autoPlay = true;
var xn_ba_js_4_interval = 4000;

//两侧图片透明度
var xn_ba_js_4_imgOpacity = 0.5;


var xn_ba_js_4_nextfunc;
var xn_ba_js_4_timer;
var banner4_Start;

$(document).ready(function() {
    banner4_Start();
});



banner4_Start = function() {
    var index = 0;
    var prev = 0;
    var imgW; ;
    var imgH; ;
    var maxLen = 0;

    imgW = $(".xn_ba_js_4_bigImg").find("img").eq(0).width();
    imgH = $(".xn_ba_js_4_bigImg").find("img").eq(0).height();
    maxLen = $(".xn_ba_js_4_main").find(".xn_ba_js_4_element").length;

    $(".xn_ba_js_4_main").css({ background: "#000000", visibility: "visible" });
    if ($(".xn_ba_js_4_main").size()) {
        funcmaincontent();
    };


    function funcmaincontent() {

        startslide();

        function startslide() {


            var btn = $(".xn_ba_js_4_row_btn").find("li").eq(0);
            var btnW = btn.width() + Math.round(parseFloat(btn.css("margin-left")));
            $(".xn_ba_js_4_row_btn").css("width", maxLen * btnW);
            btn.attr("class", "xn_ba_js_4_element_btn_on");

            $(".xn_ba_js_4_row_btn li").each(function(i) {
                $(this).click(function() {
                    skipHandler((i));
                });
            });

            for (var i = 0; i < maxLen; i++) {
                $(".xn_ba_js_4_main").find(".xn_ba_js_4_element").eq(i).attr("id", "ea_ba_no_b_" + i);

                var _pos = Math.round(imgW * (i - index) + $(window).width() / 2 - imgW / 2);
                var _opa = xn_ba_js_4_imgOpacity;
                if (i == index) _opa = 1;
                if (_pos > $(window).width()) {
                    _pos -= maxLen * imgW
                } else if (_pos < -imgW) {
                    _pos += maxLen * imgW
                }
                $(".xn_ba_js_4_main").find(".xn_ba_js_4_element").eq(i).css({
                    left: _pos,
                    opacity: 0
                })
				.animate({
				    opacity: _opa
				}, {
				    duration: 400,
				    easing: 'linear'
				});



            }

            $(window).resize(onResize);
            function onResize() {
                for (var i = 0; i < maxLen; i++) {
                    var _pos = Math.round(imgW * (i - index) + $(window).width() / 2 - imgW / 2);
                    var _opa = xn_ba_js_4_imgOpacity;
                    if (i == index) _opa = 1;
                    if (_pos > $(window).width()) {
                        _pos -= maxLen * imgW
                    }
                    $(".xn_ba_js_4_main").stop().find(".xn_ba_js_4_element").eq(i).css({
                        left: _pos,
                        opacity: _opa
                    })
                }
                //alert("--");
            };
            $(".xn_ba_js_4_right").click(nextPage);
            $(".xn_ba_js_4_left").click(prevPage);

            //添加计时器跳转
            timerRepeat();
        }

        //图片轮转,循环播放
        function timerRepeat() {
            if (!xn_ba_js_4_autoPlay) return;
            xn_ba_js_4_nextfunc = isPause;
            xn_ba_js_4_timer = setInterval("xn_ba_js_4_nextfunc()", xn_ba_js_4_interval);

        }

        function isPause() {
            //每次跳转前检测判断值
            var isRun = true;
            if (typeof parent.runonce != 'undefined') {
                isRun = parent.runonce;
            }
            if (isRun) {
                nextPage();
            } else {
                //暂停
            }
        }

        function skipHandler(i) {
            if (index == i) return;
            if (i > index) {
                while (index < i) {
                    nextPage();
                }
            } else {
                while (index > i) {
                    prevPage();
                }
            }

        }

        //下一个
        function nextPage() {
            if (xn_ba_js_4_timer) {
                //重置
                clearInterval(xn_ba_js_4_timer);
                timerRepeat();
            }

            index++;
            if (index > maxLen - 1) index = 0;
            if ($(window.parent.bannerparam).length > 0) {
                //设置全局索引
                window.parent.bannerparam.cur_ba_index = index;
            }

            btnHandler();

            _pict = $(".xn_ba_js_4_main").find(".xn_ba_js_4_element");
            for (var i = 0; i < maxLen; i++) {
                var _pos = Math.round(imgW * (i - index) + $(window).width() / 2 - imgW / 2);
                var _opa = xn_ba_js_4_imgOpacity;
                if (i == index) _opa = 1;
                if (_pos > $(window).width()) {
                    _pos -= maxLen * imgW
                } else if (_pos < -imgW * 2) {
                    _pos += maxLen * imgW
                }

                _pict.eq(i)
				.css({
				    left: _pos + imgW
				})
				.animate({
				    left: _pos,
				    opacity: _opa
				}, {
				    duration: 700,
				    easing: 'easeOutQuint',
				    queue: false
				})
            }
            //if(imgW)alert([imgW,parseInt($(".xn_ba_js_4_element").css("width")),$(".xn_ba_js_4_element").css("width")]);
            //当图片为3个时特殊处理
            if (maxLen == 3 && index == 2) {
                _pict.eq(0)
				.css({
				    left: _pos + imgW * 2
				}).animate({
				    left: _pos + imgW,
				    opacity: 0.5
				}, {
				    duration: 700,
				    easing: 'easeOutQuint',
				    queue: false
				})
            }
            prev = index;
        }

        function btnHandler() {
            var btn = $(".xn_ba_js_4_row_btn").find("li")
            btn.eq(prev).attr("class", "xn_ba_js_4_element_btn");
            btn.eq(index).attr("class", "xn_ba_js_4_element_btn_on")
            var btn = $(".xn_ba_js_4_row_btn").find("li");
        }

        function prevPage() {
            if (xn_ba_js_4_timer) {
                //重置
                clearInterval(xn_ba_js_4_timer);
                timerRepeat();
            }


            index--;
            if (index < 0) index = maxLen - 1;
            if ($(window.parent.bannerparam).length > 0) {
                //设置全局索引
                window.parent.bannerparam.cur_ba_index = index;
            }
            btnHandler();
            for (var i = 0; i < maxLen; i++) {
                var _pos = Math.round(imgW * (i - index) + $(window).width() / 2 - imgW / 2);
                var _opa = xn_ba_js_4_imgOpacity;
                if (i == index) _opa = 1;
                if (_pos < -imgW) {
                    _pos += maxLen * imgW
                } else if (_pos > $(window).width() + imgW) {
                    _pos -= maxLen * imgW
                }
                $(".xn_ba_js_4_main").find(".xn_ba_js_4_element").eq(i)
				.stop()
				.css({
				    left: _pos - imgW
				})
				.animate({
				    left: _pos,
				    opacity: _opa
				}, {
				    duration: 700,
				    easing: 'easeOutQuint',
				    queue: false
				})
            }

            //当图片为3个时特殊处理
            if (maxLen == 3 && index == 0) {
                $(".xn_ba_js_4_main").find(".xn_ba_js_4_element").eq(2)
				.stop()
				.css({
				    left: -((Math.round(imgW * (0 - index) + $(window).width() - imgW / 2))) - imgW
				})
				.animate({
				    left: Math.round(imgW * (0 - index) + $(window).width() / 2 - imgW / 2) - imgW,
				    opacity: _opa
				}, {
				    duration: 700,
				    easing: 'easeOutQuint',
				    queue: false
				})
            }
            prev = index;
        }
    }

};

jQuery.extend(jQuery.easing, {
    def: 'easeOutQuint',
    swing: function(x, t, b, c, d) {
        return jQuery.easing[jQuery.easing.def](x, t, b, c, d);
    },
    easeOutQuint: function(x, t, b, c, d) {
        return c * ((t = t / d - 1) * t * t * t * t + 1) + b;
    }
});
/*end_xn_ba_js_4_banner*/