using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aud2 : MonoBehaviour
{
    public bool isPrinted = false;
    public bool isPrinted2 = false;
    public int a = 0;
    public int b = 0;
    public int a1 = 0;
    public int b1 = 0;

    public AudioSource audioSource;
    public AudioClip audioClip;
    public AudioClip micAudioClip;

    public bool micRecording = false;

    enum RecordStates
    {
        recordingStarted,
        recordingInProgress,
        recordingInLoop,
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
        {

            StartCoroutine(RecordAudioClipListCoroutine());
        }
            


        if(recordStates == RecordStates.recordingInLoop)
        {
            
        }

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


            if (a == b)
            {
                micAudioClip = Microphone.Start(Microphone.devices[0], true, 5, 44100);
                b++;

            }
            yield return new WaitForSeconds(10);
            a++;

            if (a1 == b1)
            {
                Microphone.End(Microphone.devices[0]);
                audioClipList.Add(micAudioClip);

                print(Microphone.devices[0]);

                if (audioClipList[0] != null)
                {
                    print("micAudioClip is not null");
                    audioSource.clip = micAudioClip;
                    print("audioclip played");

                    SavWav.Save("suman" + Time.time + ".wav", audioClipList[0], false);//todo
                }
                b1++;
            }
            a1++;

            if (Input.GetMouseButtonDown(1))
            {
                //Microphone.End(Microphone.devices[0]);
                micRecording = false;
                recordStates = RecordStates.recordingsMerge;
                print("Audio recording finshed");
            }
        }
    }

    private void RecordAudioClipList()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var audioClip = Microphone.Start(Microphone.devices[0], true, 5, 16000);
            Invoke("Microphone.End(Microphone.devices[0])", 5);
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
