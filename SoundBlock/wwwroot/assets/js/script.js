function filter(vol_curent){
		
	var vol_level = 1;


	//подем звука если он ниже op_velelup
	var levelup = (options.velelup / 100) - vol_curent;
         levelup = levelup < 0 ? 0 : levelup;

    levelup *= options.multipli;

	vol_level = vol_level + vol_level * levelup;


	//глушитель
	var threshold = 0;
	var threshold_min = options.threshold_min / 100;
	var threshold_max = options.threshold_max / 100;

	var threshold_bet = threshold_min + ((threshold_max - threshold_min) / 2);

	//если используется глушитель между
	if(options.threshold_use_between){
		if(vol_curent > threshold_bet){
			threshold = threshold_max - vol_curent;
		}
		else{
			threshold = vol_curent - threshold_min;
		}
	}
	else{
		threshold = vol_curent - (options.threshold_from / 100);
	}

    threshold = threshold < 0 ? 0 : threshold;

    threshold *= options.multipli;
    
	vol_level = vol_level - vol_level * threshold;

	return {
		level: vol_level,
		threshold: threshold,
		levelup: levelup
	}

	return vol_level;
}

function putTo(arr, volume, len){
	arr.push(volume);

	if(arr.length > len){
		arr.splice(0,arr.length - len);
	}
}

function getAverageVolume(array) {
	var values = 0;
	var average;

	var length = array.length;

	// get all the frequency amplitudes
	for (var i = 0; i < length; i++) {
		values += array[i];
	}

	average = values / length;
	return average;
}

	
	
function Init(){
	$('body').css({opacity:1})

	new Bar({
		elem: '#bar-volume-level',
		snap: 10,
		value: 180 * options.compensation,
		turn: function(ration){
			options.compensation = 2 * ration;
			
			saveSettings();
		},
		text: function(ration){
			return Math.round(200*(0.5-(1-ration))) + '%';
		}
	})

	new Bar({
		elem: '#bar-volume-delay',
		snap: 10,
		value: 360*(options.delay/1000),
		turn: function(ration){
			options.delay = Math.round(1000 * ration);
			
			saveSettings();
		},
		text: function(ration){
			return Math.round(1000*ration) + 'ms';
		}
    })

    new Bar({
        elem: '#bar-volume-multipli',
        snap: 5,
        value: 180 * options.multipli,
        turn: function (ration) {
            options.multipli = 2 * ration;

            saveSettings();
        },
        text: function (ration) {
            return Math.round(200 * (0.5 - (1 - ration))) + '%';
        }
    })

    


	var volumeDown = new Volume({
		canvas: 'volume-down'
	})

	var volumeUp = new Volume({
		canvas: 'volume-up'
	})




	var visual = {
		canvas: 'volume-visual',
		samples: 256
	};

	visual.grid = new VisualGrid({
		canvas: visual.canvas,
	});

	visual.original = new Visual({
		canvas: visual.canvas,
		samples: visual.samples,
		color: 'red'
	});

	visual.edit = new Visual({
		canvas: visual.canvas,
		samples: visual.samples,
		color: '#ff5700'
	});

	visual.levelup = new Visual({
		canvas: visual.canvas,
		samples: visual.samples,
		color: '#a8d718'
	});

	visual.threshold = new Visual({
		canvas: visual.canvas,
		samples: visual.samples,
		color: '#e73164'
	});


	var doom = {
		threshold_use_between:         $('#threshold_use_between'),

		display_threshold_between:     $('#display-threshold-between'),
		display_threshold:             $('#display-threshold'),

		range_threshold_between:       $('#range-threshold-between'),
		range_threshold:               $('#range-threshold'),

		velelup:                       $("#velelup")
	}

	function showSwithThreshold(){
		if(options.threshold_use_between){
			doom.display_threshold_between.show();
			doom.display_threshold.hide();
		}
		else{
			doom.display_threshold_between.hide();
			doom.display_threshold.show();
		}
	}


	doom.range_threshold_between.ionRangeSlider({
	    type: "double",

	    min: 0,
	    max: 100,

	    from: options.threshold_min,
	    from_min: 30,
	    from_max: 60,

	    to: options.threshold_max,
	    to_min: 70,
	    to_max: 100,

	    grid: true,
	    grid_num: 10,

	    hide_min_max: true,
	    hide_from_to: false,

	    onChange: function (data) {
	        options.threshold_min = data.from;
	        options.threshold_max = data.to;
			
			saveSettings();
	    },
	});

	doom.range_threshold.ionRangeSlider({
	    type: "double",

	    min: 0,
	    max: 100,

	    from: options.threshold_from,
	    from_min: 30,
	    from_max: 95,

	    to: 100,
	    to_min: 100,
	    to_max: 100,

	    grid: true,
	    grid_num: 10,

	    hide_min_max: true,
	    hide_from_to: false,

	    onChange: function (data) {
	        options.threshold_from = data.from;
			
			saveSettings();
	    },
	});

	doom.velelup.ionRangeSlider({
	    min: 0,
	    max: 100,

	    from: options.velelup,
	    from_max: 50,

	    grid: true,
	    grid_num: 10,

	    hide_min_max: true,
	    hide_from_to: false,

	    onChange: function (data) {
	        options.velelup = data.from;
			
			saveSettings();
	    },
	});

	doom.threshold_use_between.on('click', function(){
		$(this).toggleClass('active');

		options.threshold_use_between = $(this).hasClass('active');

		showSwithThreshold();

		saveSettings();
	})

	if(options.threshold_use_between) doom.threshold_use_between.addClass('active');

	showSwithThreshold();
	



	function capitalizeFirstLetter(string) {
	    return string.charAt(0).toUpperCase() + string.slice(1);
	}
	
	var settingTimer;
	
	function saveSettings(){
		clearTimeout(settingTimer);
		
		settingTimer = setTimeout(function(){
			
			var a = {};
			
			for(var i in options){
				a[capitalizeFirstLetter(i)] = options[i] + '';
			}
			
			var str = JSON.stringify(a);
				str = str.replace(/\./g,',');
				
			var json = JSON.parse(str);


			$.post('api/UpdateProfile',json);
		},1000)
	}

	var array_delay = [];

	var timeLast = Date.now();
	var timeNow;
	var timeDelta;
	
	function update(){
		timeNow   = Date.now();
	    timeDelta = (timeNow - timeLast)/1000;
	    timeLast  = timeNow;

		requestAnimationFrame(update);

		var vol = getAverageVolume(VOL.curent);

		var filtred = filter(getAverageVolume(VOL.limiter));

		volumeDown.setVolume(vol)
		volumeUp.setVolume(vol)
		volumeDown.setFilter(filtred.threshold)
		volumeUp.setFilter(filtred.levelup)


		visual.original.set(vol)
		visual.edit.set(vol*filtred.level)
		visual.levelup.set(filtred.levelup)
		visual.threshold.set(filtred.threshold)

		volumeDown.update(timeDelta);
		volumeUp.update(timeDelta);

		visual.grid.update(timeDelta);
		visual.original.update(timeDelta);
		visual.threshold.update(timeDelta);
		visual.levelup.update(timeDelta);
		visual.edit.update(timeDelta);
	}

	update();
};


