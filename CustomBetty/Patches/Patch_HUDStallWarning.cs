using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harmony;
using UnityEngine;

[HarmonyPatch(typeof(HUDStallWarning), "Start")]
class Patch_HUDStallWarning_Start
{
    [HarmonyPostfix]
    static void Postfix(HUDStallWarning __instance)
    {
        if (CustomBetty.instance.currentProfile != null)
        {
            CustomBetty.Profile voiceProfile = CustomBetty.instance.currentProfile;

            Debug.Log("Replacing stall warning");
            if (voiceProfile.stallWarning != null)
            {
                __instance.warningClip = voiceProfile.stallWarning;
            }
        }
    }
}