using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUtilityScript : MonoBehaviour
{

    /*public void UtilityMethod1(GameObject go, GameObject go1, Texture texture)
    {
        go1.GetComponent<MeshRenderer>().material.color = Color.blue;
        Invoke("ApplyTexture", 3f);
    }*/
    public IEnumerator UtilityCoroutine(GameObject go, GameObject go1, Texture texture, GameObject parentCylinder,RuntimeAnimatorController rac)
    {
        go1.GetComponent<MeshRenderer>().material.color = Color.blue;
        yield return new WaitForSeconds(2f);
        ApplyTexture(go1, texture);
        StartCoroutine(Mycouroutine(go1, parentCylinder));
       
    }

    private void ApplyTexture(GameObject go1, Texture texture)
    {
        go1.GetComponent<MeshRenderer>().material.color = Color.white;
        go1.GetComponent<MeshRenderer>().material.mainTexture = texture;
    }
    IEnumerator Mycouroutine(GameObject go1, GameObject parentCylinder)
    {
        var ator = GameObject.FindGameObjectWithTag("Capsule").GetComponent<Animator>();
        ator.SetBool("isCapsuleReadyToMove", true);
        
        yield return new WaitForSeconds(5f);
        go1.transform.parent = parentCylinder.transform;
    }
}
