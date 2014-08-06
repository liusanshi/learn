define('/static/tabManager', ['backbone'], function (require, exports, module) {

    var _ = require('underscore'),
        Backbone = require('backbone'), TabView, TabViewCollection;

    TabView = Backbone.View.extend({
        constructor: function (arg) {
            this.el = arg.el;
            this.model = new Backbone.Model(arg.model);
            Backbone.View.call(this);
        }
        , initialize: function () {
            this.listenTo(this.model, 'destroy', this.close);
        }
        , render: function () {
            /*{title : 'test', url:'...aspx', }*/
            var model = this.model;

            var tab = this.$el.tabs('add', {
                title: model.get('title'),
                content: '<div class="panel-loading">Loading...</div>',
                href : 'login.aspx', //加载页面来填充
                closable: true
            });
        }
        , close: function () {
            var el = this.$el, tab = el.tabs('getSelected');
            if (tab) {
                var index = el.tabs('getTabIndex', tab);
                el.tabs('close', index);
            }
            return this;
        }
    });

    TabViewCollection = Backbone.View.extend({
        constructor : function (el) {
            this.el = el;
            Backbone.View.call(this);
        }
        , initialize: function () {
            this.collection = new Backbone.Collection();
            this.listenTo(this.collection, 'add', this.open);
            this.listenTo(this.collection, 'remove', this.close);
        }
        , render: function () {
            var $this = this;            
            _(this.collection.models).each(function (model) {
                $this.open(model);
            });
            return this;
        }
        , open: function (model) {
            var tv = new TabView({ model: model, el: this.el })
            tv.render();
            this.collection.add(tv.model, { silent: true });
            return this;
        }
        , close: function (model) {
            if (model) {
                var tab = this.findTabModel(model.get('url'));
                if (tab) {
                    tab.trigger('destroy');
                    this.collection.remove(tab, { silent: true });
                }
            }
            return this;
        }
        , findTabModel: function (url) {
            return this.collection.findWhere({ 'url': url });
        }
    });

    return { TabView: TabView, TabViewCollection: TabViewCollection };
});