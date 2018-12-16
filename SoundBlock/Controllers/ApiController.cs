using System;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace SoundBlock.Controllers
{
    public class ApiController : Controller
    {
        public JsonResult UpdateProfile(float Compensation, int Delay, string AudioOutName, bool Threshold_use_between, float Threshold_min, float Threshold_max, float Velelup, float Threshold_from, float Multipli, string profileName = "def")
        {
            try
            {
                Settings.app.Compensation = Compensation;
                Settings.app.Delay = Delay;
                Settings.app.AudioOutName = AudioOutName;
                Settings.app.Threshold_use_between = Threshold_use_between;
                Settings.app.Threshold_min = Threshold_min;
                Settings.app.Threshold_max = Threshold_max;
                Settings.app.Velelup = Velelup;
                Settings.app.Threshold_from = Threshold_from;
                Settings.app.profileName = profileName;
                Settings.app.Multipli = Multipli;

                System.IO.File.WriteAllText("profiles/" + Settings.app.profileName + ".json", JsonConvert.SerializeObject(Settings.app));
            }
            catch { }

            return Json(Settings.app);
        }

        public JsonResult LoadProfile(string name)
        {
            string file = "profiles/" + name + ".json";
            if (!System.IO.File.Exists(file) || name == null)
                return Json(Settings.app);

            Settings.app = JsonConvert.DeserializeObject<Settings>(System.IO.File.ReadAllText("profiles/" + name + ".json"));
            return Json(Settings.app);
        }


        public JsonResult profiles()
        {
            return Json(Directory.CreateDirectory("profiles/").GetFiles().Select(i => i.Name?.Replace(".json", "")));
        }


        public bool UpdateLevel(string code)
        {
            if (Normalizer.AudioVolume != null)
            {
                Settings.Level = code == "F2" ? (Settings.Level - 0.02f) : (Settings.Level + 0.02f);
                return true;
            }

            return false;
        }
    }
}
