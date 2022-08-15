using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enter_Bird : MonoBehaviour
{
    public Transform player;
    public GameObject bird;

    // Update is called once per frame
    void Update()
    {
        if (player.position.x > transform.position.x) {
            bird.GetComponent<Bird>().isEnterBird = true;
            
            Destroy(gameObject);
        }
        
    }
}
