using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPathSelector : MonoBehaviour
{
    public bool IsOSF;
    public SavePathsScriptableObject OSFPaths;
    public SavePathsScriptableObject UIUCPaths;

    DataRecorder dataRecorder;
    MovieRecorder movieRecorder;

    private void Awake()
    {
        dataRecorder = GetComponent<DataRecorder>();
        movieRecorder = GetComponent<MovieRecorder>();
        if(IsOSF)
        {
            dataRecorder.RootPath = OSFPaths.CSVPath;
            movieRecorder.RootPath = OSFPaths.VideoPath;
        } else
        {
            dataRecorder.RootPath = UIUCPaths.CSVPath;
            movieRecorder.RootPath = UIUCPaths.VideoPath;
        }
    }
}
