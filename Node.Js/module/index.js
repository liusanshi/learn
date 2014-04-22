
function footbar () {
	this.foo = function(){
		console.log('Hello footbar')
	};
	this.bar = function(){
		console.log('Hello bar')
	}
}

exports.footbar = footbar;