using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class IRMissileWarningReplacer : MonoBehaviour
{
    private void Start() {
        if (CustomBetty.instance.currentProfile != null)
        {
            CustomBetty.Profile voiceProfile = CustomBetty.instance.currentProfile;

            Debug.Log("Replacing IR missile warning");
            if (voiceProfile.irMissileIncoming != null)
            {
                GetComponent<IRMissileIncomingWarning>().audioSource.clip = voiceProfile.irMissileIncoming;
            }
        }
    }
}