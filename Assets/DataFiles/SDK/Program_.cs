using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using sdk;
public class Program_ : MonoBehaviour
{


    private void Callback(LoginResponseModel loginResponseModel)
    {
        Debug.Log(loginResponseModel.api_key);


    }
    ConversationResponseModel conversationResponseModelObject = null;
    private void Callback(ConversationResponseModel conversationResponseModelObject)
    {
        Debug.Log("audio path = " + conversationResponseModelObject.response_channels.voice);
        this.conversationResponseModelObject = conversationResponseModelObject;

        // Utils.talk = !Utils.talk;
        Transform[] gos = GetComponentsInChildren<Transform>();
        foreach (Transform go in gos)
        {
            if (go.name.Equals("head"))
            {
                Debug.Log("component = " + go.name + "  type " + go.GetType());
                go.GetComponent<HeadScript>().StartTalking(conversationResponseModelObject.response_channels, gameObject);
            }
        }
        Debug.Log("conversationResponseModelObject.name = " + conversationResponseModelObject.name);
        if (conversationResponseModelObject.name.Equals("sr_check_brezza"))
        {
            Debug.Log("got into state");

            var pc_anim = GameObject.FindGameObjectWithTag("Brezza");
            var c_anim = pc_anim.GetComponent<Animator>();
            c_anim.SetBool("makeInvis", false);
            c_anim.SetBool("hasStarted", true);


            Invoke("DelayedAnimation_PedestalRot", 2f);

        }
    }

    private void DelayedAnimation_PedestalRot()
    {
        var pc_anim2 = GameObject.FindGameObjectWithTag("carHolder");
        var c_anim2 = pc_anim2.GetComponent<Animator>();
        c_anim2.SetBool("hasPedestalRotated", true);
    }
    //

    int count = 0;
    string[] custStates = new string[] { "", "cs_im_good", "cs_select_suv" };

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(new RestAPI().Login("dinesh+maruti@i2ce.in", "marutisuzuki", "D@vei2ce", Callback));

    }


    private void FixedUpdate()
    {
        if (/*OVRInput.GetDown(OVRInput.Button.One) || OVRInput.GetDown(OVRInput.Button.Any) ||*/ Input.GetMouseButtonDown(0))
        {
            if (count == 0)
            {
                /*var pc_anim = GameObject.FindGameObjectWithTag("Brezza");

                var c_anim = pc_anim.GetComponent<Animator>();

                c_anim.SetBool("makeInvis", true);*/
                StartCoroutine(new RestAPI().PostConversation<ConversationRequestModel>(null, "dave_maruti_vr", "asdfasdf", Callback));
            }
            else
            {
                ConversationRequestModel conversationRequestModelObject = new ConversationRequestModel();
                conversationRequestModelObject.customer_state = custStates[count];
                conversationRequestModelObject.engagement_id = this.conversationResponseModelObject.engagement_id;
                conversationRequestModelObject.system_response = this.conversationResponseModelObject.name;

                StartCoroutine(new RestAPI().PostConversation<ConversationRequestModel>(conversationRequestModelObject, "dave_maruti_vr", "asdfasdf", Callback));
            }
            count++;
            if (count >= custStates.Length)
            {
                count = 0;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
    }
    bool talk = false;
    float time = 0;
    int weightUsedCount = 0;
    float[] weights = { 100f, 50f, 20f, 0f, 15f, 30f, 50f, 70f, 10f };
    void changeBlendShapeWeight(int index, float weight)
    {
        skinnedMeshRenderer.SetBlendShapeWeight(0, blendOne);
    }
    int blendShapeCount;
    SkinnedMeshRenderer skinnedMeshRenderer;
    Mesh skinnedMesh;
    float blendOne = 0f;
    float blendTwo = 0f;
    float blendSpeed = 1f;
    bool blendOneFinished = false;

    GameObject GetChildWithName(GameObject obj, string name)
    {
        Transform trans = obj.transform;
        Transform childTrans = trans.Find(name);
        if (childTrans != null)
        {
            return childTrans.gameObject;
        }
        else
        {
            return null;
        }
    }

}
