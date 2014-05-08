//ItemView
define('static/ItemView', ['backbone', 'jquery'], function (require, exports, module) {
	var $ = require('jquery')
	, Backbone = require('backbone');
	return Backbone.View.extend({
   			tagName : 'li'
   			, events :{
   				'click .swap' : 'swap',
   				'click .delete' : 'del'
   			}
   			, initialize : function(){
   				//_.bindAll(this,'render')

   				this.listenTo(this.model, 'change', this.render);
            	this.listenTo(this.model, 'destroy', this.remove);
   			}
   			, render : function(){
   				$(this.el).html('<span style="color:black;">'+this.model.get('name')+' '+this.model.get('name1')+'</span> &nbsp; &nbsp; <span class="swap" style="font-family:sans-serif; color:blue; cursor:pointer;">[swap]</span> <span class="delete" style="cursor:pointer; color:red; font-family:sans-serif;">[delete]</span>');
 				// this.$el.html('<span>' + this.model.get('name') + '</span>');
 				return this;
   			}
   			, swap : function(){
   				this.model.set({
   					name : this.model.get('name1'),
   					name1 : this.model.get('name'),
   				});
   			}
            ,del : function(){
               this.model.destroy();
            }
   		});
});

//ListView
define('static/ListView',['backbone','static/ItemView','static/World'], function (require, exports, nodule) {
	var $ = require('jquery')
	, Backbone = require('backbone')
	, ItemView = require('static/ItemView')
	, World = require('static/World');
	return Backbone.View.extend({
		el : $('body')
		,initialize : function () {
			 _.bindAll(this, 'render', 'appendItem');

			this.itemCount = 0;	
			this.collection = new Backbone.Collection();
	    	this.listenTo(this.collection, 'add', this.appendItem)
	        this.listenTo(this.collection, 'all', this.showCount)
	       //this.collection.on('add', this.appendItem)
	       //this.collection.on('remove', this.appendItem)
			this.render();
		}
		,events : {
			'click #add' : 'addItem'
		}
		,render : function () {
			var $this = this;
			this.$el.append("<button id='add'>Add list item</button>")
			.append('<ul></ul>')
       .append('总数：<span id="count" style="color:red;">' + this.collection.length + '<span>');

			_(this.collection.models).each(function(model){
			$this.appendItem(model);
			});
		}
		, addItem : function(){
			this.itemCount++;
			var world = new World();
			world.set('name', 'liulei' + this.itemCount);
			this.collection.add(world);
		}
		, appendItem :function(model){
			var item = new ItemView({model: model});
			// item.set('model', model);
			this.$el.find('ul').append(item.render().el);
		}
	    , showCount : function(){
	       this.$el.find('#count').text(this.collection.length);
	    }
	});
})

//
define('static/World', ['backbone'], function (require, exports, nodule) {
	var Backbone = require('backbone');
	return Backbone.Model.extend({
   			defaults : {
   				name : 'NULL',
   				name2 : 'UNDEFINED'
   			}
   		});
});