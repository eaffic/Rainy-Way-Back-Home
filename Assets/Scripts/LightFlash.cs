using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightFlash : MonoBehaviour
{
    public float speed;
    public float temp;
    public float[] lightRange;

    // Start is called before the first frame update
    void Start()
    {
        temp = -speed;
    }

    // Update is called once per frame
    void Update()
    {
        Flash();
    }

    void Flash()
    {
        GetComponent<Light2D>().pointLightOuterRadius += temp;
        if(GetComponent<Light2D>().pointLightOuterRadius > lightRange[1]) { temp = -speed; }
        else if(GetComponent<Light2D>().pointLightOuterRadius < lightRange[0]) { temp = speed; }
    }
}
