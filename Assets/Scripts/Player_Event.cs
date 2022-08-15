using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_Event : MonoBehaviour
{
    public enum TextEvent { Non, Start, Car, RestArea, Wind, CrowAttack, Rain, Cat, Home, WoodHouseCat}

    public bool isEvent;
    public bool messageFlag;
    public bool eventStart;
    public bool eventCar;
    public bool eventRest;
    public bool eventWind;
    public bool eventCrowAttack;
    public bool eventRain;
    public bool eventCat;
    public bool eventHome;

    public bool eventWoodHouseCat;

    public int time;
    public int count;
    public TextEvent eventType;

    Animator anim;
    
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        eventType = TextEvent.Non;
        messageFlag = true;
        isEvent = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (eventType != TextEvent.Non) 
        {
            //StartCoroutine(TextShow());
            //isEvent = false;
            Event_Start();
        }
    }

    void Event_Start()
    {        
        string str = Event_Text(count);
        if (str == "") 
        { 
            eventType = TextEvent.Non;
            count = 0;
            messageFlag = true;
            return;
        }
        if (messageFlag) 
        { 
            MessageManager.instance.ShowMessageText(str);
            messageFlag = false;
        }
        if (MessageManager.instance.CanNextText())
        {
            messageFlag = true;
            count++;
        }
    }

    string Event_Text(int _count)
    {
        string str = "";
        if (eventType == TextEvent.Start)
        {
            string[] eventStart = { "はやくかえらなきゃ", "" };
            str = eventStart[_count];
        }
        else if (eventType == TextEvent.Car)
        {
            string[] eventCar = { "あぶないっ！", "" };
            str = eventCar[_count];
        }
        else if (eventType == TextEvent.RestArea)
        {
            string[] eventRestArea = { "すこしあまやどり...", "" };
            str = eventRestArea[_count];
        }
        else if (eventType == TextEvent.Wind)
        {
            string[] eventWind = { "わ！", "" };
            str = eventWind[_count];
        }
        else if (eventType == TextEvent.CrowAttack)
        {
            string[] eventCrowAttack = { "かさがこわれちゃう...", "" };
            str = eventCrowAttack[_count];
        }
        else if (eventType == TextEvent.Rain)
        {
            string[] eventRain = { "あめがつよくなってきた...", "" };
            str = eventRain[_count];
        }
        else if (eventType == TextEvent.Cat)
        {
            string[] eventCat = { "はいろうかな...", "" };
            str = eventCat[_count];
        }
        else if (eventType == TextEvent.Home)
        {
            string[] eventHome = { "もうすぐいえだ！", "" };
            str = eventHome[_count];
        }
        else if (eventType == TextEvent.WoodHouseCat)
        {
            string[] eventWoodHouseCat = { "ゆっくり...", "おちついて...", "" };
            str = eventWoodHouseCat[_count];
        }
        return str;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Event_Start") && !eventStart)
        {
            eventType = TextEvent.Start;
            eventStart = true;
            //StartCoroutine(PlayerStop());
        }

        if(other.gameObject.CompareTag("Event_Car") && !eventCar)
        {
            eventType = TextEvent.Car;
            eventCar = true;
        }

        if(other.gameObject.CompareTag("Event_RestArea") && !eventRest)
        {
            eventType = TextEvent.RestArea;
            eventRest = true;
        }

        if(other.gameObject.CompareTag("Event_Wind") && !eventWind)
        {
            eventType = TextEvent.Wind;
            eventWind = true;
        }

        if(other.gameObject.CompareTag("Event_CrowAttack") && !eventCrowAttack)
        {
            eventType = TextEvent.CrowAttack;
            eventCrowAttack = true;
        }

        if(other.gameObject.CompareTag("Event_Rain") && !eventRain)
        {
            eventType = TextEvent.Rain;
            eventRain = true;
        }

        if(other.gameObject.CompareTag("Event_CatWood") && !eventCat)
        {
            eventType = TextEvent.Cat;
            eventCat = true;
        }

        if(other.gameObject.CompareTag("Event_Home") && !eventHome)
        {
            eventType = TextEvent.Home;
            eventHome = true;
        }

        if(other.gameObject.CompareTag("Event_WoodHouseCat") && !eventWoodHouseCat)
        {
            eventType = TextEvent.WoodHouseCat;
            eventWoodHouseCat = true;
        }
    }

    IEnumerator PlayerStop()
    {
        GetComponent<Player>().enabled = false;
        anim.SetFloat("Speed", 0);
        yield return new WaitForSeconds(1.5f);
        GetComponent<Player>().enabled = true;
    }

    IEnumerator TextShow()
    {
        yield return new WaitForSeconds(3.0f);
    }
}
