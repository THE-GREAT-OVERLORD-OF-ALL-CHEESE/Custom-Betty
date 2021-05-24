using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harmony;
using UnityEngine;

[HarmonyPatch(typeof(IRMissileIncomingWarning), "Update")]
class Patch_IRMissileIncomingWarning_Update
{
    [HarmonyPostfix]
    static void Postfix(IRMissileIncomingWarning __instance)
    {
        if (__instance.gameObject.GetComponent<IRMissileWarningReplacer>() == null) {
            __instance.gameObject.AddComponent<IRMissileWarningReplacer>();
        }
    }
}