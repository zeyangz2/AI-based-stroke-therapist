using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CalibrationSession : MonoBehaviour
{
    public Therapist Therapist;
    public Patient Patient;

    public float ErrorMetric = 0;
    public float NumFrames = 1;
    public float NumFramesThreshold;

    public List<float> CalibrationDistances = new List<float>();

    public UnityEvent<float> OnCalibrationComplete = new UnityEvent<float>();
    public UnityEvent<float> OnNumFramesUpdated = new UnityEvent<float>();

    //public List<float> distances = new List<float>();

    private void FixedUpdate()
    {
        AccumulateDistance();

        if (NumFrames >= NumFramesThreshold)
        {
            
            OnCalibrationComplete.Invoke(ErrorMetric);
            return;
        }
        CalculateErrorMetric();
        NumFrames += 1;
        OnNumFramesUpdated.Invoke(GetPercentageCalibrated());



        //Calibrate();
        //NumFrames += 1;
        //if(NumFrames >= NumFramesThreshold)
        //{
        //    OnCalibrationComplete.Invoke(ErrorMetric);
        //}
    }

    public void InitializeCalibrationSession()
    {
        gameObject.SetActive(true);
    }

    public void StopCalibrationSession()
    {
        gameObject.SetActive(false);
    }

    public float GetPercentageCalibrated()
    {
        return (int)(100f*((float)NumFrames / (float)NumFramesThreshold));
    }

    void AccumulateDistance()
    {
        float distance = Vector3.Distance(Therapist.DeviceInformation.Position, Patient.DeviceInformation.Position);
        //Debug.Log("Distance from calibration: " + distance.ToString());
        CalibrationDistances.Add(distance);
    }

    void CalculateErrorMetric()
    {
        float errorMetric = 0;
        foreach (float calibrationDistance in CalibrationDistances)
        {
            errorMetric += calibrationDistance;
        }
        errorMetric /= CalibrationDistances.Count;
        ErrorMetric = errorMetric;
    }

    void Calibrate()
    {
        float distance = Vector3.Distance(Therapist.DeviceInformation.Position, Patient.DeviceInformation.Position);
        //Debug.Log("Distance from calibration: " + distance.ToString());
        ErrorMetric = (ErrorMetric + distance)/NumFrames;
    }
}
