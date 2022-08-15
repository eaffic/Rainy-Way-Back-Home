using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crow_Stop : MonoBehaviour
{
    public GameObject crow;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" && transform.tag == "CrowStop") { crow.GetComponent<Crow>().isStop = true; }
        if(other.tag == "Player" && transform.tag == "CrowRenew") { crow.GetComponent<Crow>().isStop = false; }
    }
}
