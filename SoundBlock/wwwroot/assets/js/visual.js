var VisualGrid = function(options){
	var canvas = document.getElementById(options.canvas);
	var ctx    = canvas.getContext("2d");

	var draw_lines = 10;
	var draw_nums = 10;

	this.update = function(){
		ctx.clearRect(0, 0, canvas.width, canvas.height);

		var lines = Math.round(canvas.height / draw_lines);

		ctx.strokeStyle = 'rgb(255, 255, 255, 0.05)';
		ctx.font = "10px Verdana";

		for (var i = draw_lines - 1; i >= 0; i--) {
			ctx.beginPath();

			var y = Math.round(lines * i)-0.5;

			ctx.moveTo(20, y);
			ctx.lineTo(canvas.width, y);

			ctx.stroke();

			ctx.fillStyle = '#ddd';
			ctx.fillText(100 - i*10,0,y);
		}
	}
}

var Visual = function(options){
	var canvas = document.getElementById(options.canvas);
	var ctx    = canvas.getContext("2d");

	var samples = [];

	this.set = function(volume){
		samples.push(volume);

		if(samples.length > options.samples){
			samples.shift();
		}
	}

	this.update = function(){
		ctx.strokeStyle = options.color;

		ctx.beginPath();

		for (var i = samples.length - 1; i >= 0; i--) {
			var v = samples[i];
			var f = canvas.width / options.samples;
			var c = 1 - v;


			if(i === 0) ctx.moveTo(f*i, canvas.height);
			else ctx.lineTo(f*i, canvas.height * c);

			ctx.stroke();
		}
	}
}