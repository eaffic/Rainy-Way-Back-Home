using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    enum Direction { LEFT, RIGHT };
    enum FlyPoint { LEFT, RIGHT };

    public Transform player, ca;
    public Transform[] birdPoint;
    public GameObject branch;
    public AudioClip[] voice;
    public bool isEnterBird = true, isDrop, isPoint, isFinal;

    IEnumerator move;
    Rigidbody2D rb;
    [SerializeField]Animator anim;
    AudioSource au;
    Direction dir;
    FlyPoint fp;
    [SerializeField]float speed = 0.08f;
    [SerializeField]int timeCnt, voiceCnt;
    [SerializeField]Vector3 playerPos, targetPos;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        au = GetComponent<AudioSource>();
        move = MoveToPosition();
        isPoint = false;
        isFinal = false;
        dir = Direction.LEFT;
        fp = FlyPoint.LEFT;
    }

    // Update is called once per frame
    void Update()
    {
        if (isEnterBird) {
            playerPos = player.position;
       
            if (isDrop) {
                if (isFinal) {
                    targetPos = new Vector3(birdPoint[3].position.x, birdPoint[3].position.y, 0);
                }
                else {
                    targetPos = new Vector3(birdPoint[1].position.x, birdPoint[1].position.y, 0);
                }
                StopCoroutine(move);
                move = MoveToHome();
                StartCoroutine(MoveToHome());
            }
            else {
                targetPos = new Vector3(playerPos.x, ca.position.y + 4, playerPos.z);
                SetTargetPoint();
                StopCoroutine(move);
                move = MoveToPosition();
                StartCoroutine(MoveToPosition());
            }
            DropBranch();
            ChangeDirection();
            ChangeSpeed();
            AnimChange();
            Voice();
        }
    }

    void SetTargetPoint()
    {
        // 取得主角移動速度，並設置提前量
        int playerSpeed = (int)player.GetComponent<Rigidbody2D>().velocity.x;
        if(playerSpeed < 0) {
            targetPos.x -= 3;
        }
        else if(playerSpeed > 0) {
            targetPos.x += 3;
        }

        if(playerPos.x >= birdPoint[3].position.x && !isFinal) { isFinal = true; }
        }

    IEnumerator MoveToPosition()
    {
        // 鳥的移動有延遲(慢主角1秒移動)
        if (isPoint) {
            yield return new WaitForSeconds(1.0f);
            isPoint = false;
        }
        else {
            while (transform.position != targetPos) {
                transform.localPosition = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
                yield return 0;
            }
        }
    }

    IEnumerator MoveToHome()
    {
        if (transform.position == targetPos) {
            branch.GetComponent<Branch>().Reset();
            isDrop = false;
        }
        else {
            while (transform.position != targetPos) {
                transform.localPosition = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
                yield return 0;
            }
        }
        yield return 0;
    }

    void ChangeSpeed()
    {
        // 在鏡頭外以超高速移動
        if(Mathf.Abs(transform.position.y - ca.position.y) > 8) {
            if(Mathf.Abs(transform.position.x - ca.position.x) > 12) {
                speed = 2.0f;
            }
        }
        else {
            speed = 0.1f;
        }
    }

    void Voice()
    {
        if(voiceCnt == 500) {
            au.PlayOneShot(voice[1]);
            voiceCnt = 0;
        }
        voiceCnt++;
    }

    void ChangeDirection()
    {
        // 如果目標位置在鳥的左邊，則鳥面向左，反之為右
        if (transform.position.x > targetPos.x) {
            dir = Direction.LEFT;
        }
        else {
            dir = Direction.RIGHT;
        }

        // 轉向
        if (dir == Direction.LEFT) {
            transform.localScale = new Vector3(0.6f, 0.6f, 1);
        }
        else {
            transform.localScale = new Vector3(-0.6f, 0.6f, 1);
        }
    }

    void DropBranch()
    {
        if (Mathf.Abs(transform.position.x - targetPos.x) <= 3.0f) {
            isPoint = true;
            if (timeCnt == 50) {
                au.PlayOneShot(voice[1]);
                branch.GetComponent<Branch>().isDrop = true;
                branch.transform.position = new Vector3(targetPos.x, branch.transform.position.y, 0);
                //Debug.Log(timeCnt);
                isDrop = true;
                timeCnt = 0;
            }

            if (!isDrop) { timeCnt++; }
        }
    }

    void AnimChange()
    {
        if (isDrop) {
            anim.SetBool("isFront", false);
            anim.SetBool("isSide", true);
        }
        else {
            if (transform.position != targetPos) {
                anim.SetBool("isSide", true);
                anim.SetBool("isIdle", false);
            }
            else {
                anim.SetBool("isFront", true);
                anim.SetBool("isSide", false);
            }
        }
    }
}
