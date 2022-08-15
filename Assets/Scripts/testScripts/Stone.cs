using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    public GameObject light;

    float speed = 1.5f;
    float x;
    bool isEnd;

    private void Start()
    {
        x = Random.Range(-2.0f, 2.1f);
    }

    void Update()
    {
        speed *= 1.06f;
        transform.position += new Vector3(x * Time.deltaTime, -speed * Time.deltaTime, 0);
        if(transform.position.y < -3.5f) {
            if (!isEnd) { StartCoroutine(DropEnd()); isEnd = true; }
            //if(transform.position.y < -100f) { Destroy(gameObject); }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Umbrella" || other.tag == "Player" || other.tag == "Ground")
        {
            if (!isEnd)
            {
                StartCoroutine(DropEnd());
                isEnd = true;
            }
        }
    }

    IEnumerator DropEnd()
    {
        GetComponent<Animator>().SetTrigger("Drop");
        GetComponent<AudioSource>().Play();
        transform.position += new Vector3(0.0f, -10.0f, 0.0f);
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
