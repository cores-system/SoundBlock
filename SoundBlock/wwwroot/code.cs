//Debug("text"); // ���� ����� ���� ���� ����� ����� � ������� �����))

bool op_threshold_use_between = Threshold_use_between; //������������� ��������)

float op_threshold_min = Threshold_min; //���, ��� �� �������� ���� ������ �����
float op_threshold_max = Threshold_max; //������������ ����� 
float op_threshold_from = Threshold_from; //���� ������� �����
float op_velelup = Velelup; //�� �������� ������� ������ �����



float vol_curent = Curent;
float vol_level = 1;



//����� ����� ���� �� ���� op_velelup
float levelup = (op_velelup / 100) - vol_curent;
      levelup = levelup< 0 ? 0 : levelup;
	  
	  levelup *= Multipli;

vol_level = vol_level + vol_level* levelup;


//���������
float threshold = 0;
float threshold_min = op_threshold_min / 100;
float threshold_max = op_threshold_max / 100;


float threshold_bet = threshold_min + ((threshold_max - threshold_min) / 2);

//���� ������������ ��������� �����
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


