using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxChange : MonoBehaviour
{

    public Material[] Skyboxes;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RenderSettings.skybox = Skyboxes[0];
        }if (Input.GetMouseButtonDown(1))
        {
            RenderSettings.skybox = Skyboxes[1];
        }
    }

    
} 


    /*  if (RenderSettings.skybox.HasProperty("_Tint"))
         RenderSettings.skybox.SetColor("_Tint", Color.red);
     else if (RenderSettings.skybox.HasProperty("_SkyTint"))
         RenderSettings.skybox.SetColor("_SkyTint", Color.red);*/
