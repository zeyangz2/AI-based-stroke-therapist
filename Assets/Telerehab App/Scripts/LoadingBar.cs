using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingBar : MonoBehaviour
{
    public Image LoadingBarImage;

    public void UpdateLoadingBarImage(float value)
    {
        LoadingBarImage.fillAmount = value;
    }
}
