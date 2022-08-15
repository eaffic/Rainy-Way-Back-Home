using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropRain : MonoBehaviour
{
    [SerializeField] private float speed = 8f;
    [SerializeField] private int damage = 1;
    Collider2D coli;
    Animator anim;
    bool isTouch = false;

    void Start()
    {
        coli = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isTouch)
        {
            transform.position += -transform.up * speed * Time.deltaTime;
        }
        if(transform.position.y < -10.0f) { Destroy(gameObject); }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Touch");

        if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Umbrella") || other.gameObject.CompareTag("Rest") || other.gameObject.CompareTag("Player"))
        {
            isTouch = true;
            coli.enabled = false;
            transform.position += new Vector3(0, 0.2f, 0);
            anim.SetBool("isTouch", true);
        }
    }

    public void Touch()
    {
        Destroy(gameObject);
    }
}
