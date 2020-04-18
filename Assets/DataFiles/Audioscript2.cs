using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audioscript2 : MonoBehaviour
{
    void Start()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        
        for(var v =0; v<100; v++)
        {
            if (Input.GetMouseButtonDown(0))
            {
                print("1");
                //Invoke("RecordMethod(audioSource)", 5);
                RecordMethod(audioSource);
            }

            print("6");
            if (Time.time == 20f)
                break;
        }
        
    }
    public void RecordMethod(AudioSource audioSource)
    {
        print("2");
        audioSource.clip = Microphone.Start(Microphone.devices[0], true, 5, 44100);
        Invoke("EndRecord", 5);
        print("3");
    }

    public void EndRecord(AudioSource audioSource)
    {
        if (Time.time == 10f)
        {
            print("4");
            Microphone.End(Microphone.devices[0]);
            SavWav.Save("sumax.wav", audioSource.clip, false);
            print("5");
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
