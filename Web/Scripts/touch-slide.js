(function ($) {
    /**
    *@基于JQ的移动设备滑动幻灯片效果
    *@按照约定的DOM结构添加内容
    *@可定制自己想要的样式
    *@杨永
    *@2014-04-11
    *@QQ:377746756
    *@调用方法及传参
    *@var touchSlide=new TouchSlide($("#J_TouchSlide"),{
    isShowBtns:"yes", //是否显示索引按钮，默认不显示
    autoPlay:true,    //是否自动播放，默认不轮播
    autoTime:1000,    //设置轮播的间隔时间
    isLoop:false,        //是否启用循环切换
    callBack:function(elem){    //切换完成回调函数
    console.log(elem);//打印当前的帧
    }
    });
    */
    function TouchSlide(touchMain, setting) {
        var _this_ = this;
        //保存单个对象
        this.touchMain = touchMain;

        //保存动画对象
        this.touchSlide = $(".slide-box", touchMain);
       
        //保存帧数列表
        this.slideItems = $(".slide-item", this.touchSlide);
        this.touchMain.get(0).addEventListener("touchstart", function (evt) { });
        alert(this.touchMain.html());
        //保存帧数
        this.slideItemSize = this.slideItems.size();
        //保存幻灯片的偏移值
        this.slideOffset = {
            offsetX: this.touchMain.offset().left,
            offsetY: this.touchMain.offset().top
        };
        //当帧数大于1的时候
        if (this.slideItemSize > 1) {
            //计数器
            this.loop = 0;
            //保存幻灯片的宽度
            this.slideW = this.touchMain.width();
            this.flag = true;
            //绑定事件
            this.touchMain.get(0).addEventListener("touchstart", function (evt) {
                if (_this_.flag) {
                    _this_.flag = false;
                    window.clearInterval(_this_.autoTimer);
                    //记录触摸位置
                    _this_.touchStartX = _this_.getLayerOffset(evt).layerX;
                    _this_.touchStartY = _this_.getLayerOffset(evt).layerY;
                    //绑定文档touchmove事件
                    function relaseMove(evt) {
                        //如果上下滚动，就不执行touchMove
                        var diffX = _this_.touchStartX - _this_.getLayerOffset(evt).layerX;
                        var diffY = Math.abs(_this_.touchStartY - _this_.getLayerOffset(evt).layerY);
                        if (diffY < Math.abs(diffX)) {
                            _this_.touchMove.call(_this_, evt);
                        };
                    };
                    document.addEventListener("touchmove", relaseMove, false);
                    //触摸停止
                    function relaseEnd(evt) {
                        document.removeEventListener("touchmove", relaseMove, false);
                        document.removeEventListener("touchend", relaseEnd, false);
                        _this_.touchEnd.call(_this_, evt);
                        if (_this_.auto) {
                            _this_.autoPlay();
                        };
                    };
                    document.addEventListener("touchend", relaseEnd, false);
                };
            }, false);
            /**
            *@这里待设置的配置参数
            *@setting={
            autoPlay:true,        //用来控制是否自动轮播
            autoTime:500,         //控制自动播放的时间
            isLoop:false/true,    //是否启用循环切换
            callBack:fun,          //每一帧切换完毕触发的回调函数
            isShowBtns:'yes/no'       //是否显示索引按钮
            }
            */
            this.auto = setting.autoPlay || false;
            this.autoTime = setting.autoTime || 5000;
            this.callBack = setting.callBack || function () { };
            this.isShowBtns = setting.isShowBtns || "yes";
            this.isLoop = setting.isLoop == false ? false : true;
            //启动自动播放
            if (this.auto) {
                this.autoPlay();
            };
            if (this.isShowBtns == "yes") {
                //创建按钮
                this.createControlBtns();
                //保存索引按钮
                this.controlBtns = this.touchMain.find(".slide-control span");
                //设置居中显示
                this.controlBtns.parent().css("marginLeft", -this.slideItemSize * 14 / 2);
            };
        };
    };
    TouchSlide.prototype = {
        //自动播放
        autoPlay: function () {
            var _this_ = this;
            this.autoTimer = window.setInterval(function () {
                _this_.transform();
            }, this.autoTime);
        },
        transform: function () {
            var _this_ = this;
            this.loop++;
            if (this.loop > this.slideItemSize - 1) {
                this.loop = 0
            };
            this.slideItems.eq(this.loop).css("left", this.slideW);
            this.touchSlide.animate({ left: -this.slideW }, "fast", function () {
                _this_.flag = true;
                $(this).css("left", 0);
                _this_.slideItems.css("left", 0).eq(_this_.loop).addClass("current").siblings().removeClass("current");
                if (_this_.isShowBtns == "yes") {
                    _this_.controlBtns.eq(_this_.loop).addClass("selected").siblings().removeClass("selected");
                };
                //触发回调函数
                if (_this_.callBack) {
                    _this_.callBack(_this_.slideItems.eq(_this_.loop));
                };
            });
        },
        //创建索引按钮
        createControlBtns: function () {
            var html = "<div class='slide-control'>";
            this.slideItems.each(function (i, o) {
                if (i == 0) {
                    html += "<span class='selected'></span>";
                } else {
                    html += "<span></span>";
                };
            });
            html += "</div>";
            this.touchSlide.after(html);
        },
        touchMove: function (evt) {
            var diffX = this.touchStartX - this.getLayerOffset(evt).layerX;
            var diffY = Math.abs(this.touchStartY - this.getLayerOffset(evt).layerY);
            //阻止默认滚屏
            if (Math.abs(diffX) - diffY > 0) {
                evt.preventDefault();
            };
            //如果是向左滑动
            if (diffX >= 0) {
                //阻止循环切换
                if (this.loop != (this.slideItemSize - 1) || this.isLoop) {
                    this.setItemOffset("right");
                };
            } else {
                //阻止循环切换
                if (this.loop != 0 || this.isLoop) {
                    this.setItemOffset("left");
                };
            };
            //滑动起来
            this.touchSlide.css("left", -(this.touchStartX - this.getLayerOffset(evt).layerX));

        },
        touchEnd: function (evt) {
            var _this_ = this;
            var diff = this.touchStartX - this.getLayerOffset(evt).layerX;
            //当触摸水平位置大于30的时候才过度
            if (Math.abs(diff) >= 30) {
                if (diff > 0) {
                    //滑动结束的时候不在循
                    if (this.loop != (this.slideItemSize - 1) || this.isLoop) {
                        this.loop++;
                        if (this.loop > this.slideItemSize - 1) {
                            this.loop = 0;
                        };
                        this.touchSlide.animate({ left: -this.slideW }, "fast", function () {
                            _this_.flag = true;
                            $(this).css("left", 0);
                            _this_.slideItems.css({ left: "auto", right: "auto" }).eq(_this_.loop).addClass("current").siblings().removeClass("current");
                            //设置选中样式
                            if (_this_.isShowBtns == "yes") {
                                _this_.controlBtns.eq(_this_.loop).addClass("selected").siblings().removeClass("selected");
                            };
                            //触发回调函数
                            if (_this_.callBack) {
                                _this_.callBack(_this_.slideItems.eq(_this_.loop));
                            };
                        });
                    } else {
                        this.touchSlide.animate({ left: 0 }, "fast", function () {
                            _this_.flag = true;
                            _this_.slideItems.css({ left: "auto", right: "auto" }).eq(_this_.loop).addClass("current").siblings().removeClass("current");
                        });
                    };
                } else {
                    //滑动结束的时候不在循环
                    if (this.loop != 0 || this.isLoop) {
                        this.loop--;
                        if (this.loop < 0) {
                            this.loop = this.slideItemSize - 1;
                        };
                        this.touchSlide.animate({ left: this.slideW }, "fast", function () {
                            _this_.flag = true;
                            $(this).css("left", 0);
                            _this_.slideItems.css({ left: "auto", right: "auto" }).eq(_this_.loop).addClass("current").siblings().removeClass("current");
                            //设置选中样式
                            if (_this_.isShowBtns == "yes") {
                                _this_.controlBtns.eq(_this_.loop).addClass("selected").siblings().removeClass("selected");
                            };
                            //触发回调函数
                            if (_this_.callBack) {
                                _this_.callBack(_this_.slideItems.eq(_this_.loop));
                            };
                        });
                    } else {
                        this.touchSlide.animate({ left: 0 }, "fast", function () {
                            _this_.flag = true;
                            _this_.slideItems.css({ left: "auto", right: "auto" }).eq(_this_.loop).addClass("current").siblings().removeClass("current");
                        });
                    };
                };
            } else {
                this.touchSlide.animate({ left: 0 }, "fast", function () {
                    _this_.flag = true;
                    _this_.slideItems.css({ left: "auto", right: "auto" }).eq(_this_.loop).addClass("current").siblings().removeClass("current");
                });
            };
        },
        //设置对应帧左右偏移
        setItemOffset: function (dir) {
            //如果dir=="right"
            if (dir == "right") {
                if (this.loop == this.slideItemSize - 1) {
                    this.slideItems.eq(0).css("left", this.slideW);
                } else {
                    this.slideItems.eq(this.loop + 1).css("left", this.slideW);
                };
            } else if (dir == "left") {
                if (this.loop == 0) {
                    this.slideItems.eq(this.slideItemSize - 1).css("left", -this.slideW);
                } else {
                    this.slideItems.eq(this.loop - 1).css("left", -this.slideW);
                };
            };
        },
        //获取layerX、layerY
        getLayerOffset: function (evt) {
            return {
                layerX: evt.changedTouches[0].pageX - this.slideOffset.offsetX,
                layerY: evt.changedTouches[0].pageY - this.slideOffset.offsetY
            };
        }
    };
    window.TouchSlide = TouchSlide;
})(jQuery);