using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title_Camera : MonoBehaviour
{
    float speed = 2;


    private void Awake()
    {
        transform.position = new Vector3(0, -5.5f, -10);
    }
    // Update is called once per frame
    void Update()
    {
        if(transform.position.y < 1.5f) {
            transform.position += new Vector3(0, speed * Time.deltaTime, 0);
        }
    }
}
