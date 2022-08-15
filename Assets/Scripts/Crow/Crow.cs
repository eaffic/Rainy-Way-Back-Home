using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crow : MonoBehaviour
{
    public enum Direction { LEFT, RIGHT }; // �Q�~�����k���m�I(�|����Ӧ^)

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
        // �IĲ�Q�~���}�l�I -> �J���Q�~�ʵe -> �Q�~�l��
        anim = GetComponent<Animator>();
        au = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        isStart = false;    // �I���Q�~���� true
        isEvent = false;    // �Q�~�ʵe���� true
        isDrop = true;     // �Q�~��U��K true
        isFall = false;     // �Q�~���U�ʵe true
        isClose = false;    // ���Ⱶ��Q�~
        isFront = false;    // �Q�~����Y�� true
        isPoint = false;    // �Q�~���ʨ�w�I�_ 
        canAttact = false;   // �����N�o�ɶ�
        isStop = false;     // �������
        dir = Direction.LEFT; // �Q�~����]�w�b����(�ؼЬ��k)
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
        if (speed > 1.5f) { speed *= 0.995f; } // �t�׻���

        //timeCnt++;
        //Debug.Log("move");
        yield return 0;
    }
    void Event_PositionCheck()
    {
        // �˴����a�P�����Z��(<3�������})
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
        //�����ʵe
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
        // �Q�~�b���A�h�ؼ��I�]���k�A�����O�h�ؼг]�����a
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
            // ���o����t�׹w�]���e�q
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


        // �b���Y�d�򤺴�t
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

        // �p�G�Z���D��x�b<4�B����U��K�h��V����
        if (distanceX < 3 && !isDrop)
        {
            isFront = true;
            return;
        }
        isFront = false;
    }
    IEnumerator AttactWait()
    {
        // �⦸�������j
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
        // �Q�~�b���d�򤺮ɶ}�l�p��150�V
        if (distanceX <= 2 && !isDrop)
        {
            //Debug.Log(distanceX);
            isPoint = true;
            timeCnt++;

            if (timeCnt >= 100)
            {
                //Voice_Volumn();
                branch.GetComponent<Branch>().isDrop = true; // �ק��K���A�����U
                branch.transform.position = new Vector3(targetPos.x, branch.transform.position.y, 0); // �վ��K��m�ܴ��e�q
                isDrop = true; // �w�N��K��U
                canAttact = false;
                timeCnt = 0; // ���s�p��
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
        // ���o�P���⤧�Z��
        distanceX = Mathf.Abs(transform.position.x - player.position.x);

        // �q�L�}�l�I �i�J�ʵe�Ҧ�
        if (player.position.x > birdPoint[0].position.x && !isStart)
        {
            isStart = true;
            isEvent = true;

            // ���B���K�i���l��
            speed = 8.0f; // �Q�~�ʵe���ʳt��
            targetPos = birdPoint[1].position; // ���ʥؼ��I
        }

        // �˴��ĤG�ƥ��I(�}�l���K)
        if (player.position.x > birdPoint[2].position.x && isEvent && Player.instance.GetOutDoor())
        { 
            isEvent = false;
            branch.gameObject.SetActive(true);
        }
    }

    void Voice_Volumn()
    {
        // ���o����P�Q�~���Z��(��)
        float distance = Vector2.Distance(transform.position, player.position);
        au.volume = 0.5f - ((distance - 7) / 7.0f);
        au.Play();
        //Debug.Log(distance);
    }
}
