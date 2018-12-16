var Bar = function(options){
	var rad2deg = 180/Math.PI;
	var deg = 0;
	var bars = $('.bar',options.elem);
	var perc = $('.bar-control-value',options.elem);

	var w = 6;
	var h = 2;
	
	for(var i=0;i<30;i++){
		deg = i*12;
	
		$('<div class="colorBar">').css({
			transform:'rotate('+deg+'deg)',
			top: -Math.sin(deg/rad2deg)*60+70-(h/2),
			left: Math.cos((180 - deg)/rad2deg)*60+70-(w/2),
		}).appendTo(bars);
	}
	
	var colorBars = bars.find('.colorBar');
	var numBars = 0, lastNum = -1;
	
	$('.bar-control',options.elem).knobKnob({
		snap : options.snap,
		value: options.value,
		turn : function(ratio){
			numBars = Math.round(colorBars.length*ratio);
			
			options.turn(ratio);
			
			if(numBars == lastNum){
				return false;
			}
			lastNum = numBars;

			perc.text(options.text ? options.text(ratio) : Math.round(ratio/1*100)+'%');
			
			colorBars.removeClass('active').css({backgroundColor: 'white'}).slice(0, numBars).addClass('active').css({backgroundColor: 'red'});
		}
	});
	
};
