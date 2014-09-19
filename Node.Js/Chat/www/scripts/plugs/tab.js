
//选项卡插件
;(function($){
	$.fn.tab = function(options){
		var defaults = {
			cName : 'curr',
			titName:'.im_tab_tit li',
			contName:'.im_personnel_list .im_item'
		}
		var options = $.extend({},defaults,options);
		return this.each(function(){
		    var obj = new tab(options); //实例化对象
		    obj.init();
		});
	};
	//构造函数
	function tab(v){
		for(var i in v){
			this[i] = v[i];
		}
	}

	tab.prototype = {
		init : function(){
			var _this = this;
		   	$(this.titName).eq(0).addClass(this.cName);  //选项一选中
		    $(this.contName).not(':first').hide();        //第一个内容显示
		    $(this.titName).on({
				'click' : function(){
				    _this.switchover($(this));
				    tabAfterClick(this);
					}
		    });
		},
		//切换方法
		switchover : function(obj){
	    	obj.addClass(this.cName).siblings().removeClass(this.cName);
	    	var index = obj.index();
	    	$(this.contName).hide().eq(index).show();
		}
	};
})(jQuery);

$(function(){
	$(document).tab();
});

