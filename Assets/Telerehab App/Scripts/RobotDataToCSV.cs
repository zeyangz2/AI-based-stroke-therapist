using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class RobotDataToCSV : MonoBehaviour
{
    [SerializeField] private DeviceInformation robotDeviceInformaion;

    [SerializeField] private float SampleRate = 30.0f; // samples/s
    [SerializeField] private bool IsRecording;
    [SerializeField] private string RootPath;
    [SerializeField] private string fileName;

    public void StartRecording()
    {
        IsRecording = true;
        StartCoroutine(RobotDataRecordingRoutine());
    }

    public void StopRecording()
    {
        IsRecording = false;
        StopCoroutine(RobotDataRecordingRoutine());
    }

    public void SetFileName(string fileName)
    {
        this.fileName = fileName;
    }

    private string generateFileName(string fileName)
    {
        string generatedFileName = fileName + "_" + DateTime.Now.ToString().Replace('/', '-').Replace(':', '-') + "_" + Guid.NewGuid();
        return generatedFileName;
    }

    IEnumerator RobotDataRecordingRoutine()
    {
        string generatedFileName = generateFileName(fileName);
        string filePath = RootPath + "/" + generatedFileName + ".csv";
        Debug.Log(filePath);

        TextWriter tw = new StreamWriter(filePath, false);
        tw.WriteLine("Timestamp," +
            "Joint angle 0,Joint angle 1,Joint angle 2," +
            "Stylus Position X,Stylus Position Y,Stylus Position Z,"
           );

        while (IsRecording)
        {
            tw.WriteLine(
                DateTime.Now.Ticks.ToString() + ","
                + robotDeviceInformaion.JointAngles[0].ToString() + ","
                + robotDeviceInformaion.JointAngles[1].ToString() + ","
                + robotDeviceInformaion.JointAngles[2].ToString() + ","
                + robotDeviceInformaion.Position[0].ToString() + ","
                + robotDeviceInformaion.Position[1].ToString() + ","
                + robotDeviceInformaion.Position[2].ToString()
                );


            yield return new WaitForSeconds(1 / SampleRate);
        }

        tw.Close();
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
