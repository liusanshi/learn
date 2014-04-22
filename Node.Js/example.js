//example.js

console.log('evaluating example.js')

var invisible = function () {
	console.log('invisible');
}

exports.message = 'hi';

exports.say = function(){
	console.log(this.message);
}