//main.jS

var exp = require('./example.js')
var exp2 = require('./funexports.js')
var exp3 = require('./module').footbar,
fb = new exp3(),
fs = require('fs'),
file = './module/index.html',
encode = 'utf8';

var EventEmitter = require('events').EventEmitter,
    event = new EventEmitter();

exp.say()

exp2('你好!!');

fb.foo()
fb.bar()

debugger

fs.readFile(file, encode, function (err, f) {
	console.log(err);
	console.log(f)
})

//console.log('....\r\n' + fs.readFileSync(file, encode));


event.on('some_event', function(){
	console.log('some_event occured.')
})

setTimeout(function () {
	event.emit('some_event')
}, 1000);
