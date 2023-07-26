using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSpringSetter : MonoBehaviour
{
    public HapticPlugin HapticPlugin;
    public GameObject plate;

    private void Start()
    {
        SetSpring();
    }
    public void SetSpring()
    {
        HapticPlugin.SpringAnchorObj = plate;
        HapticPlugin.SpringGMag = .02f;
        HapticPlugin.EnableSpring();
    }
}
