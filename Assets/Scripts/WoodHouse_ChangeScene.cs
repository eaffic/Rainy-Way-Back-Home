using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WoodHouse_ChangeScene : MonoBehaviour
{
    public Animator anim;
    public AudioSource au;
    public AudioSource rain;
    public AudioSource umbrella;

    public bool isChange;

    private void Update()
    {
        if (isChange)
        {
            rain.volume *= 0.99f;
            umbrella.volume *= 0.99f;
        }
        else
        {
            if(rain.volume <= 1.0f) { rain.volume += 0.005f; }
        }
    }

    public void LoadNextScene(int sceneIndex)
    {
        StartCoroutine(LoadScene(sceneIndex));
    }

    IEnumerator LoadScene(int sceneIndex)
    {
        anim.SetTrigger("Start");
        isChange = true;
        yield return new WaitForSeconds(1.5f);
        au.Play();
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene(sceneIndex);
    }
}
