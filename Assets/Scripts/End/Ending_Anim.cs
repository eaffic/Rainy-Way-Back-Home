using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ending_Anim : MonoBehaviour
{
    public GameObject text;
    public GameObject cat;
    public GameObject crow;
    public GameObject umbrella;
    public Ending_SceneChange sceneChangeEnd;
    public Text ending;
    public int hp;
    public bool catchCat;
    public float time;

    // Start is called before the first frame update
    void Start()
    {
        hp = GlobalControl.Instance.HP;
        catchCat = GlobalControl.Instance.CatchCat;
        ChangeEndingText();
    }

    // Update is called once per frame
    void Update()
    {
        if (time > 3.0f)
        {
            text.GetComponent<Text>().color += new Color(0, 0, 0, 0.01f);
            if (text.GetComponent<Text>().color.a > 0.5f)
            {
                ending.color += new Color(0, 0, 0, 0.01f);
            }
        }

        if (Input.anyKeyDown && time > 6.0f)
        {
            sceneChangeEnd.LoadNextScene(0);
        }

        if(time > 25.0f)
        {
            sceneChangeEnd.LoadNextScene(0);
        }
        time += Time.deltaTime;
    }

    void ChangeEndingText()
    {
        cat.gameObject.SetActive(false);
        if (catchCat)
        {
            if (hp < 50) { ending.text = "End: Cat"; cat.gameObject.SetActive(true); }
            else if (hp < 100) { ending.text = "End: Normal"; }
            else { ending.text = "End: Bad"; umbrella.SetActive(false); }
        }
        else
        {
            ending.text = "End: Non";
            crow.gameObject.SetActive(false);
        }
    }
}
