using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAnimationController : MonoBehaviour
{

    public Animator pc_anim; 
    // Start is called before the first frame update
    void Start()
    {
        pc_anim = GameObject.FindGameObjectWithTag("Brezza").GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            showCar();
        }

    }
    public void showCar()
    {
        pc_anim.SetBool("hasStarted", true);
    }
    /*public void AlertObservers(string message)
    {
        if (message.Equals("AttackAnimationEnded"))
        {
            CAPS = false;
            pc_anim.SetBool("attack", false);
            // Do other things based on an attack ending.
        }
    }*/

    

}
