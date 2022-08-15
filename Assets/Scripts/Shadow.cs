using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : MonoBehaviour
{
    public GameObject ca;
    public float time;
    public float disappearTime;
    public float showTime;
    public float[] randomRange;

    // Update is called once per frame
    void Update()
    {
        if (time > showTime && time < disappearTime && GetComponent<SpriteRenderer>().color.a < 0.7f )
        {
            GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, 0.25f * Time.deltaTime);
        }else if(time > disappearTime)
        {
            GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, 0.2f * Time.deltaTime);
            if (GetComponent<SpriteRenderer>().color.a <= 0) 
            { 
                time = 0;
                if (tag != "Shadow")
                {
                    transform.position = new Vector3(Random.Range(ca.transform.position.x + randomRange[0], ca.transform.position.x + randomRange[1]), transform.position.y, 0);
                }
            }
        }
        time += Time.deltaTime;
    }
}