using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudScr : MonoBehaviour
{
    public bool isPrinted = false;
    public bool isPrinted2 = false;

    public AudioSource audioSource;
    public AudioClip audioClip;
    public AudioClip micAudioClip;

    public bool micRecording = false;

    enum RecordStates
    {
        recordingStarted,
        recordingInProgress,
        recordingDone,
        recordingsMerge
    }
    RecordStates recordStates = RecordStates.recordingStarted;

    public List<AudioClip> audioClipList;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        print(Application.persistentDataPath);


    }

    // Update is called once per frame
    void Update()
    {
        if (recordStates == RecordStates.recordingStarted)
            RecordStart();

        if (recordStates == RecordStates.recordingInProgress)
            StartCoroutine(RecordAudioClipListCoroutine());

        if (recordStates == RecordStates.recordingsMerge)
            AudioClipListMerge(audioClipList);

        if (recordStates == RecordStates.recordingDone)
        {
            SaveAudioFileByPressing_Q();
            PlayAudioFileByPressing_W();
        }

    }

    private void RecordStart()
    {
        if (Input.GetMouseButtonDown(0))
        {
            print("Audio recording started");
            audioClipList.Clear();
            micRecording = true;
            recordStates = RecordStates.recordingInProgress;
        }
    }


    //-->recordStates = RecordStates.recordingInProgress;
    private IEnumerator RecordAudioClipListCoroutine()
    {

        if (micRecording == true)
        {
            audioClipList.Add(micAudioClip);

            //if(finished10Sec)
            //date and time
            print("statuscode 1");
            var timeStart = System.DateTime.Now;
            yield return new WaitForSeconds(5);
            print("statuscode 2");
            var timeFin = System.DateTime.Now;
            if ((timeFin - timeStart) >= System.TimeSpan.FromMilliseconds(5100) && (timeFin - timeStart) <= System.TimeSpan.FromMilliseconds(5500))
            {
                print("inside the timespan");
                micAudioClip = Microphone.Start(Microphone.devices[0], true, 5, 16000);
                //Microphone.End(Microphone.devices[0]);
                //audioClipList.Add(micAudioClip);

                print(Microphone.devices[0]);

                if (audioClipList[0] != null)
                {
                    print("micAudioClip is not null");
                    audioSource.clip = micAudioClip;
                    audioSource.Play();
                    print("audioclip played");

                    SavWav.Save("suman" + Time.time + ".wav", audioClipList[0], false);//todo
                }

            }

            if (Input.GetMouseButtonDown(1))
            {
                //Microphone.End(Microphone.devices[0]);
                micRecording = false;
                recordStates = RecordStates.recordingsMerge;
                print("Audio recording finshed");
            }
        }
    }

    //-->recordStates == RecordStates.recordingDone
    private void PlayAudioFileByPressing_W()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            print("playing audio");
            audioSource.Play();
        }
    }

    private void SaveAudioFileByPressing_Q()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            print(Microphone.devices[0]);
            //byte [] bytes = SavWav.GetWav( audioSource.clip, out var length,false);
            SavWav.Save("suman.wav", audioSource.clip, false);
            //print("Audio saved at " + bytes.Length);

        }
    }


    //-->recordStates == RecordStates.recordingsMerge
    private void AudioClipListMerge(List<AudioClip> audioClipList)
    {
        var bigFloatList = new List<float>();
        /*var floatLength = 0;
        foreach (var audioclip in audioClipList)
        {
            floatLength += audioclip.samples * audioclip.channels;
        }*/
        foreach (var audioclip in audioClipList)
        {
            float[] samples = new float[audioclip.samples * audioclip.channels];
            audioclip.GetData(samples, 0);
            foreach (var v in samples)
            {
                bigFloatList.Add(v);
            }
            audioclip.SetData(samples, 0);
            if (isPrinted2 == false)
            {
                isPrinted2 = true;
                audioSource.clip = audioclip;
                audioSource.Play();
            }

        }
        for (var v = 0; v < 1000; v++)
        {
            print(bigFloatList[v]);
        }
        var bigFloatArray = bigFloatList.ToArray();
        print(bigFloatArray[1000]);

        //AudioClip tempAudioClip;
        audioSource.clip = gameObject.GetComponent<AudioClip>();
        audioSource.clip.SetData(bigFloatArray, 0);
        recordStates = RecordStates.recordingDone;
    }
}
