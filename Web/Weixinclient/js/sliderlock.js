$(document).ready(function(){
   // $(":input").attr("disabled", "disabled");
   
});


function refreshSwatch() {
    $SliderValue = $('#slider').slider("value");
   // alert($SliderValue)
	if($SliderValue>88){
	    $('#slider').slider("value", 88);
	    $('#slider p').html("解锁成功");
	    // $(":input").removeAttr("disabled");
	    $(".logo-btn").css("color", "white");
	    $(".logo-btn").attr("class", "logo-btn");
	    $(".logo-btn").removeAttr("disabled");
		$("#slider").unbind();
	}
}
$(function() {
	$("#slider").slider({
		change: refreshSwatch
	});

	
	$(".logo-btn").attr("disabled", "disabled");
	$(".logo-btn").css("color", "gray");
	$("#slider").bind("mousedown", function () {
       
	    setInterval(refreshSwatch, 100)
	});	
	
	// 上面说的你要是直接使用官方的ui js 文件, 要加入以代码
	// $("#slider").html('<span class="glyphicon glyphicon-arrow-right"></span>');
});