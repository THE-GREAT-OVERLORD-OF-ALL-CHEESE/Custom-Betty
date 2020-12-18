using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using Harmony;
using System.Reflection;

namespace CustomBetty
{
    public class CustomBetty : VTOLMOD
    {
        public class Profile
        {
            public string filePath;
            public string name;
            public List<LineGroup> lineGroups;

            public FlightWarnings.CommonWarningsClips GenerateBettyVoiceProfile()
            {
                FlightWarnings.CommonWarningsClips output = new FlightWarnings.CommonWarningsClips();
                for (int i = 0; i < lineGroups.Count; i++)
                {
                    AudioClip temp = lineGroups[i].GenerateMessageAudio();
                    switch (lineGroups[i].type)
                    {
                        case FlightWarnings.CommonWarnings.EngineFailure:
                            output.EngineFailure = temp;
                            break;
                        case FlightWarnings.CommonWarnings.LeftEngineFailure:
                            output.LeftEngineFailure = temp;
                            break;
                        case FlightWarnings.CommonWarnings.RightEngineFailure:
                            output.RightEngineFailure = temp;
                            break;
                        case FlightWarnings.CommonWarnings.APUFailure:
                            output.APUFailure = temp;
                            break;
                        case FlightWarnings.CommonWarnings.HydraulicsFailure:
                            output.HydraulicsFailure = temp;
                            break;
                        case FlightWarnings.CommonWarnings.Chaff:
                            output.Chaff = temp;
                            break;
                        case FlightWarnings.CommonWarnings.ChaffLow:
                            output.ChaffLow = temp;
                            break;
                        case FlightWarnings.CommonWarnings.ChaffEmpty:
                            output.ChaffEmpty = temp;
                            break;
                        case FlightWarnings.CommonWarnings.Flare:
                            output.Flare = temp;
                            break;
                        case FlightWarnings.CommonWarnings.FlareLow:
                            output.FlareLow = temp;
                            break;
                        case FlightWarnings.CommonWarnings.FlareEmpty:
                            output.FlareEmpty = temp;
                            break;
                        case FlightWarnings.CommonWarnings.BingoFuel:
                            output.BingoFuel = temp;
                            break;
                        case FlightWarnings.CommonWarnings.Altitude:
                            output.Altitude = temp;
                            break;
                        case FlightWarnings.CommonWarnings.PullUp:
                            output.PullUp = temp;
                            break;
                        case FlightWarnings.CommonWarnings.OverG:
                            output.OverG = temp;
                            break;
                        case FlightWarnings.CommonWarnings.MissileLaunch:
                            output.MissileLaunch = temp;
                            break;
                        case FlightWarnings.CommonWarnings.Missile:
                            output.Missile = temp;
                            break;
                        case FlightWarnings.CommonWarnings.Shoot:
                            output.Shoot = temp;
                            break;
                        case FlightWarnings.CommonWarnings.Pitbull:
                            output.Pitbull = temp;
                            break;
                        case FlightWarnings.CommonWarnings.Warning:
                            output.Warning = temp;
                            break;
                        case FlightWarnings.CommonWarnings.Fire:
                            output.Fire = temp;
                            break;
                        case FlightWarnings.CommonWarnings.FuelLeak:
                            output.FuelLeak = temp;
                            break;
                        case FlightWarnings.CommonWarnings.FuelDump:
                            output.FuelDump = temp;
                            break;
                        case FlightWarnings.CommonWarnings.LandingGear:
                            output.LandingGear = temp;
                            break;
                        case FlightWarnings.CommonWarnings.AutopilotOff:
                            output.AutopilotOff = temp;
                            break;
                        case FlightWarnings.CommonWarnings.WingFold:
                            output.WingFold = temp;
                            break;
                        default:
                            break;
                    }
                }
                return output;
            }

