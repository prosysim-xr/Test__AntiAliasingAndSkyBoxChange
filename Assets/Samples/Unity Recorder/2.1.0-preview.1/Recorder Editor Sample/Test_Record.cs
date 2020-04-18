#if UNITY_EDITOR

using UnityEngine;
using System.IO;
using UnityEditor;
using UnityEditor.Recorder;
using UnityEditor.Recorder.Input;

public class Test_Record : MonoBehaviour
{
    RecorderController m_RecorderController;

    private void OnEnable()
    {
        var controllerSettings = ScriptableObject.CreateInstance<RecorderControllerSettings>();
        m_RecorderController = new RecorderController(controllerSettings);

        var mediaOutputFolder = Path.Combine(Application.dataPath, "..", "SampleRecordings");

        //video
        var videoRecorder = ScriptableObject.CreateInstance<MovieRecorderSettings>();
        videoRecorder.name = "My Video Recorder";
        videoRecorder.Enabled = true;

        videoRecorder.OutputFormat = MovieRecorderSettings.VideoRecorderOutputFormat.MP4;
        videoRecorder.VideoBitRateMode = VideoBitrateMode.Low;

        videoRecorder.ImageInputSettings = new GameViewInputSettings
        {
            OutputWidth = 1920,
            OutputHeight = 1080
        };

        videoRecorder.AudioInputSettings.PreserveAudio = true;

        videoRecorder.OutputFile = Path.Combine(mediaOutputFolder, "video_v" + DefaultWildcard.Take);


        // Setup Recording
        controllerSettings.AddRecorderSettings(videoRecorder);

        controllerSettings.SetRecordModeToManual();
        controllerSettings.FrameRate = 60.0f;

        RecorderOptions.VerboseMode = false;
        m_RecorderController.StartRecording();

    }
    void Start()
    {
        
    }
    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            RecorderEditorExample.StartRecording();
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            RecorderEditorExample.StopRecording();
        }*/
    }
}

#endif