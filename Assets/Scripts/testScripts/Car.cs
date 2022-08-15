using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField] private GameObject water;
    [SerializeField] private float speed = 10f;
    float randTime;
    int randAngle;
    Vector2 shotPos;
    string objName;

    void Start()
    {
        objName = gameObject.name.Substring(3, 1);
    }

    void Update()
    {
        //Debug.Log(objName == "2");
        if (objName != "2")
        {
            transform.position += -transform.right * speed * Time.deltaTime;
        }
        else
        {
            transform.position += transform.right * speed * Time.deltaTime;
        }
    }

    IEnumerator DestroyCar()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("waterPos"))
        {
            shotPos = new Vector2(transform.position.x, transform.position.y - 1.0f);
            //StartCoroutine("ShotWater");
            ShotWater();
            StartCoroutine("DestroyCar");
        }
    }

    void ShotWater()
    {
        //yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < 20; i++)
        {
            randTime = Random.Range(0.05f, 0.07f);
            randAngle = Random.Range(30, 70);
            //randAngle = objName != "2" ? randAngle : -randAngle;
            Instantiate(water, shotPos, Quaternion.Euler(0, 0, randAngle));
            randTime = Random.Range(0.05f, 0.07f);
            randAngle = Random.Range(30, 70);
            //randAngle = objName != "2" ? randAngle : -randAngle;
            Instantiate(water, shotPos, Quaternion.Euler(0, 0, randAngle));
            //yield return null;
        }
    }
}
