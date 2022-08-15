using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Branch : MonoBehaviour
{
    public Transform bird;
    public bool isDrop;
    public bool isEnd;
    float speed = 5.0f;

    private void Start()
    {
        isEnd = false;
        isDrop = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDrop) {
            transform.position -= new Vector3(0, speed * Time.deltaTime, 0);
        }
        else {
            transform.position = bird.position + new Vector3(0, -0.3f, 0);
        }
    }

    public void Reset()
    {
        isDrop = false;
        isEnd = false;
        //gameObject.SetActive(true);
        transform.position = bird.position + new Vector3(0, -0.3f, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Ground" || other.tag == "Rest" || other.tag == "Umbrella" || other.tag == "Player") {
            if (!isEnd)
            {
                StartCoroutine(DropEnd());
                isEnd = true;
            }
            //gameObject.SetActive(false);
        }
    }

    IEnumerator DropEnd()
    {
        GetComponent<Animator>().SetTrigger("Umbrella");
        GetComponent<AudioSource>().Play();
        transform.position += new Vector3(0.0f, -10.0f, 0.0f);
        yield return new WaitForSeconds(0.3f);
        GetComponent<Animator>().SetTrigger("Normal");
        //gameObject.SetActive(false);
    }
}
