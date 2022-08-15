using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowCreater : MonoBehaviour
{
    public GameObject[] shadow;
    public Transform ca;
    int timeCnt;

    // Update is called once per frame
    void Update()
    {
        if (timeCnt == 500)
        {
            GameObject.Instantiate(shadow[Random.Range(0, 4)], new Vector3(ca.position.x - 12, -1.5f, 0), Quaternion.identity);
            timeCnt = 0;
        }
        timeCnt++;
    }
}

