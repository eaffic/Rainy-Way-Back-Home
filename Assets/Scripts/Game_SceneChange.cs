using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_SceneChange : MonoBehaviour
{
    public Animator anim;
    public AudioSource au;
    public AudioSource rain;
    public AudioSource umbrella;
    public bool isChange;

    // Update is called once per frame
    void Update()
    {
        if (isChange)
        {
            rain.volume *= 0.99f;
            umbrella.volume *= 0.99f;
        }
        else
        {
            if(rain.volume <= 1.0f) { rain.volume += 0.008f; }
        }
        
    }

    public void LoadNextScene(int sceneIndex)
    {
        if(sceneIndex == 2) { au.Play();}
        StartCoroutine(LoadScene(sceneIndex));
    }

    IEnumerator LoadScene(int sceneIndex)
    {
        anim.SetTrigger("Start");
        isChange = true;
        yield return new WaitForSeconds(3.5f);
        SceneManager.LoadScene(sceneIndex);
    }
}
