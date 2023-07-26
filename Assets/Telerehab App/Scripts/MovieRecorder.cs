using System.IO;
using UnityEditor;
using UnityEditor.Recorder;
using UnityEditor.Recorder.Input;
using UnityEngine;

public class MovieRecorder : MonoBehaviour
{
    RecorderController m_RecorderController;
    public bool m_RecordAudio = true;
    internal MovieRecorderSettings m_Settings = null;

    public string RootPath;
    private string fileName;

    //public FileInfo OutputFile
    //{
    //    get
    //    {
    //        var fileName = m_Settings.OutputFile + ".mp4";
    //        return new FileInfo(fileName);
    //    }
    //}

    //private void OnEnable()
    //{
    //    StartRecording();
    //}

    private void OnDisable()
    {
        StopRecording();
    }

    public void SetFileName(string _fileName)
    {
        this.fileName = _fileName;
    }

    public void StartRecording()
    {
        Debug.Log(fileName);
        string filePath = RootPath + "/" + fileName;
        var controllerSettings = ScriptableObject.CreateInstance<RecorderControllerSettings>();
        m_RecorderController = new RecorderController(controllerSettings);

        var mediaOutputFolder = new DirectoryInfo(RootPath);

        // Video
        m_Settings = ScriptableObject.CreateInstance<MovieRecorderSettings>();
        m_Settings.name = "My Video Recorder";
        m_Settings.Enabled = true;

        // This example performs an MP4 recording
        m_Settings.OutputFormat = MovieRecorderSettings.VideoRecorderOutputFormat.MP4;
        m_Settings.VideoBitRateMode = VideoBitrateMode.High;

        m_Settings.ImageInputSettings = new GameViewInputSettings
        {
            OutputWidth = 1920,
            OutputHeight = 1080
        };

        m_Settings.AudioInputSettings.PreserveAudio = m_RecordAudio;

        // Simple file name (no wildcards) so that FileInfo constructor works in OutputFile getter.
        m_Settings.OutputFile = filePath;

        // Setup Recording
        controllerSettings.AddRecorderSettings(m_Settings);
        controllerSettings.SetRecordModeToManual();
        controllerSettings.FrameRate = 60.0f;

        RecorderOptions.VerboseMode = false;
        m_RecorderController.PrepareRecording();
        m_RecorderController.StartRecording();

        Debug.Log($"Started recording for file {filePath}");
    }

    public void StopRecording()
    {
        if (m_RecorderController == null)
            return;
        m_RecorderController.StopRecording();
    }
}
