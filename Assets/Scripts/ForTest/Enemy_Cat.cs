using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Cat : MonoBehaviour
{
    public GameObject player;
    public GameObject stone;
    public Transform[] pos;

    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
