/**
 * rule执行引擎
 * payneliu
 * 2018-04-02
 * rule的格式：
 * { type: 'and', list: [{type: ''and, list: [{logicL: '==', value: 1, executor: function(){}}]}, {type: 'or', list: []}] }
 * 
 * 栗子：
var rule = new qv.zero.RuleManager({
    logic: '==',
    value: 2,
    executor: function(context, next){
        next(2, '参数错误');
    }
});
rule.exec({arg1: 1, arg2: 2}, function(match, msg){
    console.log(match, msg);
});

var rule2 = new qv.zero.RuleManager({
    type: 'and',
    list: [
        {
            logic: '==',
            value: 1,
            executor: function(context, next){
                next(context.arg1, 'rule2 - 参数错误1');
            }
        },
        {
            logic: '==',
            value: 2,
            executor: function(context, next){
                next(context.arg2, 'rule2 - 参数错误2');
            }
        },
]
});
rule2.exec({arg1: 1, arg2: 3}, function(match, msg){
    console.log(match, msg);
});

var rule3 = new qv.zero.RuleManager({
    type: 'or',
    list: [
        {
            logic: '==',
            value: 1,
            executor: function(context, next){
                next(context.arg1, 'rule3 - 参数错误1');
            }
        },
        {
            logic: '==',
            value: 2,
            executor: function(context, next){
                next(context.arg2, 'rule3 - 参数错误2');
            }
        },
]
});
rule3.exec({arg1: 1, arg2: 3}, function(match, msg){
    console.log(match, msg);
});


var rule4 = new qv.zero.RuleManager({
    type: 'or',
    list: [
        {
            type: 'and',
            list: [
                {
                    logic: '==',
                    value: 1,
                    executor: function(context, next){
                        next(context.arg1, 'rule4 - 参数错误1');
                    }
                },
                {
                    logic: '>=',
                    value: 2,
                    executor: function(context, next){
                        next(context.arg1, 'rule4 - 参数错误1');
                    }
                }
            ]
        },
        {
            type: 'and',
            list: [
                {
                    logic: '==',
                    value: 1,
                    executor: function(context, next){
                        next(context.arg1, 'rule4 - 参数错误3');
                    }
                },
                {
                    logic: '>=',
                    value: 2,
                    executor: function(context, next){
                        next(context.arg1, 'rule4 - 参数错误4');
                    }
                }
            ]
        },
]
});
rule4.exec({arg1: 1, arg2: 3}, function(match, msg){
    console.log(match, msg);
});

 */
(function (exports) {
    function Rule(logic, value, executor, plist){
        this.value = value;
        this.logic = logic;
        this.executor = executor;
        this.plist = plist || {};
    }
    Rule.prototype.exec = function(context, next){
        var me = this;
        this.executor(context, function(res, data){
            switch (me.logic){
                case '>':
                    return next(res > me.value, data);
                case '>=':
                    return (res >= me.value, data);
                case '<':
                    return next(res < me.value, data);
                case '<=':
                    return next(res <= me.value, data);
                case '==':
                    return next(res == me.value, data);
                case 'in':
                    if(me.value.indexOf){ //string || array
                        return next(~me.value.indexOf(res), data);
                    } else { //object
                        return next(!!(me.value && me.value[res]), data);
                    }
                default:
                    return next(false, {msg: 'logic error'});
            }
        });
    };

    //将数据结构转换为rule
    function convertToRule(data){
        if(data.list){
            if(data.type === 'or'){
                return new OrRule(data.list);
            } else if(data.type === 'and'){
                return new AndRule(data.list);
            } else {
                throw '数据格式错误';
            }
        } else if('logic' in data && 'value' in data && 'executor' in data) {
            return new Rule(data.logic, data.value, data.executor, data.plist);
        } else {
            throw '数据格式错误';
        }
    }

    //组装rule的结构
    function convertToRuleList(data){
        var list = [];
        data = data || [];
        for (var index = 0; index < data.length; index++) {
            list.push(convertToRule(data[index]));
        }
        return list;
    }

    function AndRule(data){
        this.list = convertToRuleList(data);
        this._compiled = null;
        
    }
    AndRule.prototype.add = function(rule){
        this._compiled = null;
        this.list.push(rule);
    };
    AndRule.prototype.exec = function(context, next){
        var total, length = this.list.length - 1;
        if(length === 0){ //单个条件时的优化
            return this.list[0].exec(context, next);
        }
        if(!this._compiled){
            for (var index = length; index >= 0; index--) {
                var element = this.list[index];
                total = (function(prev, item){
                    return function(context){
                        item.exec(context, function(match, msg){
                            if(match){
                                prev ? prev(context) : next(true, msg);
                            } else {
                                next(false, msg);
                            }
                        });
                    };
                }(total, element));
            }
            this._compiled = total;
        }
        return this._compiled(context);
    };

    function OrRule(data){
        this.list = convertToRuleList(data);
        this._compiled = null;
    }
    OrRule.prototype.add = function(rule){
        this._compiled = null;
        this.list.push(rule);
    };
    OrRule.prototype.exec = function(context, next){
        var total, length = this.list.length - 1;
        if(length === 0){ //单个条件时的优化
            return this.list[0].exec(context, next);
        }
        if(!this._compiled){
            for (var index = length; index >= 0; index--) {
                var element = this.list[index];
                total = (function(prev, item){
                    return function(context){
                        item.exec(context, function(match, msg){
                            if(match){
                                next(true, msg);
                            } else {
                                prev ? prev(context) : next(false, msg) ;
                            }
                        });
                    };
                }(total, element));
            }
            this._compiled = total;
        }
        return this._compiled(context);
    };

    /**
     * rule管理
     * { type: 'and', list: [{type: ''and, list: [{logicL: '==', value: 1, executor: function(){}}]}, {type: 'or', list: []}] }
     * @param {*} data 
     */
    function RuleManager(data){
        this.rule = convertToRule(data);
    }
    RuleManager.prototype.exec = function(context, callback){
        this.rule && this.rule.exec(context, callback);
    };
    exports.RuleManager = RuleManager;
})(qv.zero);