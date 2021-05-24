using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harmony;
using UnityEngine;

[HarmonyPatch(typeof(CountermeasureManager), "Start")]
class Patch_CountermeasureManager_Start
{
    [HarmonyPostfix]
    static void Postfix(CountermeasureManager __instance)
    {
        if (CustomBetty.instance.currentCommonWarnings != null)
        {
            FlightWarnings.CommonWarningsClips bettyVoiceProfile = CustomBetty.instance.currentCommonWarnings;

            Debug.Log("Replacing cm sounds");
            __instance.chaffAnnounceClip = bettyVoiceProfile.Chaff;
            __instance.flareAnnounceClip = bettyVoiceProfile.Flare;
        }
    }
}