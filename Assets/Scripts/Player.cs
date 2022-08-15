using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D coll;

    public static int Damage { get; set; } = 0;

    public Game_SceneChange sceneChangeGame;
    public WoodHouse_ChangeScene sceneChangeWood;
    public GameObject damageLight;
    public GameObject[] gameSceneTu;
    public GameObject[] woodSceneTu;

    public GameObject umbrella;
    public GameObject bullet;
    public Slider hp;
    [SerializeField] private Image damageGauge;
    [SerializeField] private Sprite[] damageGauges;

    [SerializeField] private int umbrellaAngleSpeed = 4;

    private static float umbrellaAngle = 0;
    private static float umbrellaAngleMax = 80;
    private static float fixUmbrellaAngle = 90;
    private static int bulletCount = 1000;
    [SerializeField] private static int damage;
    private float shotTime = 0;
    private float shotWait = 1f;
    private bool canShot = false;
    public static bool outDoor = false;
    public static bool notCatchCat = false;
    public bool inDoorExperience = false;
    public static bool[] stepCheck = { false, false, false, false, false };

    private Vector3 mousePosScreen;
    private Vector3 mousePos;
    private float deg;

    //------------------------------------
    public bool isEvent = false;
    private bool catFlag = false;
    int catEventTime = 0;
    bool inoperable = false;
    private int catEventCount = 0;
    private int doorExplanationEventCount = 0;

    private static bool isWoodHouse = false;
    private Vector3 housePos = new Vector3(9f, -2.2f);


    public bool eventStart = false;
    //------------------------------------

    public float speed, jumpForce;
    public Transform groundCheck;
    public LayerMask ground;

    public bool isJump, isGround, isItemDamage;
    public bool catCoolTime, isChangeScene;

    bool jumpPressed;
    int jumpCount;

    float inputTime = 0;
    float waitTime = 0;
    bool messageFlg = false;
    public string sceneName;

    string[] hpMessage = {
        "ぬれちゃった",
        "よごれちゃった",
        "ふくがぬれた",
        "びしょびしょだ",
    };

    string[] message = {
        "あし、ちょっとぬれた",
        "....",
        "あめのおと...",
        "ごはんなにかな...",
        "おなかすいたな...",
    };


    public static Player instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        sceneName = SceneManager.GetActiveScene().name;
    }

    void Start()
    {
        ComponentInitialize();
        //stepCheck = new bool[] { false, false, false, false, false };
        isChangeScene = false;
        Damage = GlobalControl.Instance.HP;
        //for (int i = 0; i < Damage / 25; ++i)
        //{
        //    stepCheck[i] = true;
        //}

        if (sceneName == "WoodHouse")
        {
            transform.localScale = new Vector3(1.3f, 1.3f, 1.0f);
            transform.position = new Vector3(-9.7f, -1.7f, 0);
        }
        else if (sceneName == "Game" && notCatchCat)
        {
            inDoorExperience = true;
            GetComponent<Player_Event>().eventCat = true;
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            transform.position = new Vector3(150f, 1.3f, 0);
        }
        else if (sceneName == "Game" && outDoor)
        {
            notCatchCat = false;
            GetComponent<Player_Event>().eventCat = true;
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            transform.position = new Vector3(150f, 1.3f, 0);
        }
        else if(sceneName == "Game")
        {
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            transform.position = new Vector3(-8.5f, -1.8f, 0);
        }
        //StartCoroutine(ShowDebug());
    }

    void Update()
    {
        if (!inoperable)
        {
            if (Input.GetButtonDown("Jump") && jumpCount > 0)
            {
                jumpPressed = true;
            }
            //Shot();
            NotWorking();
        }
        MoveUmbrella();
        SwitchAnim();
        CheckEvent();
        HPCheck();
        ChangeScene();

        if (transform.position.x > 106.0f && transform.position.x < 123.0f)
        {
            transform.position += new Vector3(-0.2f * Time.deltaTime, 0f, 0f);
        }

        if (Input.GetKeyDown(KeyCode.N)) { Damage = 75; }

        // save
        GlobalControl.Instance.HP = Damage;

        // load
        Damage = GlobalControl.Instance.HP;
        //Debug.Log(Damage);
        ChangeDamageGauge();

        //Debug.Log(outDoor);
    }

    private void FixedUpdate()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.1f, ground);
        if (!inoperable)
        {
            GroundMovement();
        }
        Jump();
    }

    void GroundMovement()
    {
        float horizontalMove = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(horizontalMove * speed, rb.velocity.y);

        if(horizontalMove > 0) { horizontalMove = 1; }
        else if(horizontalMove < 0) { horizontalMove = -1; }

        if(horizontalMove != 0) { GetComponent<AudioSource>().volume = 0.8f; }
        else { GetComponent<AudioSource>().volume = 0; }

        if (horizontalMove != 0)
        {
            transform.localScale = sceneName != "WoodHouse" ? new Vector3(horizontalMove, 1, 1) : new Vector3(horizontalMove * 1.3f, 1.3f, 1.3f);
        }

        FixTransform();
    }


    void FixTransform()
    {
        float right = sceneName == "Game" ? 1000f : 24f;
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -12f, right), transform.position.y, 0);
    }

    void Jump()
    {
        if (isGround)
        {
            jumpCount = 1;
            isJump = false;
        }

        if (jumpPressed && isGround)
        {
            isJump = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount--;
            jumpPressed = false;
        }
        else if (jumpPressed && jumpCount > 0 && isJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount--;
            jumpPressed = false;
        }
    }

    void SwitchAnim()
    {
        anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));

        if (isGround)
        {
            anim.SetBool("IsJump", false);
            anim.SetBool("IsFall", false);
        }
        else if (!isGround && rb.velocity.y > 0)
        {
            anim.SetBool("IsJump", true);
        }
        else if (rb.velocity.y < 0)
        {
            anim.SetBool("IsJump", false);
            anim.SetBool("IsFall", true);
        }
    }

    //bool CanShot()
    //{
    //    shotTime += Time.deltaTime;
    //    if (shotTime > shotWait) canShot = true;
    //    if (canShot)
    //    {
    //        shotTime = 0;
    //        return Input.GetKeyDown(KeyCode.Z) || Input.GetMouseButtonDown(0);
    //    }
    //    return false;
    //}

    //void Shot()
    //{
    //    if (!CanShot()) return;

    //    Instantiate(bullet, transform.position, Quaternion.Euler(0, 0, -transform.localScale.x * 90));
    //    canShot = false;
    //}

    void ChangeDamageGauge()
    {
        if (Damage < 25) { damageGauge.sprite = damageGauges[0]; }
        else if (Damage < 50) { damageGauge.sprite = damageGauges[1]; }
        else if (Damage < 75) { damageGauge.sprite = damageGauges[2]; }
        else if (Damage < 100) { damageGauge.sprite = damageGauges[3]; }
        else { damageGauge.sprite = damageGauges[4]; }
    }

    public void AddDamage(int damage)
    {
        if (damage > 0) { damageLight.GetComponent<Animator>().SetTrigger("Damage"); }
        Damage += damage;
        ChangeDamageGauge();
        damageLight.GetComponent<Animator>().SetTrigger("Normal");
    }

    void ComponentInitialize()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
    }

    void MoveUmbrella()
    {
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetButton("UmbrellaL")) { umbrellaAngle -= 0.6f; }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetButton("UmbrellaR")) { umbrellaAngle += 0.6f; }

        float posY = sceneName == "WoodHouse" ? transform.position.y + 1.3f : transform.position.y + 0.9f;
        umbrella.transform.position = new Vector3(transform.position.x, posY, 0);
        umbrella.transform.RotateAround(transform.position, transform.forward, umbrellaAngle);
        umbrella.transform.rotation = Quaternion.Euler(0, 0, umbrellaAngle);
        LimitUmbrellaAngle();

        if (Input.GetKey(KeyCode.G) || Input.GetButton("Gaurd"))
        {
            //float tempAngle = umbrellaAngle;
            //for (float i = tempAngle; i > -60 - tempAngle; --i){
            //	umbrellaAngle -= 1;
            //	posY = transform.position.y + 0.9f;
            //	umbrella.transform.position = new Vector3(transform.position.x, posY, 0);
            //	umbrella.transform.RotateAround(transform.position, transform.forward, umbrellaAngle);
            //	umbrella.transform.rotation = Quaternion.Euler(0, 0, umbrellaAngle);
            //}
            if (transform.localScale.x > 0)
            {
                SetUmbrellaAngle(-umbrellaAngleMax);
            }
            else
            {
                SetUmbrellaAngle(umbrellaAngleMax);
            }
        }
        if (Input.GetKeyUp(KeyCode.G) || Input.GetButtonUp("Gaurd"))
        {
            SetUmbrellaAngle(0);
        }
    }

    void LimitUmbrellaAngle()
    {
        if (umbrellaAngle > umbrellaAngleMax) { umbrellaAngle = umbrellaAngleMax; }
        if (umbrellaAngle < -umbrellaAngleMax) { umbrellaAngle = -umbrellaAngleMax; }
    }

    public void SetUmbrellaAngle(float deg)
    {
        if (deg > Mathf.Abs(deg)) return;
        float posY = transform.position.y + 0.9f;
        umbrella.transform.position = new Vector3(transform.position.x, posY, 0);
        umbrella.transform.RotateAround(transform.position, transform.forward, deg);
        umbrella.transform.rotation = Quaternion.Euler(0, 0, deg);
    }

    void CheckEvent()
    {
        if (catFlag)
        {
            if (catEventTime > 0) { return; }
            catEventTime++;
            rb.velocity = Vector2.zero;
            //anim.Play("Idle");
            anim.SetBool("Jump", false);
            anim.SetBool("Fall", false);
            StartCoroutine(WaitCatEvent(catFlag));

            inoperable = true;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        //if (other.gameObject.CompareTag("EventDoor"))
        //{
        //    if (Input.GetKeyDown(KeyCode.S) || Input.GetButtonDown("Decide"))
        //    {
        //        sceneChangeGame.LoadNextScene(2);
        //        inDoorExperience = true;
        //    }
        //}

        //if (other.gameObject.CompareTag("OutDoor"))
        //{
        //    if (Input.GetKeyDown(KeyCode.S) || Input.GetButtonDown("Decide"))
        //    {
        //        sceneChangeWood.LoadNextScene(1);
        //    }
        //}

        //if (other.gameObject.CompareTag("Cat") && Cat.instance.GetAngry() < 0.8f && (Input.GetKeyDown(KeyCode.S) || Input.GetButtonDown("Decide")))
        //{
        //    Cat.instance.Catch();
        //    StartCoroutine(MoveSceneToGame());
        //}

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        int damage = 0;

        if (catEventCount == 0 && other.gameObject.CompareTag("EventCat"))
        {
            // anim.SetBool("IsFall", false);
            catEventCount++;
            catFlag = true;
            isEvent = true;
            EventManager.instance.EventCat();
        }

        if (doorExplanationEventCount == 0 && other.gameObject.CompareTag("EventDoor"))
        {
            doorExplanationEventCount++;
        }

        if (other.gameObject.CompareTag("DropRain") || other.gameObject.CompareTag("DropWater"))
        {

            damage = 1;
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("DropItem"))
        {
            damage = 6;
            if (!isItemDamage)
            {
                isItemDamage = true;
                MessageManager.instance.ShowMessageText("いたっ！");
            }
            //other.gameObject.SetActive(false); ;
        }

        if (other.gameObject.CompareTag("Cat") && Cat.instance.getIsAngry())
        {
            if (!catCoolTime)
            {
                catCoolTime = true;
                //Debug.Log("hit");
                damage = 8;
                MessageManager.instance.ShowMessageText("わ！");
                BackPlayer(other.gameObject);
                StartCoroutine(CatCoolTimeCount());
            }
        }

        if (other.gameObject.CompareTag("Event_Ending"))
        {
            anim.SetBool("isJump", false);
            GetComponent<Player_Ending>().enabled = true;
        }

        if (other.gameObject.CompareTag("EventDoor"))
        {
            gameSceneTu[0].gameObject.SetActive(true);
        }

        if (other.gameObject.CompareTag("OutDoor"))
        {
            if (!outDoor) { notCatchCat = true; }
            woodSceneTu[0].gameObject.SetActive(true);
        }

        //if (other.gameObject.CompareTag("Cat") && !Cat.instance.getIsAngry())
        //{
        //    woodSceneTu[1].gameObject.SetActive(true);
        //}

        if(damage > 0 && !isChangeScene) { AddDamage(damage); }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("EventDoor"))
        {
            gameSceneTu[0].gameObject.SetActive(false);
        }

        if (other.gameObject.CompareTag("OutDoor"))
        {
            woodSceneTu[0].gameObject.SetActive(false);
        }

        if (other.gameObject.CompareTag("Cat"))
        {
            //woodSceneTu[1].gameObject.SetActive(false);
        }
    }

    IEnumerator WaitCatEvent(bool flg)
    {
        inputTime = 0;
        waitTime = 0;
        yield return new WaitForSeconds(2.0f);
        flg = false;
        isEvent = false;
        inoperable = false;
        inputTime = 0;
        waitTime = 0;
        yield return null;
    }

    void NotWorking()
    {
        if (rb.velocity.x == 0 && rb.velocity.y == 0)
        {
            inputTime += Time.deltaTime;
        }
        else
        {
            inputTime = 0;
        }

        if (messageFlg)
        {
            waitTime += Time.deltaTime;
            if (waitTime > 5.5f)
            {
                waitTime = 0;
                messageFlg = false;
            }
            return;
        }

        if (inputTime > 10.0f && sceneName == "Game")
        {
            messageFlg = true;
            inputTime = 0;
            string msg = message[Random.Range(0, message.Length)];
            MessageManager.instance.ShowMessageText(msg);
        }

    }

    void BackPlayer(GameObject enemy)
    {
        float dir = transform.position.x - enemy.transform.position.x;
        //Debug.Log(dir);
        if (dir > 0)
        {
            transform.position += new Vector3(0.2f, 0, 0);
        }
        else
        {
            transform.position += new Vector3(-0.2f, 0, 0);
        }
        //float power = dir > 0 ? -5f : 5f;
        //rb.AddForce(new Vector2(10f, 1f));
    }

    void HPCheck()
    {
        if (Damage <= 100)
        {
            hp.value = Damage % 25;

            if (Damage >= 25)
            {
                if (!stepCheck[Damage / 25])
                {
                    Debug.Log(Damage);
                    MessageManager.instance.ShowMessageText(hpMessage[Damage / 25 - 1]);
                    stepCheck[Damage / 25] = true;
                }
            }
        }
        else
        {
            hp.value = 0;
        }
    }

    public void ClearWoodHouse()
    {
        outDoor = true;
        EventManager.instance.CatchCatText();
    }

    public bool GetOutDoor()
    {
        return outDoor;
    }

    void ChangeScene()
    {
        if (sceneName == "Game")
        {
            if (transform.position.x > 147.8f && transform.position.x < 148.9f)
            {
                if (Input.GetKeyDown(KeyCode.S) || Input.GetButtonDown("Decide") && !isChangeScene)
                {
                    sceneChangeGame.LoadNextScene(2);
                    isChangeScene = true;
                    inDoorExperience = true;
                }
            }
        }

        if (sceneName == "WoodHouse")
        {
            if (Mathf.Abs(Cat.instance.transform.position.x - transform.position.x) < 1.0f)
            {
                woodSceneTu[1].gameObject.SetActive(true);
                //Debug.Log(Mathf.Abs(Cat.instance.transform.position.x - transform.position.x));
                if (Cat.instance.GetAngry() < 0.8f && (Input.GetKeyDown(KeyCode.S) || Input.GetButtonDown("Decide")))
                {
                    Cat.instance.Catch();
                    StartCoroutine(MoveSceneToGame());
                    isChangeScene = true;
                }
            }
            else
            {
                woodSceneTu[1].gameObject.SetActive(false);
            }

            if (transform.position.x > -10.25f && transform.position.x < -8.0f)
            {
                if (Input.GetKeyDown(KeyCode.S) || Input.GetButtonDown("Decide") && !isChangeScene)
                {
                    sceneChangeWood.LoadNextScene(1);
                    isChangeScene = true;
                }
            }
        }
    }

    IEnumerator MoveSceneToGame()
    {
        yield return new WaitForSeconds(9.0f);
        sceneChangeWood.LoadNextScene(1);
    }

    IEnumerator CatCoolTimeCount()
    {
        yield return new WaitForSeconds(1.0f);
        catCoolTime = false;
    }

    IEnumerator ShowDebug()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            Debug.Log(Damage);
        }
    }
}