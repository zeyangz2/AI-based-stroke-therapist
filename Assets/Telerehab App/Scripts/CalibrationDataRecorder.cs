using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CalibrationDataRecorder : MonoBehaviour
{
    //public HapticPlugin HapticDevice;

    public float SampleRate = 30.0f; // samples/s

    public bool IsRecording;

    public string FileName;

    string filePath;

    public CalibrationSession CalibrationSession;




    IEnumerator RecordDataForFrame()
    {
        //filePath = Application.dataPath + "/" + FileName + ".csv";
        filePath = "C:/Users/nisar2/Box/Tele-Rehab Paper/Tele-rehab Project Files/Data/" + FileName + ".csv";
        Debug.Log(filePath);
        TextWriter tw = new StreamWriter(filePath, false);
        tw.WriteLine(
           "Therapist position x,Therapist position y,Therapist position z,Patient position x,Patient position y,Patient position z"
            );


        while (IsRecording)
        {

            tw.WriteLine(
                DateTime.Now.Ticks.ToString() + ","
                + CalibrationSession.Therapist.DeviceInformation.Position[0].ToString() + ","
                + CalibrationSession.Therapist.DeviceInformation.Position[1].ToString() + ","
                + CalibrationSession.Therapist.DeviceInformation.Position[2].ToString() + ","
                + CalibrationSession.Patient.DeviceInformation.Position[0].ToString() + ","
                + CalibrationSession.Patient.DeviceInformation.Position[1].ToString() + ","
                + CalibrationSession.Patient.DeviceInformation.Position[2].ToString() + ","
                
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
