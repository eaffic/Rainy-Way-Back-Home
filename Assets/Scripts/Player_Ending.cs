using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_Ending : MonoBehaviour
{
    enum End { Cat, Normal, Bad }

    public GameObject sceneChange;
    public GameObject cat;
    public MessageWindow textBox;
    public AudioClip catAudio;
    public bool catchCat;
    public bool inDoor;
    public bool isCatRun;
    public bool isEnd;
    public bool isFollow;
    public int hp;
    End end;

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        hp = GlobalControl.Instance.HP;
        if (hp < 50)
        {
            end = End.Cat;
        }
        else if (hp < 100)
        {
            end = End.Normal;
        }
        else
        {
            end = End.Bad;
        }

        isFollow = false;
        isCatRun = false;
        inDoor = false;
        catchCat = GetComponent<Player>().GetOutDoor();
        Player.outDoor = false;
        Player.notCatchCat = false;
        GetComponent<Animator>().SetFloat("Speed", 0.2f);
        GetComponent<Player>().SetUmbrellaAngle(0);
        GetComponent<Player>().enabled = false;

        cat.SetActive(true);
        cat.GetComponent<Animator>().SetBool("isWalk", true);
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Player>().enabled = false;
        if (cat.transform.position.x < 211 && !isCatRun && catchCat && isFollow)
        {
            cat.transform.position += new Vector3(4.4f * Time.deltaTime, 0, 0);
        }
        else if(isFollow)
        {
            //cat.GetComponent<Animator>().SetBool("isWalk", false);
            cat.transform.localScale = new Vector3(0.8f, 0.8f, 0);
            cat.GetComponent<SpriteRenderer>().enabled = false;
        }

        if (transform.position.x < 211)
        {
            GetComponent<Animator>().SetFloat("Speed", 1.0f);
            transform.position += new Vector3(1.9f * Time.deltaTime, 0, 0);
        }
        else if (!inDoor)
        {
            inDoor = true;
            if (catchCat)
            {
                if (end == End.Cat) { StartCoroutine(Ending_Cat()); }
                else if (end == End.Normal) { StartCoroutine(Ending_Normal()); }
                else if (end == End.Bad) { StartCoroutine(Ending_Bad()); }
                GlobalControl.Instance.CatchCat = true;
            }
            else
            {
                StartCoroutine(Ending_Non());
            }
            sceneChange.GetComponent<AudioSource>().Play();
            GetComponent<AudioSource>().Stop();
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<Rigidbody2D>().gravityScale = 0;
            GetComponent<CircleCollider2D>().enabled = false;
        }

        if (isCatRun)
        {
            //StartCoroutine(CatRun());
        }

        if (isEnd)
        {
            isEnd = false;
            sceneChange.GetComponent<Game_SceneChange>().LoadNextScene(3);
        }
    }

    IEnumerator CatRun()
    {
        cat.transform.position = Vector3.MoveTowards(cat.transform.position, new Vector3(225.0f, cat.transform.position.y, 0), 0.3f);
        yield return 0;
    }

    IEnumerator Ending_Non()
    {
        yield return new WaitForSeconds(2.5f);
        MessageManager.instance.ShowMessageText("ただいまー");
        yield return new WaitForSeconds(2.8f);

        textBox.SetTextBox(2);
        MessageManager.instance.ShowMessageText("おかえり");
        isEnd = true;
        yield return 0;
    }

    IEnumerator Ending_Normal()
    {
        yield return new WaitForSeconds(2.5f);
        MessageManager.instance.ShowMessageText("ただいまー");
        yield return new WaitForSeconds(2.8f);

        textBox.SetTextBox(2);
        MessageManager.instance.ShowMessageText("おかえり");
        yield return new WaitForSeconds(2.8f);

        textBox.SetTextBox(1);
        MessageManager.instance.ShowMessageText("ママー、あのね...");
        //yield return new WaitForSeconds(2.8f);

        //MessageManager.instance.ShowMessageText("おやつを用意したよう～");
        //yield return new WaitForSeconds(3.0f);

        //textBox.SetTextBox(1);
        //MessageManager.instance.ShowMessageText("わぃー");
        //yield return new WaitForSeconds(1.0f);

        //isCatRun = true;
        //cat.GetComponent<SpriteRenderer>().enabled = true;
        //cat.transform.localScale = new Vector3(1.0f, 1.0f, 0);
        //yield return new WaitForSeconds(2.0f);

        //MessageManager.instance.ShowMessageText("?");
        //yield return new WaitForSeconds(0.5f);

        //MessageManager.instance.ShowMessageText("何の声.......？");
        isEnd = true;
        yield return 0;
       
    }

    IEnumerator Ending_Cat()
    {
        isFollow = true;
        yield return new WaitForSeconds(2.5f);
        MessageManager.instance.ShowMessageText("ただいま");
        yield return new WaitForSeconds(2.8f);

        textBox.SetTextBox(2);
        MessageManager.instance.ShowMessageText("おかえり");
        yield return new WaitForSeconds(2.8f);

        //textBox.SetTextBox(3);
        //MessageManager.instance.ShowMessageText("ニャー！");
        GetComponent<AudioSource>().PlayOneShot(catAudio);
        yield return new WaitForSeconds(2.8f);

        textBox.SetTextBox(1);
        MessageManager.instance.ShowMessageText("わ！ついてきちゃった！");

        //MessageManager.instance.ShowMessageText("すごい汚れてるじゃない");
        //yield return new WaitForSeconds(3.0f);

        //MessageManager.instance.ShowMessageText("またどこで遊んできたの？");
        //yield return new WaitForSeconds(3.0f);

        //textBox.SetTextBox(1);
        //MessageManager.instance.ShowMessageText("う、うん...");
        //yield return new WaitForSeconds(2.0f);

        //textBox.SetTextBox(3);
        //MessageManager.instance.ShowMessageText("ニャー");
        //yield return new WaitForSeconds(1.0f);

        //textBox.SetTextBox(1);
        //MessageManager.instance.ShowMessageText("わっ！ついてきた！");
        //yield return new WaitForSeconds(3.0f);

        //textBox.SetTextBox(2);
        //MessageManager.instance.ShowMessageText("猫...？");
        //yield return new WaitForSeconds(3.0f);

        //MessageManager.instance.ShowMessageText("はぁ...しょうがないね...");
        //yield return new WaitForSeconds(3.0f);

        //MessageManager.instance.ShowMessageText("早くお風呂に入りなさい、ねこもね");
        //yield return new WaitForSeconds(3.0f);

        //textBox.SetTextBox(1);
        //MessageManager.instance.ShowMessageText("え...飼えるの？");
        //yield return new WaitForSeconds(3.0f);

        //textBox.SetTextBox(2);
        //MessageManager.instance.ShowMessageText("前からずっと欲しいて言ってたでしょう、");
        //yield return new WaitForSeconds(3.0f);

        //MessageManager.instance.ShowMessageText("責任をもって最後まで面倒しなさい！");
        //yield return new WaitForSeconds(3.0f);

        //textBox.SetTextBox(1);
        //MessageManager.instance.ShowMessageText("はーい！ママ、ありがどう！");
        //yield return new WaitForSeconds(2.0f);

        //textBox.SetTextBox(3);
        //MessageManager.instance.ShowMessageText("ニャー");
        //yield return new WaitForSeconds(3.0f);

        //textBox.SetTextBox(2);
        //MessageManager.instance.ShowMessageText("天気が良くなったらまず病院に行こう...");
        isEnd = true;
        yield return 0;
    }

    IEnumerator Ending_Bad()
    {
        yield return new WaitForSeconds(2.5f);
        MessageManager.instance.ShowMessageText("ただいま.....");
        yield return new WaitForSeconds(3.5f);

        textBox.SetTextBox(2);
        //MessageManager.instance.ShowMessageText("おかえ...");
        //yield return new WaitForSeconds(0.8f);

        MessageManager.instance.ShowMessageText("なんで汚れてるの！！");
        isCatRun = true;
        cat.GetComponent<SpriteRenderer>().enabled = true;
        cat.transform.localScale = new Vector3(1.0f, 1.0f, 0);
        yield return new WaitForSeconds(2.0f);

        MessageManager.instance.ShowMessageText("おふろいってきなさい！！");
        yield return new WaitForSeconds(2.8f);

        textBox.SetTextBox(1);
        MessageManager.instance.ShowMessageText("うん...");
        //yield return new WaitForSeconds(3.0f);

        //MessageManager.instance.ShowMessageText("ママに怒られた......");
        isEnd = true;
        yield return 0;
    }
}
