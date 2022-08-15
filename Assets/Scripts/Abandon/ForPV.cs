using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForPV : MonoBehaviour
{
    public Transform point, player;
    public AudioClip voice;

    public bool canReset, isFall, isClose, isLeave;
    public float speed;
    public int timeCnt;

    Animator anim;
    AudioSource au;

    // Start is called before the first frame update
    void Start()
    {
        speed = 8.0f;
        canReset = true;
        isFall = false;
        isClose = false;
        isLeave = false;
        anim = GetComponent<Animator>();
        au = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (point.position.x - player.position.x < 13) {
            if (canReset) {
                transform.position = new Vector3(point.position.x, point.position.y + 10, 0);
                canReset = false;
            }
            StartCoroutine(MoveToPosition());
        }

        if(transform.position.x - player.position.x < 7) {
            Debug.Log(transform.position.x - player.position.x);
            isClose = true;
        }


        if (timeCnt == 500 && isClose) { isLeave = true; }
        if (timeCnt > 580) {
            isClose = false;
        }

        if (isLeave) {
            point.position += new Vector3(5, 20, 0);
            transform.localScale = new Vector3(-0.6f, 0.6f, 1);
            speed = 10.0f;
            isLeave = false;
        }
        Voice();
        AnimChange();
    }

    IEnumerator MoveToPosition()
    {
        transform.position = Vector3.MoveTowards(transform.position, point.position, speed * Time.deltaTime);
        if (speed > 1) { speed *= 0.995f; }
        if(transform.position != point.position) {
            isFall = true;
        }
        else {
            isFall = false;
        }
        yield return 0;
    }

    void Voice()
    {
        if (isClose) {
            if (timeCnt % 120 == 0) {
                au.PlayOneShot(voice);
            }
            timeCnt++;
        }
    }

    void AnimChange()
    {
        if (isFall) {
            anim.SetBool("isFall", true);
            anim.SetBool("isIdle", false);
        }
        else {
            anim.SetBool("isFall", false);
            anim.SetBool("isIdle", true);
        }
    }
}

