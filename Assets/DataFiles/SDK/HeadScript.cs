using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using sdk;
using System;
using static sdk.ConversationResponseModel;

public class HeadScript : MonoBehaviour
{
    SkinnedMeshRenderer skinnedMeshRenderer;
    Mesh skinnedMesh;

    void Awake()
    {
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        skinnedMesh = GetComponent<SkinnedMeshRenderer>().sharedMesh;
    }

    void Start()
    {
        
    }


    public void StartTalking(ResponseChannels responseChannels,GameObject gObject)
    {
        talkData = Utils.Read(new Utils().DownloadFileInline(responseChannels.mouth));
        Debug.Log("data = " + talkData.Count);
        Debug.Log("data = " + talkData[0]["state"]);
        StartCoroutine(new Utils().GetAndPlayAudio(responseChannels.voice, gObject));
        time = Time.time;
    }
    List<Dictionary<string, object>> talkData;
    Dictionary<string, float> talkParser = new Dictionary<string, float>()
    {
        {"open",80f },
        {"semi",40f },
        {"closed",0f }

    };
    void Update()
    {
        if (Utils.talk)
        {
            float now = Time.time;
            float deltaTime = now-time;

            for(int i = 0; i < talkData.Count; i++)
            {
                Dictionary<string, object> keyValuePairs = talkData[i];
                float timeStamp = (float)keyValuePairs["time"];
                //Debug.Log("in loop "+i);
                if (deltaTime >= timeStamp && i<talkData.Count-1 && deltaTime<=(float)talkData[i+1]["time"])
                {
                    changeBlendShapeWeight(2, ((float)keyValuePairs["state"]*100)-1f);
                }/*
                if (i == talkData.Count - 1)
                {
                    Utils.talk = false;
                }*/
            }

        }
    }
    float time = 0;
    int weightUsedCount = 0;
    float[] weights = { 99f, 50f, 20f, 0f, 15f, 30f, 50f, 70f, 10f };

    void changeBlendShapeWeight(int index, float weight)
    {
        Debug.Log("changed weight = " + weight);
        if(weight == 100f)
        {
            weight = 99f;
        }
        skinnedMeshRenderer.SetBlendShapeWeight(index, weight);
    }

}
