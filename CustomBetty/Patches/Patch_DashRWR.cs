using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harmony;
using UnityEngine;

[HarmonyPatch(typeof(DashRWR), "Start")]
class Patch_DashRWR_Start
{
    [HarmonyPostfix]
    static void Postfix(DashRWR __instance)
    {
        if (CustomBetty.instance.currentProfile != null)
        {
            CustomBetty.Profile voiceProfile = CustomBetty.instance.currentProfile;

            Debug.Log("Replacing RWR");
            if (voiceProfile.blip != null)
            {
                __instance.radarBlip = voiceProfile.blip;
            }
            if (voiceProfile.irMissileIncoming != null)
            {
                __instance.lockBlip = voiceProfile.lockBlip;
            }
            if (voiceProfile.missileLoopLock != null)
            {
                __instance.missileLockLoopAudioSource.clip = voiceProfile.missileLoopLock;
            }
            if (voiceProfile.newContactBlip != null)
            {
                __instance.newContactBlip = voiceProfile.newContactBlip;
            }
        }
    }
}