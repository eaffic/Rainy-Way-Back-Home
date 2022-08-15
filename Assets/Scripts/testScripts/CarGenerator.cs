using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarGenerator : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] GameObject[] car;
    [SerializeField] GameObject[] carPos;
    [SerializeField] AudioClip SE;
    Vector3[] pos = new Vector3[3];
    int[] once = { 0, 0, 0 };
    AudioSource audio;

    void Start()
    {
        for (int i = 0; i < pos.Length; i++)
        {
            pos[i] = carPos[i].transform.position;
        }

        audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Check(14.5f, 15.0f, 0))
        {
            Car0();
        }
        if (Check(32.5f, 33.0f, 1))
        {
            Car1();
        }
        if (Check(159f, 160.5f, 2))
        {
            Car2();
        }
    }

    void InstantCar(int i)
    {
        Instantiate(car[i], pos[i], Quaternion.identity);
        audio.PlayOneShot(SE);
    }

    void Car0()
    {
        InstantCar(0); // 0.8
        StartCoroutine(Show());
    }
    void Car1()
    {
        InstantCar(1);
    }
    void Car2()
    {
        InstantCar(2);
    }

    bool Check(float s, float e, int i)
    {
        if (player.position.x >= s && player.position.x < e && once[i] == 0)
        {
            once[i]++;
            return true;
        }
        return false;
    }

    IEnumerator Show()
    {
        yield return new WaitForSeconds(0.3f);
        MessageManager.instance.ShowMessageText("あぶないっ！");
    }
}
