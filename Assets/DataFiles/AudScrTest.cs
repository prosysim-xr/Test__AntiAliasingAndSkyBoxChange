using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudScrTest : MonoBehaviour
{
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        print(Application.persistentDataPath);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            print("audio recording started");
            var micAudioClip = Microphone.Start(Microphone.devices[0], true, 10, 16000);
            audioSource.clip = micAudioClip;
        }

        if (Input.GetMouseButtonDown(1))
        {
            Microphone.End(Microphone.devices[0]);
            print("Audio recording finshed");
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            print(Microphone.devices[0]);
            //byte [] bytes = SavWav.GetWav( audioSource.clip, out var length,false);
            SavWav.Save("soham.wav", audioSource.clip, false);
            //print("Audio saved at " + bytes.Length);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            print("playing audio");
            audioSource.Play();
        }
    }
}