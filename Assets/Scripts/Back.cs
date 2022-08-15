using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Back : MonoBehaviour
{
    public Transform ca;
    public bool isClose;
    public float time;
    public float distance;

    // Update is called once per frame
    void Update()
    {
        distance = Mathf.Abs(transform.position.x - ca.position.x);
        //Debug.Log(distance);
        if (tag == "Shadow") { 
            GetComponent<SpriteRenderer>().color = transform.parent.GetComponent<SpriteRenderer>().color;
        }
        else
        {
            if (time > 10.0f)
            {
                GetComponent<SpriteRenderer>().color *= new Color(1, 1, 1, 0.99f);
            }
            else if (distance <= 10 && distance >= 3)
            {
                float alpha = (10 - distance) / 7.0f;
                //Debug.Log(alpha);
                GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, alpha);
            }
        }


        if (distance <= 12 || !isClose)
        {
            time += Time.deltaTime;
            isClose = true;
        }
        else if(distance <= 22)
        {
            isClose = false;
            time = 0;
        }
    }
}
