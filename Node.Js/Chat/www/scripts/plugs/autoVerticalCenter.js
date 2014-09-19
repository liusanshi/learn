
//弹出层自动垂直居中
;(function($){
	$.fn.autoVerticalCenter = function(){
		// var defaults = {
		// 	cName : 'curr'
		// }
		// var options = $.extend({},defaults,options);
		return this.each(function(){
		    var obj = new autoVerticalCenter(this); //实例化对象
		    obj.init();
		});
	};
	//构造函数
	function autoVerticalCenter(o){
		this.$o = $(o);
	}

	autoVerticalCenter.prototype = {
		init : function(){
			var _this = this;
			$(window).on('resize',function(){
				_this.centerPos(_this.$o);
			});
			this.centerPos(_this.$o);
		},
		//垂直居中
		centerPos : function($o){
			var win_w = $(window).width();
			var win_h = $(window).height();

	    	var w = $o.width();
	    	var h = $o.height();

	    	var l = (win_w - w)/2 + $(document).scrollLeft();
	    	var t = (win_h - h)/2 + $(document).scrollTop();

	    	$o.css({ top:t+'px',left:l+'px'});
		}
	};
})(jQuery);

$(function(){
	$('.im_window').autoVerticalCenter();
});

