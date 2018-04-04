var http = require('http'),
	url = require('url'),
	fs = require('fs'),
	filepath = './module/'

http.createServer(function (req, res) {
	console.log(req.url)
	var path = url.parse(req.url)
	console.log(path)

// debugger
// console.log(filepath + path.pathname);
// 	fs.readFile(filepath + path.pathname, 'utf8', function(err, file){
// 		if(err){
// 			console.log(err)
// 			res.writeHead(404, {'Content-Type' : 'text/plain'})
// 			res.end()
// 			return;
// 		}
// 		res.writeHead(200, {'Content-Type' : 'text/html'});
// 		res.write(file);
// 		res.end();
// 	})
// 	return;
	
	//res.writeHead(200,{'Content-Type' : 'text/plain'});
	switch(path.pathname){
		case '/index':
			//res.end('I am index. \n')
			res.write('I am index. \n')
			break;
		case '/test':
			//res.end('this is test page. \n')
			res.write('this is test page. \n')
			break;
		default :
			//res.end('default page. \n');
			res.write('default page. \n');
			break;
	}
	
	//res.end('hello world\n')
}).listen(1334, '127.0.0.1');

console.log('server running at http://127.0.0.1:1334/')
