using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TherapyManager : MonoBehaviour
{
    public CalibrationSession CalibrationSession;
    public TrainingSession TrainingSession;

    public bool IsCalibrating;

    public UnityEvent<TherapyManager> OnCalibrationStart = new UnityEvent<TherapyManager>();
    public UnityEvent<TherapyManager> OnTrainingStart = new UnityEvent<TherapyManager>();

   

    public void StartCalibrationSession()
    {
        TrainingSession.StopTrainingSession();
        CalibrationSession.InitializeCalibrationSession();
        IsCalibrating = true;
        OnCalibrationStart.Invoke(this);
    }

    public void StartTrainingSession()
    {
        CalibrationSession.StopCalibrationSession();
        TrainingSession.InitializeTrainingSession(CalibrationSession.ErrorMetric);
        IsCalibrating = false;

        OnTrainingStart.Invoke(this);
    }

    public void StartTrainingSession(float RecoveryNot)
    {
        CalibrationSession.StopCalibrationSession();
        TrainingSession.InitializeTrainingSession(RecoveryNot);
        IsCalibrating = false;
        OnTrainingStart.Invoke(this);
    }

}
