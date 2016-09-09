if (typeof Object.create !== 'function') {
  // Production steps of ECMA-262, Edition 5, 15.2.3.5
  // Reference: http://es5.github.io/#x15.2.3.5
  Object.create = (function() {
    //为了节省内存，使用一个共享的构造器
    function Temp() {}

    // 使用 Object.prototype.hasOwnProperty 更安全的引用 
    var hasOwn = Object.prototype.hasOwnProperty;

    return function (O) {
      // 1. 如果 O 不是 Object 或 null，抛出一个 TypeError 异常。
      if (typeof O !== 'object') {
        throw TypeError('Object prototype may only be an Object or null');
      }

      // 2. 使创建的一个新的对象为 obj ，就和通过
      //    new Object() 表达式创建一个新对象一样，
      //    Object是标准内置的构造器名
      // 3. 设置 obj 的内部属性 [[Prototype]] 为 O。
      Temp.prototype = O;
      var obj = new Temp();
      Temp.prototype = null; // 不要保持一个 O 的杂散引用（a stray reference）...

      // 4. 如果存在参数 Properties ，而不是 undefined ，
      //    那么就把参数的自身属性添加到 obj 上，就像调用
      //    携带obj ，Properties两个参数的标准内置函数
      //    Object.defineProperties() 一样。
      if (arguments.length > 1) {
        // Object.defineProperties does ToObject on its first argument.
        var Properties = Object(arguments[1]);
        for (var prop in Properties) {
          if (hasOwn.call(Properties, prop)) {
            obj[prop] = Properties[prop];
          }
        }
      }

      // 5. 返回 obj
      return obj;
    };
  })();
}

/**
 * 代理
 * @param object obj     
 * @param object handler 
 * {
 *     apply : function(target, thisArg, args){},
 *     get   : function(target, property, receiver){},
 *     set   : function(target, property, value, receiver){}
 * }
 */
function ObjectProxy(obj, handler) {
    var proxy = Object.create(obj);
    init(proxy, obj, handler);
    return proxy;
}
function init(newobj, oldobj, handler){
    var item, hasapply = !!handler.apply, hasget = !!handler.get, hasset = !!handler.set;
    for(var i in oldobj){
        if(oldobj.hasOwnProperty(i)){
            item = oldobj[i];
            if(isFunction(item)){
                hasapply && (newobj[i] = (function(func, name){
                    return function(){
                        return handler.apply(func, newobj, arguments, name, oldobj);
                    }
                })(item, i));
            } else {
                if(hasget && hasset){
                    (function(key){
                        Object.defineProperty(newobj, key, { 
                            get : function(){ return handler.get(oldobj, key, newobj) }, 
                            set : function(nval){ handler.set(oldobj, key, nval, newobj)  } 
                        });
                    })(i);
                } else if(hasget){
                    (function(key){
                        Object.defineProperty(newobj, key, { 
                            get : function(){ return handler.get(oldobj, key, newobj) }
                        });
                    })(i);
                } else if(hasset){
                    (function(key){
                        Object.defineProperty(newobj, key, { 
                            set : function(nval){ handler.set(oldobj, key, nval, newobj)  } 
                        });
                    })(i);
                }
            }
        }
    }
}

function isFunction(val){
    return Object.prototype.toString.call(val) === '[object Function]';
}
//标识
function Identity(){
    this.pre = '__id__';
    this.id = this.pre + (+new Date);
}
Identity.prototype.getIdentity = Identity.prototype.toString = Identity.prototype.valueOf = function(){
    return this.id;
};

//调用描述
function Invocation(proxy, context, handler){
    this.proxy = proxy;
    this.context = context;
    this.handler = handler;
    this.info = '';
}
//执行
Invocation.prototype.procced = function() {
    this.result = this.handler.apply(this.context, this.args);
    return this.result;
};
//设置参数
Invocation.prototype.setArgs = function(args) {
    this.args = args;
};

