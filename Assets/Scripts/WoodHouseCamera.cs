using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodHouseCamera : MonoBehaviour
{
    [SerializeField] GameObject cmvcam;
    [SerializeField] Transform player;

    void Update()
    {
        if (player.position.x > 0f)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(8.12f, 2.2f, -10f), 0.15f); //new Vector3(8.12f, 2.34f, -10f);
        }
        else if (player.position.x < -2.0f)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(0.25f, 1.95f, -10f), 0.1f);
        }
    }
}


