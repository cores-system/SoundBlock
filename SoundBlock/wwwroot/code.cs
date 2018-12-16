//Debug("text"); // Типа дебаг если надо будет потом в консоль круто))

bool op_threshold_use_between = Threshold_use_between; //переключатель замутить)

float op_threshold_min = Threshold_min; //нус, тут из настроек нуно тянуть порог
float op_threshold_max = Threshold_max; //максимальный порог 
float op_threshold_from = Threshold_from; //надо завести такую
float op_velelup = Velelup; //из настроек поднять низкие звуки



float vol_curent = Curent;
float vol_level = 1;



//подем звука если он ниже op_velelup
float levelup = (op_velelup / 100) - vol_curent;
      levelup = levelup< 0 ? 0 : levelup;
	  
	  levelup *= Multipli;

vol_level = vol_level + vol_level* levelup;


//глушитель
float threshold = 0;
float threshold_min = op_threshold_min / 100;
float threshold_max = op_threshold_max / 100;


float threshold_bet = threshold_min + ((threshold_max - threshold_min) / 2);

//если используется глушитель между
if (op_threshold_use_between)
{
    if (vol_curent > threshold_bet)
    {
        threshold = threshold_max - vol_curent;
    }
    else
    {
        threshold = vol_curent - threshold_min;
    }
}
else
{
    threshold = vol_curent - (op_threshold_from / 100);
}

threshold = threshold< 0 ? 0 : threshold;

threshold *= Multipli;

vol_level = vol_level - vol_level* threshold;


vol_level *= Level;

vol_level *= Compensation;


return vol_level;


