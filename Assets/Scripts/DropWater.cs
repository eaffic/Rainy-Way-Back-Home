using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropWater : MonoBehaviour
{
    Collider2D coli;
    Rigidbody2D rd;
    Animator anim;

    private void Start()
    {
        coli = GetComponent<Collider2D>();
        rd = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        //float speed = -5;
        //float ratio = Random.Range(10, 101) / 100.0f;

        //rd.velocity = new Vector2(speed * ratio, speed * (1 - ratio));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Umbrella") || other.gameObject.CompareTag("Rest")) {
            coli.enabled = false;
            rd.gravityScale = 0;
            rd.velocity = Vector2.zero;
            rd.position += new Vector2(0, 0.3f);
            anim.SetBool("isTouch", true);
        }
    }

    public void Touch()
    {
        Destroy(gameObject);
    }
}
