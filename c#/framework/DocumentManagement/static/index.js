define('/static/index', ['backbone', '/static/tabManager'], function (require, exports, module) {
    
    var _ = require('underscore'),
        Backbone = require('backbone'), Menu, MenuItemView, MenuItemData;

    MenuItemView = Backbone.View.extend({
        constructor : function (args) {
            this.tabManager = args.tabManager;
            Backbone.View.call(this, args);
        },
        tagName: 'li'
        , events: {
            'click': 'open'
        }
        , initialize: function () {
            //_.bindAll(this,'render')
        }
   		, render: function () {
   		    /*
            <li><div class="menuitem-icon icon-ok">功能管理图标</div><div>功能管理</div></li>
            {title:'功能管理', icon:'icon-ok', tips: '功能管理图标'}
            */
   		    var model = this.model;
   		    $(this.el).html('<div class="menuitem-icon ' + model.get('icon') + '">'
                + model.get('tips') + '</div><div>' + model.get('title') + '</div>');
   			return this;
   		}
        , del: function () {
            this.model.destroy();
        }
        , open: function () {
            var model = this.model;
            this.tabManager.open({ title: model.get('title'), url: model.get('title') });
        }
    });
    //菜单项
    Menu = Backbone.View.extend({
        constructor: function (el, tabManager, models) {
            this.el = el;
            this.$el = $(el);
            this.tabManager = tabManager;
            var coll = this.collection = new Backbone.Collection();
            _(models || []).each(function (m) {
                coll.add(m);
            });
            Backbone.View.call(this);
        }
        , initialize: function () {
            // this.render();
        }
        , render: function () {
            var $this = this;
            _(this.collection.models).each(function (model) {
                $this.appendItem(model);
            });
        }
        , appendItem: function (model) {
            var item = new MenuItemView({ model: model, tabManager: this.tabManager });
            this.$el.append(item.render().el);
        }
    });

    MenuItemData = Backbone.Model.extend({
        defaults: {
            title: '功能管理',
            icon: 'icon-ok',
            tips: '功能管理图标'
        }
    });

    return { 'Menu': Menu, 'Data': MenuItemData };
});

