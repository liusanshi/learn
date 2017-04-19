function SetTimeout(steptime){
	this.stepTime = steptime || 2000;
}

SetTimeout.prototype.Do = function(func, time) {
	time = time || 0;
	if(typeof this.startTime === 'undefined'){
		this.startTime = new Date().getTime();
	}
	if(time <= this.stepTime){
		this.handle = setTimeout(func, time);
	} else {
		var curTime = new Date().getTime(), st, difftime = curTime - this.startTime,sleft = this;
		if(difftime < time){
			difftime = difftime === 0 ? this.stepTime : difftime;
			st = Math.min(difftime, this.stepTime);
			this.handle = setTimeout(function(){sleft.Do.call(sleft, func, time); sleft = null;}, time);
		} else {
			clearTimeout(this.handle);
			func();
		}
	}
	return this;
};
SetTimeout.prototype.Cancel = function() {
	clearTimeout(this.handle);
	return this;
};