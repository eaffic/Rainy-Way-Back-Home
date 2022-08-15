using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Umbrella : MonoBehaviour
{
    // 音をどうしたいか聞く

    public int hp = 30;
    int damageMax = 100;
    enum Direction { LEFT, RIGHT, Non };
    Direction dir = Direction.Non;

    public GameObject umWater;
    public GameObject player;
    public Transform[] dropPoint;
    //[SerializeField]private Image hpGauge;
    [SerializeField] private Sprite[] hpGauges;
    Image gauge;
    [Header("SE")]
    [SerializeField] private AudioClip[] waterAudio;
    private AudioSource audio;
    private SpriteRenderer sr;
    int random;
    [SerializeField] int timeCnt;

    public bool isItemDamage;

    void Start()
    {
        player = transform.parent.gameObject;
        audio = GetComponent<AudioSource>();
        sr = GetComponent<SpriteRenderer>();
        LoadHpGauge();
    }

    void Update()
    {
        ChangeDirection();

        if (hp <= 0)
        {
            //Destroy(gameObject);
        }

        //GetComponent<SpriteRenderer>().enabled = player.GetComponent<SpriteRenderer>().enabled;
        gameObject.SetActive(player.GetComponent<SpriteRenderer>().enabled);
    }

    void LoadHpGauge()
    {
        //gauge = hpGauge.gameObject.GetComponent<Image>();
    }

    public void AddDamage(int damage)
    {
        if (hp > 0)
        {
            hp -= damage;
        }
        ChangeHpGauge();
    }
    void ChangeHpGauge()
    {
        if (hp > 0)
        {
            if (hp < 10) { sr.sprite = hpGauges[2]; return; }
            if (hp < 20) { sr.sprite = hpGauges[1]; return; }
            else { sr.sprite = hpGauges[0]; }
        }
        else
        {
            if (timeCnt % 8 == 0)
            {
                //Debug.Log(dir);
                float temp = Random.Range(-50, 50) / 100.0f;
                if (dir == Direction.LEFT) { 
                    GameObject.Instantiate(umWater, new Vector3(dropPoint[0].position.x + temp, dropPoint[0].position.y, 0), Quaternion.identity);
                }
                else if (dir == Direction.RIGHT) { 
                    GameObject.Instantiate(umWater, new Vector3(dropPoint[1].position.x + temp, dropPoint[1].position.y, 0), Quaternion.identity); 
                }
            }
            if (timeCnt >= 40) { hp = 25; timeCnt = 0; }
            timeCnt++;
        }


        //if (Hp < 20) {gauge.sprite = hpGauges[0];  return; }
        //if (Hp < 40) {gauge.sprite = hpGauges[1]; return; }
        //if (Hp < 60) {gauge.sprite = hpGauges[2]; return; }
        //if (Hp < 80) {gauge.sprite = hpGauges[3]; return; }
        //if (Hp < 100) {gauge.sprite = hpGauges[4]; return; }
        //else {gauge.sprite = hpGauges[5];}
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        int damage = 1;
        if (other.gameObject.CompareTag("DropRain") || other.gameObject.CompareTag("DropWater"))
        {
            random = Random.Range(0, waterAudio.Length);
            audio.PlayOneShot(waterAudio[random]);
            //Destroy(other.gameObject);

            // test
            // damage = 10;
            AddDamage(damage);
        }

        if (other.tag == "DropItem")
        {
            player.GetComponent<Player>().AddDamage(3);
            if (!isItemDamage)
            {
                isItemDamage = true;
                MessageManager.instance.ShowMessageText("かさが...");
            }
        }


        // 岩などの傘にダメージを与えるものだった場合
        // if (other.gameObject.CompareTag(""))
        // {
        //     damage = ;
        //     Destroy(other.gameObject);
        //     AddDamage(damage);
        // }
    }

    void ChangeDirection()
    {
        if(transform.localEulerAngles.z - 180 > 0)
        {
            dir = Direction.RIGHT;
        }
        else
        {
            dir = Direction.LEFT;
        }

        //Debug.Log(transform.localEulerAngles.z);
        //// キャラ右向き
        //if (dropPoint[0].position.x - dropPoint[1].position.x < 0)
        //{
        //    if (transform.localEulerAngles.z >= 0)
        //    {
        //        dir = Direction.RIGHT;
        //    }
        //    else
        //    {
        //        dir = Direction.LEFT;
        //    }
        //}
        //else
        //{
        //    // キャラ左向き
        //    if (transform.localEulerAngles.z >= 0)
        //    {
        //        dir = Direction.RIGHT;
        //    }
        //    else
        //    {
        //        dir = Direction.LEFT;
        //    }
        //}
    }
}
