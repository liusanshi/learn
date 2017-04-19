
Function.prototype.bind = function (oThis) {
  if (typeof this !== "function") {
    // closest thing possible to the ECMAScript 5 internal IsCallable function
    throw new TypeError("Function.prototype.bind - what is trying to be bound is not callable");
  }

  var aArgs = Array.prototype.slice.call(arguments, 1), 
      fToBind = this, 
      fNOP = function () {},
      fBound = function () {
        return fToBind.apply(this instanceof fNOP && oThis
                               ? this
                               : oThis || window,
                             aArgs.concat(Array.prototype.slice.call(arguments)));
      };

  fNOP.prototype = this.prototype;
  fBound.prototype = new fNOP();

  return fBound;
};

function Class(){
  this.data = 1;
}
Class.prototype.show = function (){
  console.log(this.data);
}
Class.prototype.shows = function (x,y,z){
  console.log(x,y,z, this.data);
}

// var c = new Class();
// var b1 = c.shows.bind(c,1), b2 = b1.bind(c,2), b3 = b2.bind(c,3);
// b3.call(b1);

var ClassB = Class.bind(2);
var bb = new ClassB();
bb.show();