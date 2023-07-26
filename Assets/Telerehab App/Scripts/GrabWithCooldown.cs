using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabWithCooldown : MonoBehaviour
{
    public float coolDownPeriod;
    public HapticPlugin hapticPlugin;
    public bool CanGrab = true;

    public void Grab()
    {
        if (!CanGrab)
        {
            return;
        }
        hapticPlugin.Grab_Object();
    }

    public void Release()
    {
        hapticPlugin.Release_Object();
        StartCoroutine(CoolDownProcess());
    }

    IEnumerator CoolDownProcess()
    {
        CanGrab = false;
        yield return new WaitForSeconds(coolDownPeriod);
        CanGrab = true;
    }
}
