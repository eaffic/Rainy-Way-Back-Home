using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class RestPoint : MonoBehaviour
{
    public GameObject player;
    public GameObject crow;
    public GameObject flashLight;

    public bool isIn;
    public float time;
    public float flashTemp;

    // Start is called before the first frame update
    void Start()
    {
        isIn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.instance.sceneName == "Game")
        {
            Flash();
        }
        PlayerRest();
    }

    void Flash()
    {
            flashLight.GetComponent<Light2D>().color += new Color(flashTemp, flashTemp, flashTemp);//new Color(flashLight.GetComponent<Light2D>().color.r, flashLight.GetComponent<Light2D>().color.g, flashLight.GetComponent<Light2D>().color.b);
            if (flashLight.GetComponent<Light2D>().color.r >= 0.6f) { flashTemp = -0.003f; }
            else if (flashLight.GetComponent<Light2D>().color.r <= 0.2f) { flashTemp = 0.003f; }
    }

    void PlayerRest()
    {
        if (isIn)
        {
            int temp = Player.Damage / 25;
            time += Time.deltaTime;
            if (time > 0.2f && Player.Damage > 25 * temp)
            {
                player.GetComponent<Player>().AddDamage(-1);
                time = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player") {
            isIn = true;
            if (Player.instance.sceneName == "Game")
            {
                crow.GetComponent<Crow>().isStop = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player") {
            isIn = false;
            if (Player.instance.sceneName == "Game")
            {
                crow.GetComponent<Crow>().isStop = false;
            }
        }
    }

}
