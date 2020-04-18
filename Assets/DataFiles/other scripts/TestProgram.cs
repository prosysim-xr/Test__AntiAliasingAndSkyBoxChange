using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using TestCContainer;

public class TestProgram : MonoBehaviour
{

    private void Start()
    {
        GameObject.FindGameObjectWithTag("TestMain").GetComponent<TestMain>().enabled = false;
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.W))
        {
            GameObject.FindGameObjectWithTag("TestMain").GetComponent<TestMain>().enabled = true;
        }
    }
}
