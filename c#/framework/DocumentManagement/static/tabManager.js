define('/static/tabManager', ['backbone'], function (require, exports, module) {

    var $ = require('$UI'), _ = require('underscore'),
        Backbone = require('backbone'), TabView, TabViewCollection;

    TabView = Backbone.View.extend({
        constructor: function (arg) {
            this.el = arg.el;
            this.model = new Backbone.Model(arg.model);
            Backbone.View.apply(this, arg);
        }
        , initialize: function () {
            this.listenTo(this.model, 'destroy', this.close);
        }
        , render: function () {
            /*{title : 'test', url:'...aspx', }*/
            var model = this.model;

            this.$el.tabs('add', {
                title: model.get('title'),
                content: '<div style="padding:10px">Content' + model.get('url') + '</div>',
                closable: true
            });
        }
        , close: function () {
            var el = this.$el, tab = el.tabs('getSelected');
            if (tab) {
                var index = el.tabs('getTabIndex', tab);
                el.tabs('close', index);
            }
        }
    });

    TabViewCollection = Backbone.View.extend({
        constructor : function (el) {
            this.el = el;
        }
        , initialize: function () {

            this.itemCount = 0;
            this.collection = new Backbone.Collection();
            this.listenTo(this.collection, 'add', this.open);
            this.listenTo(this.collection, 'remove', this.close);
        }
        , render: function () {
            var $this = this;            
            _(this.collection.models).each(function (model) {
                $this.open(model);
            });
        }
        , open: function (model) {
            new TabView({ model: model, el: this.el }).render();
        }
        , close: function (model) {
            var tab = this.findTab(model.get('url'));
            if (tab) {
                tab.trigger('destroy');
            }
        }
        , findTab: function (url) {
            return _(this.collection).find(function (m) { return m.get('url') === url; });
        }
    });

    return { TabView: TabView, TabManager: TabViewCollection };
});