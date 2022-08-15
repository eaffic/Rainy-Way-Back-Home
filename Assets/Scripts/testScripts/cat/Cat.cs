using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Cat : MonoBehaviour
{

    public enum POSITION
    {
        LEFT,
        RIGHT,
        POS0,
        POS1,
        POS2,
        POS3,
        POS4, // 追加
    }
    // jumpPosの修正

    [SerializeField] GameObject player; // 追加
    [SerializeField] GameObject stone;
    [SerializeField] int Hp { get; set; } = 10;
    [SerializeField] Slider angry; // 追加
    [SerializeField] AudioClip[] SE;
    Vector3 posRight;
    Vector3 posLeft;
    float speed = 7f;
    float jumpTime = 1.2f;
    POSITION pos;
    int randPosInt;
    Rigidbody2D rb;
    Animator anim;
    AudioSource audio;

    int nextPos;
    int nowPos;

    Vector3 offset;
    Vector3 target;
    float deg;

    [SerializeField] GameObject[] JumpPos;
    Vector3[] randPos = new Vector3[5];

    float vy;
    float prevVy;

    bool isMove = false;
    int hp = 1;
    bool isAngry = false;

    float playerPosX;
    float prevPlayerPosX;

    bool isStop = false;
    bool isOnce = false;

    public static Cat instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        posRight = new Vector3(18f, -2.47f, 0);
        posLeft = new Vector3(-2f, -2.47f, 0);

        for (int i = 0; i < randPos.Length; i++)
        {
            randPos[i] = JumpPos[i].transform.position;
        }
        pos = POSITION.RIGHT;
        if (!angry == null)
        {
            angry.value = 0;
        }

        if (Player.outDoor) 
        {
            angry.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (isStop)
        {
            angry.gameObject.SetActive(false);
        }
        OnceMove();

        if (!isMove && angry.value > 0.95f)
        {
            isAngry = true;
            isOnce = true;
            StartCoroutine("Move");
        }

        CheckPlayerPosition();
        JumpAnim();
        ChangeAnim();
    }

    void OnceMove()
    {
        if (playerPosX > 8.5f && !isOnce)
        {
            isOnce = true;
            angry.value = 1;
        }
    }

    public float GetAngry()
    {
        return angry.value;
    }

    public bool getIsAngry()
    {
        return isAngry;
    }

    void AddAngry()
    {
        if (playerPosX > -2.5f && !isMove && !isStop)
        {
            angry.value += 0.012f;
        }
    }

    void SubtractAngry()
    {
        if (isAngry)
        {
            angry.value -= 0.0015f;
        }
        else
        {
            angry.value -= 0.0075f;
        }
    }

    void CheckPlayerPosition()
    {
        playerPosX = player.transform.position.x;
        if (prevPlayerPosX == playerPosX)
        {
            SubtractAngry();
        }
        else
        {
            AddAngry();
        }
        prevPlayerPosX = playerPosX;
    }

    // 着地した後にpos変更
    IEnumerator Move()
    {
        isMove = true;
        jumpTime = 1.05f;
        audio.PlayOneShot(SE[1]);
        yield return new WaitForSeconds(1f);
        // 攻撃パターン
        switch (pos)
        {
            case POSITION.RIGHT:
                StartCoroutine(RushSet(posLeft, POSITION.LEFT));
                break;
            case POSITION.LEFT:
                StartCoroutine(RushSet(posRight, POSITION.RIGHT));
                break;
        }
        yield return new WaitForSeconds(2f);

        // int randJumpTime = Random.Range(1, 5);
        //
        // 岩へのランダムジャンプ
        // jumpTime = 1.2f;
        // for (int i = 0; i < randJumpTime; i++)
        // {
        //     randPosInt = Random.Range(0, randPos.Length);

        //     StartCoroutine(Jump(randPos[randPosInt], jumpTime));
        //     nowPos = nextPos;

        //     float stoneTime = 1.2f;
        //     yield return new WaitForSeconds(stoneTime); // 岩を落とすまでの時間
        //     StartCoroutine(DropStone(randPos[randPosInt]));
        //     yield return new WaitForSeconds(4f - stoneTime); // 次の行動までの時間
        // }

        float stoneTime = 1.05f;
        int randMove = Random.Range(0, 2);
        int jumpStone = Random.Range(0, 3);
        if (pos == POSITION.LEFT)
        {
            StartCoroutine(Jump(randPos[0], jumpTime));
            yield return new WaitForSeconds(stoneTime);
            StartCoroutine(DropStone(randPos[0], Random.Range(3, 7)));
            yield return new WaitForSeconds(1.5f);
            if (jumpStone == 0 || jumpStone == 2) { StartCoroutine(DropStone(randPos[0], Random.Range(1, 3))); }

            if (randMove == 0)
            {
                StartCoroutine(Jump(randPos[1], jumpTime));
                yield return new WaitForSeconds(stoneTime);
                StartCoroutine(DropStone(randPos[1], Random.Range(2, 5)));
                yield return new WaitForSeconds(1.0f);
                if (jumpStone == 1 ) { StartCoroutine(DropStone(randPos[1], Random.Range(1, 3))); }

                StartCoroutine(Jump(randPos[2], jumpTime));
                yield return new WaitForSeconds(stoneTime);
                StartCoroutine(DropStone(randPos[2], 4));
                yield return new WaitForSeconds(1.8f);
                StartCoroutine(DropStone(randPos[2], Random.Range(1, 3)));
            }
            else
            {
                StartCoroutine(Jump(randPos[2], jumpTime));
                yield return new WaitForSeconds(stoneTime);
                StartCoroutine(DropStone(randPos[2], 4));
                yield return new WaitForSeconds(1.8f);
                if (jumpStone == 1 || jumpStone == 2) { StartCoroutine(DropStone(randPos[2], Random.Range(1, 3))); }

                StartCoroutine(Jump(randPos[3], jumpTime));
                yield return new WaitForSeconds(stoneTime);
                StartCoroutine(DropStone(randPos[3], Random.Range(2, 5)));
                yield return new WaitForSeconds(1.0f);
                StartCoroutine(DropStone(randPos[3], Random.Range(1, 3)));
                if (jumpStone == 0) { StartCoroutine(DropStone(randPos[3], Random.Range(1, 3))); }
            }

            StartCoroutine(Jump(randPos[4], jumpTime));
            yield return new WaitForSeconds(stoneTime);
            StartCoroutine(DropStone(randPos[4], Random.Range(4, 7)));
            yield return new WaitForSeconds(1.5f);
            StartCoroutine(DropStone(randPos[4], Random.Range(1, 3)));
        }
        else
        {
            StartCoroutine(Jump(randPos[4], jumpTime));
            yield return new WaitForSeconds(stoneTime);
            StartCoroutine(DropStone(randPos[4], Random.Range(3, 7)));
            yield return new WaitForSeconds(1.5f);
            if (jumpStone == 0 || jumpStone == 2) { StartCoroutine(DropStone(randPos[4], Random.Range(1, 3))); }

            if (randMove == 0)
            {
                StartCoroutine(Jump(randPos[3], jumpTime));
                yield return new WaitForSeconds(stoneTime);
                StartCoroutine(DropStone(randPos[3], Random.Range(2, 6)));
                yield return new WaitForSeconds(1.0f);
                if (jumpStone == 1) { StartCoroutine(DropStone(randPos[3], Random.Range(1, 3))); }

                StartCoroutine(Jump(randPos[2], jumpTime));
                yield return new WaitForSeconds(stoneTime);
                StartCoroutine(DropStone(randPos[2], 4));
                yield return new WaitForSeconds(1.8f);
                StartCoroutine(DropStone(randPos[2], Random.Range(1, 3)));
            }
            else
            {
                StartCoroutine(Jump(randPos[2], jumpTime));
                yield return new WaitForSeconds(stoneTime);
                StartCoroutine(DropStone(randPos[2], 4));
                yield return new WaitForSeconds(1.8f);
                if (jumpStone == 1 || jumpStone == 2) { StartCoroutine(DropStone(randPos[2], Random.Range(1, 3))); }

                StartCoroutine(Jump(randPos[1], jumpTime));
                yield return new WaitForSeconds(stoneTime);
                StartCoroutine(DropStone(randPos[1], Random.Range(2, 6)));
                yield return new WaitForSeconds(1.0f); 
                StartCoroutine(DropStone(randPos[1], Random.Range(1, 3)));
                if (jumpStone == 0) { StartCoroutine(DropStone(randPos[1], Random.Range(1, 3))); }
            }

            StartCoroutine(Jump(randPos[0], jumpTime));
            yield return new WaitForSeconds(stoneTime);
            StartCoroutine(DropStone(randPos[0], Random.Range(4, 7)));
            yield return new WaitForSeconds(1.5f);
            StartCoroutine(DropStone(randPos[0], Random.Range(1, 3)));
        }

        //int randDir = Random.Range(0, 2); // 0-left, 1-right
        //randPosInt = Random.Range(1, 3);
        //StartCoroutine(Jump(randPos[2], jumpTime));
        //yield return new WaitForSeconds(stoneTime);
        //StartCoroutine(DropStone(randPos[2]));
        //yield return new WaitForSeconds(4f - stoneTime);
        //if (randDir == 0)
        //{
        //    pos = POSITION.LEFT;
        //}
        //else
        //{
        //    pos = POSITION.RIGHT;
        //}
        //int end = pos == POSITION.RIGHT ? 0 : 4;
        //StartCoroutine(Jump(randPos[end], jumpTime));
        //// go endPos
        //yield return new WaitForSeconds(stoneTime);
        //StartCoroutine(DropStone(randPos[end]));
        //yield return new WaitForSeconds(4f - stoneTime);

        POSITION endJumpPos = pos == POSITION.RIGHT ? POSITION.POS0 : POSITION.POS4;
        // 地面に着地
        jumpTime = 2f;
        int n = endJumpPos == POSITION.POS0 ? 0 : 1;
        if (n == 0)
        {
            StartCoroutine(Jump(posLeft, 0.9f));
            yield return new WaitForSeconds(1.5f);
            pos = POSITION.LEFT;
        }
        if (n == 1)
        {
            StartCoroutine(Jump(posRight, 0.9f));
            yield return new WaitForSeconds(1.5f);
            pos = POSITION.RIGHT;
        }

        if (Random.Range(1, 100) < 75)
        {
            switch (pos)
            {
                case POSITION.RIGHT:
                    StartCoroutine(RushSet(posLeft, POSITION.LEFT));
                    break;
                case POSITION.LEFT:
                    StartCoroutine(RushSet(posRight, POSITION.RIGHT));
                    break;
            }
            yield return new WaitForSeconds(1.0f);
        }
        

        yield return new WaitForSeconds(0.5f);
        isMove = false;
        isAngry = false;
    }

    IEnumerator DropStone(Vector3 point, int num)
    {
        for (int i = 0; i < num; i++)
        {
            float randTime = Random.Range(0, 0.15f);
            float randScale = Random.Range(0.7f, 1.4f);
            {
                GameObject obj = Instantiate(stone, new Vector3(point.x + randTime, point.y - 0.5f, point.z), Quaternion.identity);
                obj.transform.localScale = new Vector3(randScale, randScale, 1);
                yield return new WaitForSeconds(Mathf.Abs(randTime));
            }
        }
    }

    IEnumerator Rush(Vector3 endPos, float time)
    {
        var startPos = transform.position;
        var x = endPos.x - startPos.x;

        for (var t = 0f; t < time; t += Time.deltaTime)
        {
            transform.position = new Vector3(transform.position.x + x / (time / Time.deltaTime), transform.position.y, transform.position.z);
            yield return null;
        }
        transform.position = new Vector3(endPos.x, transform.position.y, transform.position.z);
        anim.SetBool("isRush", false);
    }

    IEnumerator RushSet(Vector3 endPos, POSITION p)
    {
        anim.SetBool("isRush", true);
        StartCoroutine(Rush(endPos, 1f));
        yield return new WaitForSeconds(1f);
        pos = p;
    }

    IEnumerator Jump(Vector3 endPos, float time)
    {
        var startPos = transform.position; // 初期位置
        var diffY = (endPos - startPos).y; // 始点と終点のy成分の差分
        var vs = (diffY - -9.8f * 0.5f * time * time) / time; // 鉛直方向の初速度vsinθ
                                                              // 放物運動
        for (var t = 0f; t < time; t += Time.deltaTime)
        {
            var p = Vector3.Lerp(startPos, endPos, t / time);   //水平方向の座標を求める (x,z座標)
            p.y = startPos.y + vs * t + 0.5f * -9.8f * t * t; // 鉛直方向の座標 y
            transform.position = p;
            yield return null; //1フレーム経過
        }
        // 終点座標へ補正 大事
        transform.position = endPos;
    }

    void ChangeAnim()
    {
        switch (pos)
        {
            case POSITION.LEFT:
                transform.localScale = new Vector3(1, 1, 1);
                break;
            case POSITION.RIGHT:
                transform.localScale = new Vector3(-1, 1, 1);
                break;
        }
    }

    void JumpAnim()
    {
        vy = transform.position.y - prevVy;
        if (vy > 0)
        {
            JumpTrueAnim();
        }
        else if (vy < 0)
        {
            FallTrueAnim();
        }
        else
        {
            StartCoroutine("ChangeIdle");
        }

        prevVy = transform.position.y;
    }
    IEnumerator ChangeIdle()
    {
        FallTrueAnim();
        yield return null;
        JumpFalseAnim();
    }
    void JumpTrueAnim()
    {
        anim.SetBool("isJump", true);
    }
    void JumpFalseAnim()
    {
        anim.SetBool("isJump", false);
        anim.SetBool("isFall", false);
    }
    void FallTrueAnim()
    {
        anim.SetBool("isJump", true);
        anim.SetBool("isFall", true);
    }

    public void Catch()
    {
        hp--;
        if (hp == 0)
        {
            Player.instance.ClearWoodHouse();
            audio.PlayOneShot(SE[0]);
            StartCoroutine("CatEventEnd");
        }
    }

    IEnumerator CatEventEnd()
    {
        isStop = true;

        yield return new WaitForSeconds(4f);

        transform.localScale = new Vector3(-1, 1, 1);
        StartCoroutine(RushSet(new Vector3(-15f, transform.position.y, 0), pos));
        yield return new WaitForSeconds(1f);

        EventManager.instance.EscapeCatText();
        yield return new WaitForSeconds(1.0f);
        gameObject.SetActive(false);
    }

    public bool GetStop()
    {
        return isStop;
    }
}


