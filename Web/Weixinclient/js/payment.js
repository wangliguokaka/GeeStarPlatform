$(function () {
    $(document).on("click",".order-click",function () {
        $("#order-bg").css({
            display: "block", height: $(document).height()
        });
        var $box = $('.order-mask');
        $box.css({
            display: "block",
        });
    });
    //点击关闭按钮的时候，遮罩层关闭
    $(document).on('click',".order-close",function () {
        $("#order-bg,.order-mask").css("display", "none");
    });
});



$(document).ready(function() {
    $(document).on("click", '.inactive',function () {
        
        var currentID = $(this).parent().attr("id");
        //alert($(this).parent().parent().html())
        
        $(this).parent().parent().find('ul').each(function () {

            if (currentID != $(this).parent().attr("id")) {
                $(this).siblings('span').removeClass('inactives');
                $(this).css("display", "none");
            }           
        });

        //$(this).parent('li').siblings('li').removeClass('inactives');
        //$(this).addClass('inactives');
        //$(this).siblings('ul').slideDown(100).children('li');
			if($(this).siblings('ul').css('display')=='none'){
				$(this).parent('li').siblings('li').removeClass('inactives');
				$(this).addClass('inactives');
				$(this).siblings('ul').slideDown(100).children('li');
			}else{
				//控制自身变成+号
				$(this).removeClass('inactives');
				//控制自身菜单下子菜单隐藏
				$(this).siblings('ul').slideUp(100);
				//控制同级菜单只保持一个是展开的（-号显示）
				$(this).siblings('ul').children('li').children('span').removeClass('inactives');
			}
		})
	});
