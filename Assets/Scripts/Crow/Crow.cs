using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crow : MonoBehaviour
{
    public enum Direction { LEFT, RIGHT }; // 烏鴉的左右重置點(會兩邊來回)

    public Transform player, ca;
    public Transform[] birdPoint;
    public GameObject branch;
    public bool isStart, isEvent, isDrop, isFall, isClose, isFront, isPoint, canAttact, isStop;
    public float speed, distanceX;
    public int timeCnt, voiceCnt;
    public Direction dir;

    Animator anim;
    AudioSource au;
    Rigidbody2D rb;
    [SerializeField]Vector3 playerPos, targetPos;

    // Start is called before the first frame update
    void Start()
    {
        // 碰觸烏鴉的開始點 -> 遇見烏鴉動畫 -> 烏鴉追擊
        anim = GetComponent<Animator>();
        au = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        isStart = false;    // 碰見烏鴉之後 true
        isEvent = false;    // 烏鴉動畫期間 true
        isDrop = true;     // 烏鴉丟下樹枝 true
        isFall = false;     // 烏鴉落下動畫 true
        isClose = false;    // 角色接近烏鴉
        isFront = false;    // 烏鴉位於頭頂 true
        isPoint = false;    // 烏鴉移動到定點否 
        canAttact = false;   // 攻擊冷卻時間
        isStop = false;     // 停止攻擊
        dir = Direction.LEFT; // 烏鴉初期設定在左側(目標為右)
    }

    // Update is called once per frame
    void Update()
    {
        ModeCheck();

        if (isEvent)
        {
            Event_UpDate();
        }
        else if(isStart)
        {
            Normal_UpDate();
            Normal_DropBranch();
        }
        voiceCnt++;
    }

    private void FixedUpdate()
    {
        if (isEvent)
        {
            StartCoroutine(Event_Move());
        }
        else if(isStart)
        {
            StartCoroutine(Normal_Move());
        }
    }

    void Event_UpDate()
    {
        Event_PositionCheck();
        //StartCoroutine(Event_Move());
        Event_AnimChange();
    }
    IEnumerator Event_Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        if (speed > 1.5f) { speed *= 0.995f; } // 速度遞減

        //timeCnt++;
        //Debug.Log("move");
        yield return 0;
    }
    void Event_PositionCheck()
    {
        // 檢測玩家與鳥的距離(<3讓鳥離開)
        if (birdPoint[1].position.x - player.position.x < 3 && !isClose)
        {
            isClose = true;
            speed = 10.0f;
            transform.localScale = new Vector3(-0.6f, 0.6f, 1);
            targetPos += new Vector3(8, 12, 0);
        }else if(birdPoint[1].position.x - player.position.x < 7)
        {
            if(voiceCnt >= 100)
            {
                Voice_Volumn();
                voiceCnt = 0;
            }
        }
    }
    void Event_AnimChange()
    {
        //切換動畫
        if (transform.position != targetPos )//|| (transform.position.x - player.position.x < 7 && timeCnt % 80 < 40))
        {
            isFall = true;
        }
        else
        {
            isFall = false;
        }

        if (isFall)
        {
            anim.SetBool("isFall", true);
            anim.SetBool("isIdle", false);
        }
        else
        {
            anim.SetBool("isFall", false);
            anim.SetBool("isIdle", true);
        }
    }

    void Normal_UpDate()
    {
        Normal_PositionCheck();
        Normal_DirectionCheck();
      //StartCoroutine(Normal_Move());
        Normal_DropBranch();
        Normal_AnimChange();

        if (voiceCnt >= 100)
        {
            Voice_Volumn();
            voiceCnt = 0;
        }
    }

    void Normal_PositionCheck()
    {
        // 烏鴉在左，則目標點設為右，都不是則目標設為玩家
        if (dir == Direction.LEFT && isDrop)
        {
            targetPos = birdPoint[4].position;
            if (Vector3.Distance(transform.position, birdPoint[4].position) < 0.1f)
            {
                StartCoroutine(AttactWait());
                branch.GetComponent<Branch>().Reset();
                dir = Direction.RIGHT;
                isDrop = false;
            }
        }
        else if (dir == Direction.RIGHT && isDrop)
        {
            targetPos = birdPoint[3].position;
            if (Vector3.Distance(transform.position, birdPoint[3].position) < 0.1f)
            {
                StartCoroutine(AttactWait());
                branch.GetComponent<Branch>().Reset();
                dir = Direction.LEFT;
                isDrop = false;
            }
        }
        else
        {
            // 取得角色速度預設提前量
            if (canAttact && !isStop)
            {
                float playerSpeed = player.gameObject.GetComponent<Rigidbody2D>().velocity.x;
                if (playerSpeed > 0)
                {
                    targetPos = new Vector3(player.position.x + 1.0f, player.position.y + 6.0f, 0);
                }
                else if (playerSpeed < 0)
                {
                    targetPos = new Vector3(player.position.x - 0.8f, player.position.y + 6.0f, 0);
                }
                else
                {
                    targetPos = new Vector3(player.position.x, player.position.y + 6.0f, 0);
                }
            }
        }


        // 在鏡頭範圍內減速
        if (Mathf.Abs(transform.position.x - ca.position.x) > 13 || Mathf.Abs(transform.position.y - ca.position.y) > 8)
        {
            speed = 4.0f;
        }
        else
        {
            if(distanceX < 3)
            {
                speed = 0.1f;
            }
            else
            {
                speed = 0.08f;
            }
        }

        // 如果距離主角x軸<4且未丟下樹枝則轉向正面
        if (distanceX < 3 && !isDrop)
        {
            isFront = true;
            return;
        }
        isFront = false;
    }
    IEnumerator AttactWait()
    {
        // 兩次攻擊間隔
        yield return new WaitForSeconds(0.2f);
        canAttact = true;
    }

    void Normal_DirectionCheck()
    {
        if (dir == Direction.LEFT)
        {
            transform.localScale = new Vector3(-0.6f, 0.6f, 1);
        }
        else if (dir == Direction.RIGHT)
        {
            transform.localScale = new Vector3(0.6f, 0.6f, 1);
        }
    }
    IEnumerator Normal_Move()
    {
        if (isPoint)
        {
            //Debug.Log("wait");
            yield return new WaitForSeconds(1.0f);
            isPoint = false;
        }
        else
        {
            while (transform.position != targetPos)
            {
                //Debug.Log("move");
                transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
                yield return new WaitForSeconds(0.01f);
            }
        }
    }
    void Normal_DropBranch()
    {
        // 烏鴉在投放範圍內時開始計算150幀
        if (distanceX <= 2 && !isDrop)
        {
            //Debug.Log(distanceX);
            isPoint = true;
            timeCnt++;

            if (timeCnt >= 100)
            {
                //Voice_Volumn();
                branch.GetComponent<Branch>().isDrop = true; // 修改樹枝狀態為落下
                branch.transform.position = new Vector3(targetPos.x, branch.transform.position.y, 0); // 調整樹枝位置至提前量
                isDrop = true; // 已將樹枝丟下
                canAttact = false;
                timeCnt = 0; // 重製計時
            }
        }

        if(isStop && !isDrop)
        {
            au.Play();
            branch.GetComponent<Branch>().isDrop = true;
            branch.transform.position = new Vector3(0, 100, 0);
            isDrop = true;
            canAttact = false;
            timeCnt = 0;
        }
    }
    void Normal_AnimChange()
    {
        anim.SetBool("isSide", true);
        anim.SetBool("isIdle", false);

        if (isDrop)
        {
            anim.SetBool("isFront", false);
            anim.SetBool("isSide", true);
        }
        else
        {
            if (distanceX < 5.0f)
            {
                anim.SetBool("isFront", true);
                anim.SetBool("isSide", false);
            }
            else
            {
                anim.SetBool("isSide", true);
                anim.SetBool("isIdle", false);
            }
        }
    }

    void ModeCheck()
    {
        // 取得與角色之距離
        distanceX = Mathf.Abs(transform.position.x - player.position.x);

        // 通過開始點 進入動畫模式
        if (player.position.x > birdPoint[0].position.x && !isStart)
        {
            isStart = true;
            isEvent = true;

            // 此處順便進行初始化
            speed = 8.0f; // 烏鴉動畫移動速度
            targetPos = birdPoint[1].position; // 移動目標點
        }

        // 檢測第二事件點(開始丟樹枝)
        if (player.position.x > birdPoint[2].position.x && isEvent && Player.instance.GetOutDoor())
        { 
            isEvent = false;
            branch.gameObject.SetActive(true);
        }
    }

    void Voice_Volumn()
    {
        // 取得角色與烏鴉的距離(圓)
        float distance = Vector2.Distance(transform.position, player.position);
        au.volume = 0.5f - ((distance - 7) / 7.0f);
        au.Play();
        //Debug.Log(distance);
    }
}
