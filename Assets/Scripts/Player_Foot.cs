using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Foot : MonoBehaviour
{
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "FloorWater")
        {
            anim.SetBool("isTouch", true);
        }
    }

    public void OutArea()
    {
        anim.SetBool("isTouch", false);
    }
}
