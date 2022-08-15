using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public enum STATE
    {
        STOP,
        LEFT,
        RIGHT,
        JUMP,
        FALL,
    }

    public static int Hp{get; set;} = 0;
    int damageMax = 100;

    public GameObject umbrella;
    public GameObject bullet;
    public Slider hp;
    [SerializeField]private Image hpGauge;
    [SerializeField]private Sprite[] hpGauges;
    [SerializeField]private GameObject doorExplanationImage;

    [SerializeField] private static LayerMask groundLayer = 1 << 3;
    [Header("移動速度/傘の傾ける速さ")]
    [SerializeField]private float moveSpeed = 1f;
    [SerializeField]private float noUmbrellaSpeed = 2f;
    [SerializeField]private int umbrellaAngleSpeed = 4;

    private Rigidbody2D rb;
    private Animator anim;
    private static STATE state = STATE.RIGHT;
    private static float x = 0;
    private float leftEdge = -12.5f;
    private static int umbrellaAngle = 0;
    private static int umbrellaAngleMax = 60;
    private static int fixUmbrellaAngle = 90;
    private static int bulletCount = 1000;
    private static int score;
    private static int damage;

    private Vector3 mousePosScreen;
    private Vector3 mousePos;
    private float deg;

    float slopeX = 0.5f;
    float slopeY = 1.25f;
    float y;

    private bool isGround = true;
    private bool isSlope = false;
    private bool isJump = false;

    [Header("重力/ジャンプの長さ")]
    [SerializeField]private float vy = 5f; // 重力
    [SerializeField]float jumpTimeMax = 0.2f;
    private Vector3 gravity;
    private float jumpSpeed = 5f;
    private float jumpPower = 5f;
    float jumpTime = 0;

    public bool isEvent = false;
    private bool catFlag = false; // 本番用を書いてから変更するか考える
    private int catEventCount = 0;
    private int doorExplanationEventCount = 0;

    private static bool isWoodHouse = false;
    private Vector3 housePos = new Vector3(9f, -2.2f);
    private float roomLeftEdge = -3f;
    private float roomRightEdge = 16f;

    void Start()
    {
        Initialize();
    }

    void Update()
    {
        x = 0;
        y = 0;

        CheckRay();

        if (transform.position.x > leftEdge) {
            if (Input.GetKey(KeyCode.A)) { x = -1; state = STATE.LEFT; }
        }
        
        if (Input.GetKey(KeyCode.D)) { x = 1; state = STATE.RIGHT; }
        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) { x = 0; state = STATE.STOP; }

        
        CheckEvent();
        ChangeAnim();
    }

    void FixedUpdate()
    {
        if (!isEvent)
        {
            Movement();
            if (umbrella != null) { MoveUmbrella(); }
            else {moveSpeed = noUmbrellaSpeed; }
            Shot();
        } else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

// -----------------------------------------------------------------------------------------------------
// -----------------------------------------------------------------------------------------------------
// -----------------------------------------------------------------------------------------------------
    void Initialize()
    {
        if (isWoodHouse) {
            transform.position = housePos;
        }
        AddDamage(Hp);
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        CheckRay();
    }

    void Movement() // CatEvent時にジャンプしながら触れると上昇し続けるのを修正する
    {
        if (x == 0) { y = 0; }

        rb.velocity = new Vector3(x, 0, 0) * moveSpeed + new Vector3(0, y, 0) + gravity;

        if (gravity.y < 0) {
            state = STATE.FALL;
        } else {
            state = STATE.STOP;
        }
        if (isJump)
        {
            if (jumpTime < jumpTimeMax)
            {
                state = STATE.JUMP;
                jumpTime += Time.deltaTime;
                jumpSpeed *= 0.99f;
                rb.velocity = new Vector3(rb.velocity.x + 0, rb.velocity.y + jumpSpeed, 0);
            }
        } else
        {
            jumpTime = 0;
            jumpSpeed = jumpPower;
        }


        ChangeAnim();
    }

    void MoveUmbrella()
    {
        mousePosScreen = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePosScreen);
        deg = Mathf.Atan2(umbrella.transform.position.y - mousePos.y,umbrella.transform.position.x - mousePos.x);
        umbrellaAngle = (int)(deg * Mathf.Rad2Deg) + fixUmbrellaAngle;

        if (umbrellaAngle > umbrellaAngleMax && umbrellaAngle <= 180) { umbrellaAngle = umbrellaAngleMax; }
        if (umbrellaAngle > -umbrellaAngleMax && umbrellaAngle > 180) { umbrellaAngle = -umbrellaAngleMax; }

        float posY = transform.position.y + 0.9f;
        umbrella.transform.position = new Vector3(transform.position.x, posY, 0);
        umbrella.transform.RotateAround(transform.position, transform.forward, umbrellaAngle);
        umbrella.transform.rotation = Quaternion.Euler(0, 0, umbrellaAngle);
    }

    void CheckRay() // playerに使う画像が決まり次第調整する
    {
        if (Physics2D.Linecast(
            new Vector2(transform.position.x + 0.2f * transform.localScale.x, transform.position.y),
            new Vector2(transform.position.x + 0.2f * transform.localScale.x, transform.position.y - 1f),
            groundLayer))
        {
            isGround = true;
            anim.SetBool("IsFall", false);
            isJump = false;
            gravity = Vector3.zero;
            if (Input.GetKeyDown(KeyCode.Space) && !isJump)
            {
                isJump = true;
            }
        } else  // 落下
        {
            isGround = false;
            gravity = new Vector3(0, -vy, 0);
        }

        if (Physics2D.Linecast(
            transform.position,
            new Vector2(transform.position.x + 0.5f * transform.localScale.x, transform.position.y - 0.95f),
            groundLayer))
        {
            isSlope = true;
            y = slopeY / slopeX;
        } else
        {
            isSlope = false;
        }

        DebugLine();
    }

    void DebugLine()
    {
        Debug.DrawLine(new Vector2(transform.position.x + 0.2f * transform.localScale.x, transform.position.y),new Vector2(transform.position.x + 0.2f * transform.localScale.x, transform.position.y - 1f), Color.red);
        Debug.DrawLine(transform.position ,new Vector2(transform.position.x + 0.5f * transform.localScale.x, transform.position.y - 0.95f), Color.red);
    }
 
    void Shot()
    {
        if(bulletCount > 0 && (Input.GetKeyDown(KeyCode.Z) || Input.GetMouseButtonDown(0))) {
            bulletCount--;
            Instantiate(bullet, transform.position, Quaternion.Euler(0, 0, -transform.localScale.x * 90));
        }
    }

    void ChangeAnim()
    {
        Debug.Log(state);
        switch (state) {
            case STATE.STOP:
                ChangeAnimStop();
                break;
            case STATE.LEFT:
                if(!isJump) {transform.localScale = new Vector3(-1, 1, 1);}
                anim.SetBool("IsWalk", true);
                break;
            case STATE.RIGHT:
                if(!isJump){transform.localScale = Vector3.one;}
                anim.SetBool("IsWalk", true);
                break;
            case STATE.JUMP:
                anim.SetBool("IsWalk", false);
                anim.SetBool("IsJump", true);
                break;
            case STATE.FALL:
                anim.SetBool("IsFall", true);
                break;
        }
    }

    void ChangeAnimStop()
    {
        anim.SetBool("IsWalk", false);
        anim.SetBool("IsJump", false);
        anim.SetBool("IsFall", false);
    }

    void CheckEvent()
    {
        if (catFlag) {
            if (Input.GetKeyDown(KeyCode.T))
            {
                isWoodHouse = true;
                SceneManager.LoadScene("WoodHouse");
            } // test
            StartCoroutine(WaitCatEvent(catFlag));
        }
    }

    void ChangeHpGauge()
    {
        if (Hp < 20) {hpGauge.sprite = hpGauges[0]; return; }
        if (Hp < 40) {hpGauge.sprite = hpGauges[1]; return; }
        if (Hp < 60) {hpGauge.sprite = hpGauges[2]; return; }
        if (Hp < 80) {hpGauge.sprite = hpGauges[3]; return; }
        if (Hp < 100) {hpGauge.sprite = hpGauges[4]; return; }
    }

    public void AddDamage(int damage)
    {
        Hp += damage;
        ChangeHpGauge();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (catEventCount == 0 && other.gameObject.CompareTag("EventCat"))
        {
            ChangeAnimStop();
            catEventCount++;
            catFlag = true;
            isEvent = true;
        }

        // ここはgameControllerのほうがいい
        if (doorExplanationEventCount == 0 && other.gameObject.CompareTag("EventDoor"))
        {
            doorExplanationEventCount++;
            doorExplanationImage.SetActive(true);
        }
        
        if (other.gameObject.CompareTag("DropRain") || other.gameObject.CompareTag("DropWater")) {
            score += 1;
            damage = 1;
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("DropItem")) {
            score += 10;
            damage = 10;
            Destroy(other.gameObject);
        }
        AddDamage(damage);
    }

    void OnTriggerStay2D(Collider2D other) {
        if (Input.GetKeyDown(KeyCode.S)) {
            isWoodHouse = true;
            SceneManager.LoadScene("WoodHouse");
        }
    }

    IEnumerator WaitCatEvent(bool flag)
    {
        yield return new WaitForSeconds(1.5f);
        catFlag = false;
        isEvent = false;
        yield return null;
    }
}

// changeAnimとかOnTriggerEnter内は関数にする