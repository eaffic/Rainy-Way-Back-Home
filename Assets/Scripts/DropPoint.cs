using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropPoint : MonoBehaviour
{
    public GameObject water;

    // Update is called once per frame
    void Start()
    {
        StartCoroutine("Fall");
    }

    IEnumerator Fall()
    {
        while (true) {
            Instantiate(water, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(1.5f);
        }
    }
}
