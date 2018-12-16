using CSCore.CoreAudioAPI;
using System.Threading;
using System.Collections.Generic;
using System;
using Microsoft.CodeAnalysis.Scripting;
using System.Linq;
using CSCore.Win32;

namespace SoundBlock
{
    public static class Normalizer
    {
        #region Normalizer
        static List<float> array_curent = new List<float>() { Capacity = 1200 };

        public static AudioEndpointVolume AudioVolume;
        public static ScriptRunner<float> runner;
        #endregion

        #region MonitorPeakValue
        public static void MonitorPeakValue()
        {
            using (var sessionManager = DefaultAudioSessionManager(DataFlow.Render))
            {
                try
                {
                    while (true)
                    {
                        Thread.Sleep(50);

                        // Что-то пошло не так
                        if (AudioVolume == null)
                            continue;

                        using (var sessionEnumerator = sessionManager.GetSessionEnumerator())
                        {
                            if (sessionEnumerator.IsError || sessionEnumerator.IsComError)
                                continue;

                            // Ошибка тут обычно
                            foreach (var session in sessionEnumerator)
                            {
                                if (session.IsError)
                                    continue;

                                using (var audioSessionControl2 = session.QueryInterface<AudioSessionControl2>())
                                {
                                    string name = audioSessionControl2.Process?.ProcessName;

                                    // Что-то пошло не так
                                    if (name == null || !name.Contains(Settings.app.AudioOutName)) {
                                        session.Dispose();
                                        continue;
                                    }
                                    
                                    while (true)
                                    {
                                        if (session.SessionState == AudioSessionState.AudioSessionStateExpired)
                                        {
                                            session.Dispose();
                                            break;
                                        }

                                        using (var audioMeterInformation = session.QueryInterface<AudioMeterInformation>())
                                        {
                                            var value = audioMeterInformation.GetPeakValue();

                                            if (Settings.Level == 0)
                                                Settings.Level = AudioVolume.MasterVolumeLevelScalar;

                                            if (value >= 0.01 && value <= 1)
                                            {
                                                array_curent.Add(value);
                                            }
                                            else
                                            {
                                                array_curent.Add(0.01f);
                                            }

                                            int max = Settings.app.Delay / 16; // 500 / 16 = 31

                                            if (array_curent.Count >= max)
                                            {
                                                int dif = array_curent.Count - max;
                                                array_curent.RemoveRange(0, dif);
                                            }

                                            //GraficHub.OnLog(session.SessionState.ToString());

                                            var data = new Globals()
                                            {
                                                Curent = Average(array_curent),
                                                Level = Settings.Level,
                                                Compensation = Settings.app.Compensation,
                                                Threshold_max = Settings.app.Threshold_max,
                                                Threshold_min = Settings.app.Threshold_min,
                                                Velelup = Settings.app.Velelup,
                                                Threshold_use_between = Settings.app.Threshold_use_between,
                                                Threshold_from = Settings.app.Threshold_from,
                                                Multipli = Settings.app.Multipli
                                            };

                                            GraficHub.Send(data.Curent, data.Level, value);

                                            float vol = GetVolumeNormal(data);
                                            vol = ((float)Math.Round(vol, 2));
                                            vol = vol > 1 ? 1 : vol;

                                            if (vol > 0.01 && vol <= 1)
                                                AudioVolume.MasterVolumeLevelScalar = vol;
                                        }

                                        Thread.Sleep(16);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (CoreAudioAPIException) { }
                catch (AccessViolationException) { }
                catch (Win32ComException) { }
                catch { }
            }
        }
        #endregion


        #region GetVolumeNormal / Average
        static float GetVolumeNormal(Globals data)
        {
            return runner(data).Result;
        }

        static float Average(IReadOnlyList<float> mass)
        {
            if (mass.Count == 0)
                return 0;

            return mass.Average();
        }
        #endregion

        #region DefaultAudioSessionManager
        static AudioSessionManager2 DefaultAudioSessionManager(DataFlow dataFlow)
        {
            using (var enumerator = new MMDeviceEnumerator())
            {
                using (var device = enumerator.GetDefaultAudioEndpoint(dataFlow, Role.Multimedia))
                {
                    // 
                    AudioVolume = AudioEndpointVolume.FromDevice(device);

                    //
                    return AudioSessionManager2.FromMMDevice(device);
                }
            }
        }
        #endregion
    }
}
