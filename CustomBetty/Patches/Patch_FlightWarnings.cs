using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harmony;
using UnityEngine;

[HarmonyPatch(typeof(FlightWarnings), "Awake")]
class Patch_FlightWarnings_Awake
{
    [HarmonyPostfix]
    static void Postfix(FlightWarnings __instance)
    {
        if (CustomBetty.instance.currentCommonWarnings != null) {
            Debug.Log("Replacing flight warnings");
            Traverse traverse = new Traverse(__instance);

            __instance.commonWarningsClips = CustomBetty.instance.currentCommonWarnings;
            traverse.Field("cwp").SetValue(__instance.commonWarningsClips.ToArray());
        }
    }
}