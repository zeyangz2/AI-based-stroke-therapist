using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class DataRecorder : MonoBehaviour
{
    //C:/Users/Anne/Box/OSF Data/Data
    public DeviceInformation DeviceInformation;
    public float SampleRate = 30.0f; // samples/s
    public bool IsRecording;
    public string RootPath;
    private string fileName;
    public Transform BallToMoveTransform; 
    public Transform BluePlateTransform;
    public Transform RedPlateTransform;
    public Transform GreenPlateTransform;

    public UnityEvent OnRecordingStarted = new UnityEvent();
    public UnityEvent OnRecordingStopped = new UnityEvent();
    public UnityEvent<string> OnFileNameSet = new UnityEvent<string>();

    public void StartRecording()
    {
        
        IsRecording = true;
        StartCoroutine(RecordingRoutine());
        OnRecordingStarted.Invoke();
    }

    public void StopRecording()
    {
        
        IsRecording = false;
        StopCoroutine(RecordingRoutine());
        OnRecordingStopped.Invoke();
    }

    public void SetFileName(string fileName)
    {
        this.fileName = fileName;
    }

    private string generateFileName(string fileName)
    {
        string generatedFileName = fileName + "_" + DateTime.Now.ToString().Replace('/', '-').Replace(':','-') + "_" + Guid.NewGuid();
        OnFileNameSet.Invoke(generatedFileName);
        return generatedFileName;
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space) & !IsRecording)
        //{
        //    StartRecording();
        //}
        //else if (Input.GetKeyDown(KeyCode.Space) & IsRecording)
        //{
        //    StopRecording();
        //}
    }

    IEnumerator RecordingRoutine()
    {
        //filePath = Application.dataPath + "/" + FileName + ".csv";
        string generatedFileName = generateFileName(fileName);
        string filePath = RootPath + "/" + generatedFileName + ".csv";
        Debug.Log(filePath);
        TextWriter tw = new StreamWriter(filePath, false);
        tw.WriteLine("Timestamp,Joint angle 0,Joint angle 1,Joint angle 2,Stylus Position X,Stylus Position Y,Stylus Position Z,Ball X,Ball Y,Ball Z,Blue Global X,Blue Global Y,Blue Global Z,Red Global X,Red Global Y,Red Global Z,Green Global X,Green Global Y,Green Global Z");



        while (IsRecording)
        {
            tw.WriteLine(
                DateTime.Now.Ticks.ToString() + ","
                + DeviceInformation.JointAngles[0].ToString() + ","
                + DeviceInformation.JointAngles[1].ToString() + ","
                + DeviceInformation.JointAngles[2].ToString() + ","
                + DeviceInformation.Position[0].ToString() + ","
                + DeviceInformation.Position[1].ToString() + ","
                + DeviceInformation.Position[2].ToString() + ","
                + BallToMoveTransform.position.x + ","
                + BallToMoveTransform.position.y + ","
                + BallToMoveTransform.position.z + ","
                + BluePlateTransform.position.x + ","
                + BluePlateTransform.position.y + ","
                + BluePlateTransform.position.z + ","
                + RedPlateTransform.position.x + ","
                + RedPlateTransform.position.y + ","
                + RedPlateTransform.position.z + ","
                + GreenPlateTransform.position.x + ","
                + GreenPlateTransform.position.y + ","
                + GreenPlateTransform.position.z
                );


            yield return new WaitForSeconds(1 / SampleRate);
        }

        tw.Close();
    }
}
