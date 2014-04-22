var http = require('http'),
	querystring = require('querystring');

var contents = querystring.stringify({
	name : 'google',
	email : 'google@google.com',
	address : 'asdasdasdad'
});

var options = {
	host : 'www.google.com.hk',
	path: '/',
	method :'get'
}

var req = http.request(options, function(res){
	res.setEncoding('utf8')
	res.on('data', function (data) {
		console.log(data);
	});
});

req.end();