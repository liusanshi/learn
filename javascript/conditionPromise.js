/**
 * 条件Promise
 * Created by payneliu on 2017/3/28.
 *
 * promise 的if else elseif
 * new Promise(function(resolve, reject){
        setTimeout(function(){
            resolve({a:31, b: 35});
        }, 20);
    }).if(function(res){
        return res.a < 40;
    })  .if(function(res){
            return res.b < 30;
        }).then(function(data){
            console.log('a < 40 and b < 30', data);
            return Promise.resolve(data);
        }).else(function(data){
            console.log('a < 40 and b >= 30', data);
            return Promise.resolve(data);
        }).then(function(data){
            console.log('a < 40 and b >= 30 -2', data);
            return Promise.resolve(data);
        }).fi()
 .elseif(function(res){
        return res < 50;
    }).then(function(data){
        console.log(' < 50 - 1', data);
        return Promise.resolve(data);
    }).then(function(data){
        console.log(' < 50 - 2', data);
        return Promise.resolve(data);
    }).else(function(data){
        console.log(' else 1', data);
        return Promise.resolve(data);
    }).then(function(data){
        console.log(' else 2', data);
        return Promise.resolve(data);
    }).fi().catch(function(data){
        console.log(' catch 1', data);
    });

  //a < 40 and b >= 30 Object {a: 31, b: 35}
  //a < 40 and b >= 30 -2 Object {a: 31, b: 35}

 */

Define('util/conditionPromise', [], function (require, exports, module) {

    /**
     * 返回true
     * @returns {boolean}
     */
    function fTrue(){ return true; }

    /**
     * 空函数
     */
    function empty(){ }

    /**
     * 分支Promise
     * @param {function} condt 分支的条件
     * @constructor
     */
    function BranchPromise(condt){
        this._condition = condt || fTrue;
        this._promise = new Promise(function(resolve, reject){
            this._fulfill = function(res){ resolve(res) }; //执行
            //this._reject = function(res){ reject(res) }; //执行，不跑拒绝
        }.bind(this));
    }

    /**
     * 接入新的处理逻辑
     * @param {function} onFulfilled
     * @param {function} onRejected
     * @returns {BranchPromise}
     */
    BranchPromise.prototype.then = function(onFulfilled, onRejected){
        this._promise = this._promise.then(onFulfilled, onRejected);
        return this;
    };
    /**
     * 执行分支
     * @param res
     * @returns {boolean}
     */
    BranchPromise.prototype.run = function(res){
        if(this._condition(res)){
            this._fulfill(res);
            return true;
        } else {
            return false;
        }
    };

    /**
     * 条件Promise
     * @param {ConditionPromise} prev 前一个条件Promise
     * @constructor
     */
    function ConditionPromise(prev){
        this._prev = prev;
        this._branches = []; //分支
        this._curBranch = null; //当前分支
    }

    /**
     * 执行条件
     * @param res
     * @private
     */
    ConditionPromise.prototype._run = function(res){
        var index = 0;
        while(this._branches[index] && !this._branches[index++].run(res));
    };

    /**
     * 入栈条件
     * @param condt
     * @private
     */
    ConditionPromise.prototype._shift = function(condt){
        this._curBranch = new BranchPromise(condt);
        this._branches.push(this._curBranch);
    };
    /**
     * promise的then方法
     * @param fulfill
     * @param reject
     */
    ConditionPromise.prototype.then = function(fulfill, reject){
        this._curBranch.then(fulfill, reject);
        return this;
    };
    /**
     * elseif的判断
     * @param {function} condt
     * @returns {ConditionPromise}
     */
    ConditionPromise.prototype.elseif = function(condt){
        this._shift(condt);
        return this;
    };
    /**
     * else 的判断
     * @param {function} fulfill
     * @param {function} reject
     * @returns {ConditionPromise}
     */
    ConditionPromise.prototype.else = function(fulfill, reject){
        this._shift();
        if(fulfill){
            this._curBranch.then(fulfill, reject);
        }
        return this;
    };
    /**
     * 终结条件 相当于 endif
     * @returns {Promise}
     */
    ConditionPromise.prototype.fi = function(){
        return this._prev;
    };
    /**
     * 新开if
     * @param {function} condt
     * @returns {ConditionPromise}
     */
    ConditionPromise.prototype.if = function(condt){
        var branch = new BranchPromise();
        var cp = new ConditionPromise(this);
        cp._shift(condt);
        this.then(function(res){
            branch._fulfill(res);
            cp._run(res);
        }, empty);
        return cp;
    };

    /**
     * 在Promise上面挂在 if方法
     * @param {function} condt 条件
     * @returns {ConditionPromise}
     */
    Promise.prototype.if = function(condt){
        var cp = new ConditionPromise(this);
        cp._shift(condt);

        this.then(cp._run.bind(cp), empty);
        return cp;
    };

    module.exports = ConditionPromise;
});