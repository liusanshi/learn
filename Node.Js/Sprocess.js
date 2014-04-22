console.log(process.argv);

console.log(process.execPath)

/*
//process 学习
process.stdin.resume();

process.stdin.on('data', function (data) {
	process.stdin.write('read from console : ' + data.toString())
})
process.stdin.on('error' , function(err){
	process.stdin.write('read from console : ' + err.toString())
})

*/

//继承

var util = require('util')

	function Base() {
		this.name = 'base'
		this.base = 1991

		this.sayHello = function() {
			console.log('Hello: ' + this.name)
		}
	}

Base.prototype.showName = function() {
	console.log(this.name);
}

function Sub() {
	this.name = 'sub';
}

util.inherits(Sub, Base);

var objbase = new Base()
objbase.sayHello()
objbase.showName();
console.log(objbase)

var objsub = new Sub()
objsub.showName();
//objsub.sayHello()
console.log(objsub)