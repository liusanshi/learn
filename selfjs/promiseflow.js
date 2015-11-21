/*
 * promise列表
 */
 function PromiseList(list){
     this.list = list || [];
     this.index = 0;
 }
 PromiseList.prototype = Object.create({
     next : function(){
         var count = this.list.length;
         if(this.index >= count){
             return null;
         }
         return this.list[this.index++];
     },
     append : function(promise){
         this.list.push(promise);
     }
 });
 PromiseList.prototype.constructor = PromiseList;
 
 function PromiseFlow(){
     this.list = new PromiseList();
     this.pause_state = false;
 }
  PromiseFlow.prototype = Object.create({
     add : function(promise){
         this.list.append(promise);
     },
     exec : function(){
         var promise, me = this;
         if(!this.pause_state && (promise = this.list.next())){
             this._execPromise(promise, function(args){
                 me.cur_arg = args;
                 me.exec();
             });
         }
     },
     _execPromise : function(promise, cb){
         var method;
         if(typeof promise === 'function'){
             promise = promise(this.cur_arg);
         }
         method = promise.then || promise.add || promise.done;
         if(method){
             method.call(promise, function(args){
                 cb(args);
             })
         }
     },
     pause : function(){
         this.pause_state = true;
     },
     restart : function(){
         this.pause_state = false;
         this.exec();
     }
 });
 PromiseFlow.prototype.constructor = PromiseFlow;