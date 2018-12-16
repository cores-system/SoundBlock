var TestSound = function(options){
	var context  = new AudioContext();

	var gain     = context.createGain();

	var analyser = context.createAnalyser();
		analyser.fftSize = 1024;

	var sampleBuffer = new Float32Array(analyser.fftSize);

	var soundBuffer;
	var source;



	var request = new XMLHttpRequest();

		request.open('GET', options.sound, true);

		request.responseType = 'arraybuffer';

		request.onload = function() {
			context.decodeAudioData(request.response, function(theBuffer) {
				soundBuffer = theBuffer;
			});
		}

		request.send();



	function playSound(buffer) {
		source = context.createBufferSource();
		source.buffer = buffer;

		source.start(0);

		gain.gain.value = 1;

		source.connect(analyser);
		source.connect(gain);

		gain.connect(context.destination);
	}

	function getdB(){
		analyser.getFloatTimeDomainData(sampleBuffer);

		//получаем пик
		var peek = 0;

		for (let i = 0; i < sampleBuffer.length; i++) {
			peek = Math.max(sampleBuffer[i] ** 2, peek);
		}

		return 10 * Math.log10(peek);
	}

	function getVolume(){
		var db    = Math.abs(getdB());
		var dbMax = 40;

		var level = 1 - (db / dbMax);

		return isFinite(level) ? level : 0;
	}

	this.getVolume = function(){
		return getVolume();
	}

	this.play = function(){
	    playSound(soundBuffer);
	}
}