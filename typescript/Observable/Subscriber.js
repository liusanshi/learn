if (typeof Object.create != 'function') {
  // Production steps of ECMA-262, Edition 5, 15.2.3.5
  // Reference: http://es5.github.io/#x15.2.3.5
  Object.create = (function() {
    //为了节省内存，使用一个共享的构造器
    function Temp() {}

    // 使用 Object.prototype.hasOwnProperty 更安全的引用 
    var hasOwn = Object.prototype.hasOwnProperty;

    return function (O) {
      // 1. 如果 O 不是 Object 或 null，抛出一个 TypeError 异常。
      if (typeof O != 'object') {
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

function Subscriber(destination) {
    this.isStopped = false;
    this.destination = destination;
}
Subscriber.prototype = Object.create({
    next : function (data) {
        if(!this.isStopped){
            this._exec('next', [data]);
        }
    },
    error : function (err) {
        if(!this.isStopped){
            this.isStopped = true;
            this._exec('error', [err]);
        }
    },
    complete : function () {
        if(!this.isStopped){
            this.isStopped = true;
            this._exec('complete', []);
        }
    },
    _exec : function(cmd, data){
        var func
        if((func = this.destination[cmd])){
            func.apply(this.destination, data);
        }
    }
});

function Observable(){
    
}
Observable.prototype = Object.create({
    subscribe : function(){
        
    }
});