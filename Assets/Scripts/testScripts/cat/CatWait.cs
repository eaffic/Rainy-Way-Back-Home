using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CatWait : MonoBehaviour
{
    public enum STATE
    {
        IDLE,
        WALK,
    }

    [SerializeField] Player player;
    [SerializeField] float speed = 5f;
    private Animator anim;
    private string nowScene;

    void Awake()
    {
        nowScene = SceneManager.GetActiveScene().name;
    }

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (player.inDoorExperience && player.sceneName == "Game") { gameObject.SetActive(false); }

        if (nowScene == "Game")
            if (player.isEvent)
            {
                MoveToDoor();
            }

        if (transform.position.x > 148f)
        {
            Destroy(gameObject);
        }
    }

    void MoveToDoor()
    {
        anim.SetBool("isWalk", true);
        transform.Translate(speed * Time.deltaTime, 0, 0);
    }
}
