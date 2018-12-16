using System;

namespace SoundBlock
{
    public class Globals
    {
        public float Level;
        public float Curent;
        public float Compensation;

        public float Threshold_min;
        public float Threshold_max;
        public float Velelup;
        public float Threshold_from;
        public bool Threshold_use_between;
        public float Multipli;

        public static float normalize_up;


        public static void Debug(object text)
        {
            GraficHub.OnLog(text.ToString());
        }
    }
}