            public void GetFilePaths()
            {
                lineGroups = new List<LineGroup>();

                Debug.Log("Checking for: " + filePath);

                if (Directory.Exists(filePath))
                {
                    Debug.Log(filePath + " exists!");
                    DirectoryInfo info = new DirectoryInfo(filePath);
                    foreach (FlightWarnings.CommonWarnings messageType in Enum.GetValues(typeof(FlightWarnings.CommonWarnings)))
                    {
                        Debug.Log("Checking for: " + messageType.ToString());
                        if (Directory.Exists(filePath + messageType.ToString() + @"\"))
                        {
                            Debug.Log("Found: " + messageType.ToString());
                            LineGroup temp = new LineGroup();
                            temp.filePath = filePath + messageType.ToString() + @"\";
                            temp.type = messageType;
                            temp.GetFilePaths();
                            lineGroups.Add(temp);
                            Debug.Log("\n");
                        }
                        else
                        {
                            Debug.Log(filePath + messageType.ToString() + " doesn't exist, please add it or the voicepack will not work as intended.");
                        }
                    }
                }
                else
                {
                    Debug.Log(filePath + " doesn't exist.");
                }
            }
        }

        public class LineGroup
        {
            public string filePath;
            public FlightWarnings.CommonWarnings type;
            public string clipPath;
            public AudioClip clip;

            public AudioClip GenerateMessageAudio()
            {
                return clip;
            }

            public void GetFilePaths()
            {
                Debug.Log("Checking for: " + filePath);

                if (Directory.Exists(filePath))
                {
                    Debug.Log(filePath + " exists!");
                    DirectoryInfo info = new DirectoryInfo(filePath);
                    foreach (FileInfo item in info.GetFiles("*.wav"))
                    {
                        Debug.Log("Found line: " + item.Name);
                        clipPath = filePath + item.Name;
                        Debug.Log("\n");
                    }
                }
                else
                {
                    Debug.Log(filePath + " doesn't exist.");
                }
            }
        }

        public string address;
        public List<Profile> profiles;
        public List<FlightWarnings.CommonWarningsClips> bettyVoiceProfiles;

        public override void ModLoaded()
        {
            base.ModLoaded();

            bettyVoiceProfiles = new List<FlightWarnings.CommonWarningsClips>();
            VTOLAPI.SceneLoaded += SceneLoaded;
            VTOLAPI.MissionReloaded += MissionReloaded;

            LoadCustomWingmen();
        }

        void SceneLoaded(VTOLScenes scene)
        {
            switch (scene)
            {
                case VTOLScenes.Akutan:
                case VTOLScenes.CustomMapBase:
                    StartCoroutine("SetupScene");
                    break;
                default:
                    break;
            }
        }

        private void MissionReloaded()
        {
            StartCoroutine("SetupScene");
        }

        private IEnumerator SetupScene()
        {
            while (VTMapManager.fetch == null || !VTMapManager.fetch.scenarioReady || FlightSceneManager.instance.switchingScene)
            {
                yield return null;
            }

            if (bettyVoiceProfiles.Count > 0)
            {
                Debug.Log("Replacing betty!");

                FlightWarnings.CommonWarningsClips bettyVoiceProfile = bettyVoiceProfiles[UnityEngine.Random.Range(0, bettyVoiceProfiles.Count)];

                FlightWarnings[] bettys = UnityEngine.Object.FindObjectsOfType<FlightWarnings>();
                foreach (FlightWarnings betty in bettys)
                {
                    Traverse traverse = new Traverse(betty);

                    betty.commonWarningsClips = bettyVoiceProfile;
                    traverse.Field("cwp").SetValue(betty.commonWarningsClips.ToArray());
                }

                CountermeasureManager[] cms = UnityEngine.Object.FindObjectsOfType<CountermeasureManager>();
                foreach (CountermeasureManager cm in cms)
                {
                    cm.chaffAnnounceClip = bettyVoiceProfile.Chaff;
                    cm.flareAnnounceClip = bettyVoiceProfile.Flare;
                }
            }
            else {
                Debug.Log("There are no betty voice packs, cannot replace betty...");
            }
        }

        private void LoadCustomWingmen()
        {
            profiles = new List<Profile>();

            address = Directory.GetCurrentDirectory() + @"\VTOLVR_ModLoader\mods\";
            Debug.Log("Checking for: " + address);

            if (Directory.Exists(address))
            {
                Debug.Log(address + " exists!");
                DirectoryInfo info = new DirectoryInfo(address);
                foreach (DirectoryInfo item in info.GetDirectories())
                {
                    try
                    {
                        Debug.Log("Checking for: " + address + item.Name + @"\bettyvoiceinfo.txt");
                        string temp = File.ReadAllText(address + item.Name + @"\bettyvoiceinfo.txt");
                        Debug.Log("Found betty voice pack: " + temp);

                        Profile tempProfile = new Profile();
                        tempProfile.name = temp;
                        tempProfile.filePath = address + item.Name + @"\";
                        tempProfile.GetFilePaths();
                        profiles.Add(tempProfile);
                    }
                    catch
                    {
                        Debug.Log(item.Name + " is not an Betty voice pack.");
                    }
                    Debug.Log("\n");
                }
            }
            else
            {
                Debug.Log(address + " doesn't exist.");
            }
            Debug.Log("Loading audioClips");
            StartCoroutine(LoadAudioFile());
        }

        private void MakeBettyVoiceProfiles()
        {
            for (int i = 0; i < profiles.Count; i++)
            {
                bettyVoiceProfiles.Add(profiles[i].GenerateBettyVoiceProfile());
            }
        }

        IEnumerator LoadAudioFile()
        {
            for (int y = 0; y < profiles.Count; y++)
            {
                for (int x = 0; x < profiles[y].lineGroups.Count; x++)
                {
                    using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(profiles[y].lineGroups[x].clipPath, AudioType.WAV))
                    {
                        yield return www.Send();

                        if (www.isNetworkError)
                        {
                            Debug.Log(www.error);
                        }
                        else
                        {
                            profiles[y].lineGroups[x].clip = DownloadHandlerAudioClip.GetContent(www);
                        }
                    }
                    Debug.Log("Loaded " + profiles[y].lineGroups[x].clipPath);
                }
            }
            MakeBettyVoiceProfiles();
        }
    }
}
