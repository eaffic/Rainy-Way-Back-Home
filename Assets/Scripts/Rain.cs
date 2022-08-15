using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rain : MonoBehaviour
{
    public enum LEVEL
    {
        EASY,
        NORMAL,
        HARD,
        VERYHARD,
        VERYVERYHARD,
        BRIDGE,
    }

    public Transform camera;
    public GameObject rain;
    public GameObject player;
    private float posY;
    private float time = 0;

    private float randX;
    private int randAngle = 0;
    public LEVEL level = LEVEL.HARD; // WoodHouseの雨の強さ
    private Quaternion rainAngle;

    public int[] rainLevel = { 1, 3, 5, 8, 10, 8 };
    private bool isPlay;


    void Start()
    {
        StartCoroutine("Change");
    }

    void Update()
    {
        time += Time.deltaTime;
        posY = camera.position.y + 8.0f;
        try
        {
            if (player.GetComponent<Player>().sceneName == "Game") { posY = camera.position.y + 5.0f; }
            if (player.GetComponent<Player>().sceneName == "WoodHouse") { posY = camera.position.y + 8.0f; }
        } catch { }

        InstantRain(randX);

        ChangeLevel();

        if(transform.position.x > 106.0f && transform.position.x < 126.0f && GetComponent<AudioSource>().volume <= 0.65f)
        {
            GetComponent<AudioSource>().volume += 0.007f;
            if (!isPlay) 
            {
                GetComponent<AudioSource>().Play();
                isPlay = true;
            }
        }
        else
        {
            GetComponent<AudioSource>().volume -= 0.005f;
        }

        transform.position = new Vector2(camera.position.x, posY);
    }

    void InstantRain(float randX)
    {
        switch (level) // levelごとに数を変える
        {
            case LEVEL.EASY:
                DropRain(rainLevel[0]);
                break;
            case LEVEL.NORMAL:
                DropRain(rainLevel[1]);
                break;
            case LEVEL.HARD:
                DropRain(rainLevel[2]);
                break;
            case LEVEL.VERYHARD:
                DropRain(rainLevel[3]);
                break;
            case LEVEL.VERYVERYHARD:
                DropRain(rainLevel[4]);
                break;
            case LEVEL.BRIDGE:
                DropRain(rainLevel[5]);
                break;
        }
    }

    void DropRain(int num)
    {
        for (int i = 0; i < num; i++)
        {
            randX = Random.Range(-19f, 35f);
            Instantiate(rain, new Vector3(transform.position.x + randX, transform.position.y + 5f), transform.rotation);
        }
    }

    void ChangeLevel()
    {
        if (player.GetComponent<Player>().sceneName == "WoodHouse")
        {
            level = LEVEL.HARD;
        }
        else
        {
            if (transform.position.x < 17f)
            {
                level = LEVEL.EASY;
            }
            else if (transform.position.x < 50f)
            {
                level = LEVEL.NORMAL;
            }
            else if (transform.position.x < 90f)
            {
                level = LEVEL.HARD;
            }
            else if (transform.position.x < 126f)
            {
                level = LEVEL.BRIDGE;
            }
            else if (transform.position.x < 131f)
            {
                level = LEVEL.HARD;
            }
            else if (transform.position.x < 140f)
            {
                level = LEVEL.VERYHARD;
            }
            else if (!Player.instance.GetOutDoor())
            {
                level = LEVEL.VERYVERYHARD;
            }
            else if (Player.instance.GetOutDoor())
            {
                level = LEVEL.HARD;
            }
        }
    }

    IEnumerator Change()
    {
        while (true)
        {
            switch (level)
            {
                case LEVEL.EASY:
                    randAngle = Random.Range(-10, 10);
                    break;
                case LEVEL.NORMAL:
                    randAngle = Random.Range(-20, 20);
                    break;
                case LEVEL.HARD:
                    randAngle = Random.Range(-40, 40);
                    break;
                case LEVEL.VERYHARD:
                    randAngle = Random.Range(-50, 50);
                    break;
                case LEVEL.BRIDGE:
                    randAngle = -55;
                    break;
            }
            int z = (int)transform.localEulerAngles.z;
            if(z > 180)
            {
                z -= 360;
            }
            //Debug.Log(z.ToString() + " " + randAngle.ToString());
            if (z != 0)
            {
                for (int i = z; i >= 0; i--)
                {
                    yield return new WaitForSeconds(0.2f);
                    transform.rotation = Quaternion.Euler(0, 0, i);
                }
                for (int i = z; i <= 0; i++)
                {
                    yield return new WaitForSeconds(0.2f);
                    transform.rotation = Quaternion.Euler(0, 0, i);
                }
            }

            //for (int i = 0; i < randAngle; i++)
            //{
            //    yield return new WaitForSeconds(0.3f);
            //    transform.rotation = Quaternion.Euler(0, 0, i);
            //}

            if (randAngle < 0)
            {
                for (int i = 0; i > randAngle; i--)
                {
                    yield return new WaitForSeconds(0.15f);
                    transform.rotation = Quaternion.Euler(0, 0, i);
                }
            }
            else
            {
                for (int i = 0; i < randAngle; i++)
                {
                    yield return new WaitForSeconds(0.15f);
                    transform.rotation = Quaternion.Euler(0, 0, i);
                }
            }
        }
    }
}


