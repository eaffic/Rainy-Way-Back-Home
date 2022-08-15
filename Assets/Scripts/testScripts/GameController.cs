using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    // rainの方で挙動がおかしくなるからescでゲームの中断ができるようにするなら処理の変更必須
    public bool isGame = false;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isGame = !isGame;
            // CheckGame();
        }
    }

    void CheckGame()
    {
        if (isGame)
        {
            Time.timeScale = 1f;
        }
        else
        {
            Time.timeScale = 0f;
        }

    }
}