var FunctionProxy = (function(){
    //函数调用代理
    function InvokeProxy(invocation){
        this.invocation = invocation;
        this.list = [];
        this.state = 0;//0: 初始化之前，1: 初始化完成，2：开始执行before，3：执行原始函数，4：开始执行after，5：开始执行except， 6：执行完成
        this._max = 0;
        this._index = -1;
        this._error = null;  //异常
    }
    //注册拦截函数
    InvokeProxy.prototype.register = function(/*before, after, except, index, info*/){
        if(this.state === 6){
            this.state = 0; //重置
        } else if(this.state !== 0){
            //在执行的过程中不能添加拦截函数
            throw new Error('function was running');
        }
        var len = arguments.length, before = arguments[0], after, except, index, info;
        if(len === 3 || len === 4 || len === 5){
            after = arguments[1];
            except = arguments[2];
            index = compCurIndex(this, arguments[3]);
            this.list.push({"before" : before, "after" : after, "except" : except, "index" : index, 'info' : arguments[4] || ''});
            this.state = 0;
        } else if(len === 1 && before){
            if(isFunction(before)){ //只传递before
                index = compCurIndex(this);
                this.list.push({"before" : before, "index" : index});
            } else {
                before.index = compCurIndex(this, before.index);
                this.list.push(before);
            }
            this.state = 0;
        } else {
            throw new Error('arguments error');
        }
    };
    //重置状态
    InvokeProxy.prototype.reset = function(){
        if(this.state === 6){
            this.state = 1;
        }
    };
    //执行下一个拦截器 可重入
    InvokeProxy.prototype.next = function(){
        var step = 1;
        switch(this.state){
            case 0 : //初始化之前
                this.list.sort(function(a, b){ return b.index - a.index; });
                this.state = 1;//标识初始化完成
            case 1 : //初始化完成
                this._index = -1;
                step = 1;
                this.state = 2; //标识开始执行before
            // break;
            case 2 : //开始执行before
                this._index += step;
                if(!this._next(this._index, step, 'before')){
                    this.state = 3; //标识before执行结束
                } else{
                    break;        
                }            
            case 3 : //开始执行原始函数
                try{
                    this.invocation.procced();
                    this.state = 4;
                    this.next();
                } catch(e){
                    this._error = e;
                    this._index = this.list.length;
                    this.state = 5;
                    this.next();
                }
            break;
            case 4 : //开始执行after
                step = -1;
                this._index += step;
                if(!this._next(this._index, step, 'after')){
                    this.state = 6; //标识after执行结束
                }
            break;
            case 5 : //开始执行except
                step = -1;
                this._index += step;
                if(!this._next(this._index, step, 'except')){
                    this.state = 6; //标识except执行结束
                }
            break;
        }
    }
    //执行下一个拦截器
    InvokeProxy.prototype._next = function(pos, step, method){
        var current = this.list[pos], m, that = this;
        if(current){
            if(m = current[method]){
                this.invocation.info = current.info || '';
                m.call(this.invocation.context, this.invocation, function(){ that.next() }); //调用
                return true;
            } else {
                return this._next(pos, step, method);
            }
        }
        return false;
    };
    //执行
    InvokeProxy.prototype.run = function(args){
        this.invocation.setArgs(args);
        this.next();
    }

    //计算当前的索引
    function compCurIndex(fp, index){
        index = +index;
        if(isFinite(index)){ //数字
            if(fp._max > index){
                return fp._max;
            } else {
                fp._max = index;
                return index;
            }
        } else {
            return ++fp._max;
        }
    }

    //函数的代理
    function FunctionProxy(invocation){
        this.invokeproxy = new InvokeProxy(invocation);
        this.cb = {};
    }
    //设置执行描述
    FunctionProxy.prototype.setInvocation = function(invocation){
        this.invokeproxy.invocation = invocation;
    };
    //执行
    FunctionProxy.prototype.run = function(args){
        this.invokeproxy.reset();
        this.invokeproxy.run(Array.prototype.slice.call(args || []));
        return this.invokeproxy.invocation.result;
    };
    //注册函数的拦截器
    FunctionProxy.prototype.register = function(before, after, except, index, info){
        this.invokeproxy.register(before, after, except, index, info);
    };
    //注册回调函数的拦截器
    FunctionProxy.prototype.register_args = function(posi/*参数所在的位置*/, before, after, except, index, info){
        var me = this, inproxy;
        if(!(inproxy = me.cb[posi])){
            inproxy = me.cb[posi] = new InvokeProxy();
            this.invokeproxy.register(function(invocation, next){
                if(invocation.args[posi] && isFunction(invocation.args[posi])){
                    inproxy.invocation = new Invocation(me, me, invocation.args[posi]);
                    invocation.args[posi] = function(){
                        inproxy.run(arguments);
                    }
                }
                next();
            }, null, null, -1);
        }
        inproxy.register(before, after, except, index, info);
    };
    //注册返回CallBack的拦截器
    FunctionProxy.prototype.register_cb = function(proxy, index, except, info){
        this.invokeproxy.register(null, function(invocation, next){
            var cb = new qv.zero.CallBack();
            invocation.result.add(function(ret){
                proxy(ret);
                cb.execute(ret);
            });
            invocation.result = cb;
            next();
        }, except, index, info);
    };
    //注册返回promise的拦截器
    FunctionProxy.prototype.register_promise = function(before, after, except, index, info){
        throw 'not support';
        // this.invokeproxy.register(before, after, except, index);
    };
    return FunctionProxy;
})();

