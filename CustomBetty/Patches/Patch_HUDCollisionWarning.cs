using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harmony;
using UnityEngine;

[HarmonyPatch(typeof(HUDCollisionWarning), "Start")]
class Patch_HUDCollisionWarning_Start
{
    [HarmonyPostfix]
    static void Postfix(HUDCollisionWarning __instance)
    {
        if (CustomBetty.instance.currentProfile != null)
        {
            CustomBetty.Profile voiceProfile = CustomBetty.instance.currentProfile;

            Debug.Log("Replacing collision warning");
            if (voiceProfile.collisionWarning != null)
            {
                __instance.warningSound = voiceProfile.collisionWarning;
            }
        }
    }
}