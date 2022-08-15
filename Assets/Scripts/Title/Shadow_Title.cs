using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow_Title : MonoBehaviour
{
    public GameObject[] shadow;
    public int time;

    struct ShadowShow
    {
        int typeNum;
        int timeCnt;
    }

    // Update is called once per frame
    void Update()
    {


        time++;
    }

    
}
