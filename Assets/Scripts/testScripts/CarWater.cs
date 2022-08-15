using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarWater : MonoBehaviour
{
    float speed;
    float ratio;
    Rigidbody2D rb;
    Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        StartCoroutine("Move");

        speed = Random.Range(8.0f, 14.0f);
        ratio = Random.Range(10, 91) / 100.0f;
        rb.velocity = new Vector2(-speed * ratio, speed * (1 - ratio));

    }

    void Update()
    {
        if (transform.position.y < -8f)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator Move()
    {
        for (int i = 0; i < 15; i++)
        {
            yield return new WaitForSeconds(0.1f);
            transform.rotation = Quaternion.Euler(0, 0, transform.localEulerAngles.z + i);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Ground" || other.tag == "Umbrella")
        {
            rb.velocity = Vector2.zero;
            anim.SetBool("isTouch", true);
        }
    }

    public void Touch()
    {
        Destroy(gameObject);
    }
}
