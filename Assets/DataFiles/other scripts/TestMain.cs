using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMain : MonoBehaviour
{
    //we will be adding capsule at perticular place... 
    //todo capsule to remove and attach on to...parent as sphere

    TestUtilityScript utilityObj;
    private GameObject go;
    private GameObject go1;
    private GameObject parentCylinder;

    private string tagName;

    private Texture carPaintTexture;
    public void Start()
    {
        
        //TestClassContainer.TestUtilityCall();

        tagName = "Sphere";//please specify tagname here for perticular gameobject 
        carPaintTexture = Resources.Load("CarPaint") as Texture;
    }

    // Update is called once per frame
    public void Update()
    {
        UtilityMethodUse();
    }



    public void UtilityMethodUse()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            print("the click Q worked");

            if (utilityObj == null)
                utilityObj = GameObject.FindGameObjectWithTag("TestUtility").GetComponent<TestUtilityScript>(); 

            if (go == null)
                go = GameObject.FindGameObjectWithTag(tagName);

            if (go1 == null)
                go1 = GameObject.FindGameObjectWithTag("Capsule");
            
            if (parentCylinder == null)
                parentCylinder = GameObject.FindGameObjectWithTag("ParentCylinder");
            else
                return;
            //utilityObj.UtilityMethod1(go, go1, carPaintTexture);
            var rac = GameObject.FindGameObjectWithTag("Capsule").GetComponent<RuntimeAnimatorController>();
            StartCoroutine(utilityObj.UtilityCoroutine(go, go1, carPaintTexture, parentCylinder, rac));

        }
    }

    /*public IEnumerator  TestMainCoroutine()
    {
        yield return new WaitForSeconds(0);
    }
    public void TestMainMethod()
    {
        StartCoroutine(TestMainCoroutine());
    }*/
}
