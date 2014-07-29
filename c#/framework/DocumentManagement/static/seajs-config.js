
//seajs 配置
(function (seajs) {
    seajs.config({
        // 别名配置
        alias: {
            jquery: 'jquery-1.11.0.min',
            backbone: 'backbone-min',
            underscore: 'underscore-min',
            $UI: 'jquery-easyui/jquery.easyui.min'
        },
        base: '/modules',
        preload: ['jquery', 'underscore']//这里是没有依赖的
    });

    //下面的方法可以用于第三方的库上面
    seajs.use('$UI', function () {
        var Module = seajs.Module, cache = seajs.cache;
        Module.get(Module.resolve('jquery')).exports = $;
        Module.get(Module.resolve('$UI')).exports = $;
    });

    seajs.use('backbone', function () { //先加载underscore 然后再加载backbone
        var Module = seajs.Module, cache = seajs.cache;
        Module.get(Module.resolve('underscore')).exports = _;
        var backb = Module.get(Module.resolve('backbone'));
        backb.exports = Backbone;
        (backb.dependencies || (backb.dependencies = [])).push("underscore");

    });
})(seajs);