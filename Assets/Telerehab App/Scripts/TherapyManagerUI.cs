using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TherapyManagerUI : MonoBehaviour
{
    public TextMeshProUGUI StatusText;

    public LoadingBar RecoveryFactorSlider;

    public LoadingBar DegreeOfAssistanceSlider;

    public void UpdateStatusText(TherapyManager therapyManager)
    {
        if(therapyManager.IsCalibrating)
        {

            StatusText.text = "Calibrating";
        } else
        {
            StatusText.text = "Training";
        }
    }

    public void AddCalibrationPercToStatus(float perc)
    {
        StatusText.text = "Calibrating (" + perc.ToString() + "%)";
    }

    public void UpdateReoveryFactorSlider(float recoveryFactor)
    {
        RecoveryFactorSlider.UpdateLoadingBarImage(recoveryFactor);
    }

    public void UpdateDegreeOfAssistanceSlider(float degOfAssistance)
    {
        DegreeOfAssistanceSlider.UpdateLoadingBarImage(degOfAssistance);
    }
}
