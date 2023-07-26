using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TherapyDataRecorder : MonoBehaviour
{
    //public HapticPlugin HapticDevice;

    public float SampleRate = 30.0f; // samples/s

    public bool IsRecording;

    public string FileName;

    string filePath;

    public TrainingSession TrainingSession;



    
    IEnumerator RecordDataForFrame()
    {
        //filePath = Application.dataPath + "/" + FileName + ".csv";
        filePath = "C:/Users/annamra2/Box/Telerehab Project/Data/UIUC Data/CSVs/Interns testing/"+FileName+".csv";
        Debug.Log(filePath);
        TextWriter tw = new StreamWriter(filePath, false);
        tw.WriteLine(
            "Timestamp,Therapist joint angle 0,Therapist joint angle 1,Therapist joint angle 2,Therapist joint angle 3,Therapist joint angle 4,Therapist joint angle 5,Patient joint angle 0,Patient joint angle 1,Patient joint angle 2,Patient joint angle 3,Patient joint angle 4,Patient joint angle 5,Virtual joint angle 0,Virtual joint angle 1,Virtual joint angle 2,Therapist position x,Therapist position y,Therapist position z,Patient position x,Patient position y,Patient position z,Virtual position x,Virtual position y,Virtual position z,Assistive Force, Obstacle Force Magnitude, Therapist Constant Force, Patient Constant Force, Recovery Factor, Assistance Level"
            );
        

        while (IsRecording)
        {

            tw.WriteLine(
                DateTime.Now.Ticks.ToString() + ","
                + TrainingSession.Therapist.DeviceInformation.JointAngles[0].ToString() + ","
                + TrainingSession.Therapist.DeviceInformation.JointAngles[1].ToString() + ","
                + TrainingSession.Therapist.DeviceInformation.JointAngles[2].ToString() + ","
                + TrainingSession.Therapist.DeviceInformation.GimbalAngles[0].ToString() + ","
                + TrainingSession.Therapist.DeviceInformation.GimbalAngles[1].ToString() + ","
                + TrainingSession.Therapist.DeviceInformation.GimbalAngles[2].ToString() + ","
                + TrainingSession.Patient.DeviceInformation.JointAngles[0].ToString() + ","
                + TrainingSession.Patient.DeviceInformation.JointAngles[1].ToString() + ","
                + TrainingSession.Patient.DeviceInformation.JointAngles[2].ToString() + ","
                + TrainingSession.Patient.DeviceInformation.GimbalAngles[0].ToString() + ","
                + TrainingSession.Patient.DeviceInformation.GimbalAngles[1].ToString() + ","
                + TrainingSession.Patient.DeviceInformation.GimbalAngles[2].ToString() + ","
                + TrainingSession.VirtualDeviceInformation.JointAngles[0].ToString() + ","
                + TrainingSession.VirtualDeviceInformation.JointAngles[1].ToString() + ","
                + TrainingSession.VirtualDeviceInformation.JointAngles[2].ToString() + ","
                + TrainingSession.Therapist.DeviceInformation.Position[0].ToString() + ","
                + TrainingSession.Therapist.DeviceInformation.Position[1].ToString() + ","
                + TrainingSession.Therapist.DeviceInformation.Position[2].ToString() + ","
                + TrainingSession.Patient.DeviceInformation.Position[0].ToString() + ","
                + TrainingSession.Patient.DeviceInformation.Position[1].ToString() + ","
                + TrainingSession.Patient.DeviceInformation.Position[2].ToString() + ","
                + TrainingSession.VirtualDeviceInformation.Position[0].ToString() + ","
                + TrainingSession.VirtualDeviceInformation.Position[1].ToString() + ","
                + TrainingSession.VirtualDeviceInformation.Position[2].ToString() + ","
                + TrainingSession.AssitiveForceMagnitude + ","
                + TrainingSession.obstacle.ObstacleForceMagnitude + ","
                + TrainingSession.Therapist.HapticPlugin.ConstForceGMag + ","
                + TrainingSession.Patient.HapticPlugin.ConstForceGMag + ","
                + TrainingSession.RecoveryFactor + ","
                + TrainingSession.AssistanceLevel
                );
            yield return new WaitForSeconds(1 / SampleRate);
        }

        tw.Close();


    }

    public void SaveDataToCSV()
    {

    }

    public void StartRecording()
    {
        IsRecording = true;
        StartCoroutine(RecordDataForFrame());
    }

    public void StopRecording()
    {
        IsRecording = false;
        StopCoroutine(RecordDataForFrame());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) & !IsRecording)
        {
            StartRecording();
        }
        else if (Input.GetKeyDown(KeyCode.Space) & IsRecording)
        {
            StopRecording();
        }
    }
}
