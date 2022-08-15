using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Title_UIController : MonoBehaviour
{
    public Transform ca, player;
    public GameObject title, enter;

    public int timeCnt = 0;

    // Update is called once per frame
    void Update()
    {
        if(ca.position.y > 1.4 && title.GetComponent<SpriteRenderer>().color.a <= 1) {
            title.GetComponent<Collider2D>().enabled = true;
            if(timeCnt % 3 == 0) {
                title.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, 0.05f);
            }
            timeCnt++;
        }

        //Debug.Log(title.GetComponent<Text>().color.a);

        if(title.GetComponent<SpriteRenderer>().color.a > 0.95f) {
            if(timeCnt > 200) { timeCnt = 0; }

            if(timeCnt < 100 && enter.GetComponent<Text>().color.a < 1) {
                enter.GetComponent<Text>().color += new Color(0, 0, 0, 0.01f);
            }
            else if(timeCnt > 100 && enter.GetComponent<Text>().color.a > 0) {
                enter.GetComponent<Text>().color += new Color(0, 0, 0, -0.01f);
            }
            //Debug.Log(enter.GetComponent<Text>().color.a);
            timeCnt++;
        }
    }
}