//代理工厂
var ProxyFactory = (function(){
    var factory = {};
    return {
        get : function(types){
            var list = types.split('|'), handlers = {};
            if(list){
                list.forEach(function(type){
                    if(factory[type]){
                        handlers[type] = factory[type];
                    }
                });
            } else {
                throw RangeError('type is not fount');
            }
            return handlers;
        },
        set : function(type, proxy){
            factory[type] = proxy;
        }
    };
}());
//只代理方法
ProxyFactory.set('apply', function(getFuncProxy){
    return function(target, thisArg, args, name, receiver){
        var proxy = getFuncProxy(name);
        if(proxy){
            proxy.setInvocation(new Invocation(receiver, thisArg, target));
            return proxy.run(args);
        } else {
            return target.apply(thisArg, args);
        }
    }
});
//只代理属性
ProxyFactory.set('get', function(getFuncProxy){
    return function(target, property, receiver){
        var proxy = getFuncProxy(property);
        if(proxy){
            proxy.setInvocation(new Invocation(receiver, target, function(){ return target[property]; }));
            return proxy.run();
        } else {
            return target[property];
        }
    }
});
//代理方法与属性
ProxyFactory.set('set', function(getFuncProxy){
    return function(target, property, value, receiver){
        var proxy = getFuncProxy(property);
        if(proxy){
            proxy.setInvocation(new Invocation(receiver, target, function(){ return target[property] = value; }));
            proxy.run(value);
        } else {
            target[property] = value;
        }
    }
});
//对象装饰者
function ObjectDecorator(obj, handlers){
    this.interceptorList = {};
    var list = {}, getFuncProxy;
    if(handlers){
        getFuncProxy = (function (self){
            return function(key){
                return self.interceptorList[key];
            };
        })(this);

        for (var i in handlers) {
            list[i] = handlers[i](getFuncProxy);
        }
    }
    this.proxy = new ObjectProxy(obj, list);
}
//注册拦截器
ObjectDecorator.prototype.interception = function(key, before, after, except, index, name){
    var interceptor = this.interceptorList[key] || (this.interceptorList[key] = new FunctionProxy());
    interceptor.register(before, after, except, index, name);
};
//注册拦截器
ObjectDecorator.prototype.interception_args = function(key, posi/*参数所在的位置*/, before, after, except, index, name){
    var interceptor = this.interceptorList[key] || (this.interceptorList[key] = new FunctionProxy());
    interceptor.register_args(posi, before, after, except, index, name);
};
//注册拦截器
ObjectDecorator.prototype.interception_cb = function(key, proxy, index, except, name){
    var interceptor = this.interceptorList[key] || (this.interceptorList[key] = new FunctionProxy());
    interceptor.register_cb(proxy, index, except, name);
};
ObjectDecorator.getObjectDecorator = function(obj, types){
    types = types || 'apply|get|set';
    var key = '__decortor__', cache = ObjectDecorator.cache || (ObjectDecorator.cache = {});
    if(obj[key]){
        if(!cache[obj[key]]){
            cache[obj[key]] = new ObjectDecorator(obj, /*默认的代理*/ProxyFactory.get(types));
        }
    } else {
        var id = new Identity();
        obj[key] = id.toString();
        cache[id] = new ObjectDecorator(obj, ProxyFactory.get(types));
    }
    return cache[obj[key]];
}


qv.zero.Http = ObjectDecorator.getObjectDecorator(qv.zero.Http).proxy;

ObjectDecorator.getObjectDecorator(qv.zero.Http).interception('send', function(inv, next){
        console.log('begin1');
        next();
    },
    function(inv, next){
        console.log('end1');
        next();
    },
    null, null, 'wo de 1'
);

ObjectDecorator.getObjectDecorator(qv.zero.Http).interception('send', function(inv, next){
        console.log('begin2');
        next();
    },
    function(inv, next){
        console.log('end2');
        next();
    },
    null, null, 'wo de 2'
);