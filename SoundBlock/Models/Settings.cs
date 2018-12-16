using System;

namespace SoundBlock
{
    public class Settings
    {
        public static Settings app = new Settings();

        public static float Level;


        

        
        public float Compensation = 0.5f;


        public int Delay = 200;

        public string AudioOutName = "chrome";


        public bool Threshold_use_between = false; 

        public float Threshold_min = 60;
        public float Threshold_max = 90;
        public float Velelup = 25;
        public float Threshold_from = 85;
        public float Multipli = 1;


        public string profileName = "def";
    }
}
