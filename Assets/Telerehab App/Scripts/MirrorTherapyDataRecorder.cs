using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MirrorTherapyDataRecorder : MonoBehaviour
{
    [SerializeField] private DeviceInformation therapistRobotDeviceInformation;
    [SerializeField] private DeviceInformation pflRobotDeviceInformation;
    [SerializeField] private DeviceInformation pilRobotDeviceInformation;

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
            "Therapist Angle 0,Therapist Angle 1,Therapist Angle 2," +
            "Therapist Position X,Therapist Position Y,Therapist Position Z," +
            "PFL Angle 0,PFL Angle 1,PFL Angle 2," +
            "PFL Position X,PFL Position Y,PFL Position Z," +
            "PIL Angle 0,PIL Angle 1,PIL Angle 2," +
            "PIL Position X,PIL Position Y,PIL Position Z"
           );

        while (IsRecording)
        {
            tw.WriteLine(
                DateTime.Now.Ticks.ToString() + ","
                + therapistRobotDeviceInformation.JointAngles[0].ToString() + ","
                + therapistRobotDeviceInformation.JointAngles[1].ToString() + ","
                + therapistRobotDeviceInformation.JointAngles[2].ToString() + ","
                + therapistRobotDeviceInformation.Position[0].ToString() + ","
                + therapistRobotDeviceInformation.Position[1].ToString() + ","
                + therapistRobotDeviceInformation.Position[2].ToString() + ","
                + pflRobotDeviceInformation.JointAngles[0].ToString() + ","
                + pflRobotDeviceInformation.JointAngles[1].ToString() + ","
                + pflRobotDeviceInformation.JointAngles[2].ToString() + ","
                + pflRobotDeviceInformation.Position[0].ToString() + ","
                + pflRobotDeviceInformation.Position[1].ToString() + ","
                + pflRobotDeviceInformation.Position[2].ToString() + ","
                + pilRobotDeviceInformation.JointAngles[0].ToString() + ","
                + pilRobotDeviceInformation.JointAngles[1].ToString() + ","
                + pilRobotDeviceInformation.JointAngles[2].ToString() + ","
                + pilRobotDeviceInformation.Position[0].ToString() + ","
                + pilRobotDeviceInformation.Position[1].ToString() + ","
                + pilRobotDeviceInformation.Position[2].ToString()
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
