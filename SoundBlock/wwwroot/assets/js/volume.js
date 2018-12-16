var Volume = function(options){
	var canvas = document.getElementById(options.canvas);
	var ctx    = canvas.getContext("2d");

	var delay  = 2;
	var volume = 0;
	var filter = 0;

	var cols = 2;
	var rows = 20;

	var offset = 3;


	this.setVolume = function(vol){
		if(vol > 0) volume = vol;
	}
	this.setFilter = function(fil){
		filter = fil;
	}

	this.update = function(delta){
		ctx.clearRect(0, 0, canvas.width, canvas.height);

		for (var x = 0; x < cols; x++){
			for (var y = 0; y < rows; y++){

				var w = canvas.width/cols - offset;
				var h = (canvas.height-2)/rows;
				var r = y / rows;
				var n = x == 0 ? volume : filter;

				var px = Math.round(x * w + (offset*2) * x);
				var py = Math.round(h * y);

				ctx.fillStyle = n >= 1 - r ? 'red' : 'rgb(255, 255, 255, 0.1)';
				ctx.fillRect(px, py, w ,1 );
			}
		}

		ctx.fillStyle = 'rgb(255, 255, 255, 0.1)';
		ctx.fillRect(0, canvas.height - 2, canvas.width, 2);

		volume -= delay * delta;
		filter -= delay * delta;



		if(volume < 0) volume = 0;
		if(filter < 0) filter = 0;
	}
}