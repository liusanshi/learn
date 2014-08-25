var http = require('http'),
	url = require('url'),
	fs = require('fs'),
	server, io, send404;

server = http.createServer(function (req, res) {
	var path = url.parse(req.url).pathname;

	switch(path){
		case '/':
			res.writeHead(200, {'Content-Type' : 'text/html'});
			res.write('<h1>Hello! Try the <a href="/index.html">Socket.io Test</a></h1>');
			res.end();
			break;
		case '/index.html':
			fs.readFile(__dirname + path, function(err, data){
				if(err) return send404(res);
				res.writeHead(200, {'Content-Type': 'text/html'});
				res.write(data, 'utf8');
				res.end();
			});
			break;
		default : 
			fs.readFile(__dirname + path, function (err, data) {
				if(err) return send404(res);
				res.writeHead(200, {'Content-Type': 'text/javascript'});
				res.write(data, 'utf8');
				res.end();
			})
	}
});

send404 = function(res){
	res.writeHead(404);
	res.write('404');
	res.end();
};

server.listen(8080);

send404.inc = 1;

//console.log(__dirname);

io = require('./node_modules/socket.io').listen(server);

io.sockets.on('connection', function(socket){
	console.log('connection' + socket.id + ' accepted.');

	console.log(send404.inc++);

	socket.on('message', function(msg){
		console.log('received message:' + msg + ' - from client ' + socket.id);

		// socket.emit('message', msg);//这个是给自己发消息

		// console.log(socket.broadcast);
		socket.broadcast.emit('message', msg); //这个是广播消息
	});

	socket.on('disconnect', function () {
		console.log('Connection ' + socket.id + 'terminated.');
	});
});

