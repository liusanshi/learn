//选择用户
;(function($){
	$.fn.selectItem = function(options){
		var defaults = {
			checkboxName : '.im_sel_contacts li input',
			clearName:'.im_clear_btn',
			selItemName:'.im_sel_list ul',
			changItem:{
				sedList:'.im_sel_list',   //已选择列表
				selList:'.im_item>.im_sel_contacts', //选择列表
				selTree:'.im_tree', //选择树
				selTreeList:'.im_ob .im_sel_contacts', //左侧带树的选择列表
				sd_h : 27,
				st_h : 300,
				sl_h : 331,
				slt_h : 300
			}
		}
		var options = $.extend({},defaults,options);
		return this.each(function(){
		    var obj = new selectItem(options); //实例化对象
		    obj.init();
		});
	};
	//构造函数
	function selectItem(v){
		for(var i in v){
			this[i] = v[i];
		}
	}

	selectItem.prototype = {
		init : function(){
			var $formCheckbox =$(this.checkboxName);
			var $clearEmpty =$(this.clearName);
			var $selItem = $(this.selItemName);
			var dataVal,text,str,selVal;
			var _this = this;

			this.$sedList = $(this.changItem.sedList);
			this.$selList = $(this.changItem.selList);
			this.$selTree = $(this.changItem.selTree);
			this.$selTreeList = $(this.changItem.selTreeList);
			this.sd_h = this.changItem.sd_h;
			this.sl_h = this.changItem.sl_h;
			this.st_h = this.changItem.st_h;
			this.slt_h = this.changItem.slt_h;

			this.setHeight();
			//选择分类
            $formCheckbox.on({
            	click : function () {
	                dataVal = $(this).attr('data-num');
	                if ($(this).is(':checked')) {
	                    if ($selItem.find('li[data-num="' + dataVal + '"]').length <= 0) {
	                        text = $(this).parent().text().split('—');
	                        str = '<li data-num="' + dataVal + '" name="' + $(this).attr('name') + '" companyname="' + $(this).attr('companyname') + '" deptname="' + $(this).attr('deptname') + '" iconsrc="' + $(this).attr('iconsrc') + '" isdeveloper="' + $(this).attr('isdeveloper') + '" >' + text[0] + '<i class="im_ico"></i></li>';
	                        $selItem.append(str);
	                    }
	                } else {
	                    $selItem.find('li[data-num="' + dataVal + '"]').remove();
	                }
	                _this.setHeight();
	            }
	        });

            //清空已选择的分类
            $clearEmpty.on({
            	click : function () {
	                $selItem.children().remove();
	                $formCheckbox.attr("checked", false);
	                _this.setHeight();
	            }
	        });

            //清除当前分类
            $selItem.on('click', '>li>i', function () {
                selVal = $(this).parent().attr("data-num");
                $('input[data-num="' + selVal + '"]').attr("checked", false);
                $(this).parent().remove();
                _this.setHeight();
            });
		},
		setHeight : function(){
			var h = this.$sedList.height();
			var differ_h = 0;

			if(h > this.sd_h){
				differ_h = h - this.sd_h;
				this.$selList.height(this.sl_h-differ_h);
				this.$selTree.height(this.st_h-differ_h);
				this.$selTreeList.height(this.slt_h-differ_h);
			}else{
				this.$selList.height(this.sl_h);
				this.$selTree.height(this.st_h);
				this.$selTreeList.height(this.slt_h);
			}

		}
	};
})(jQuery);

$(function(){
	$(document).selectItem();
});
