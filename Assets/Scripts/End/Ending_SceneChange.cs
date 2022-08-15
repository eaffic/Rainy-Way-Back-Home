using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ending_SceneChange : MonoBehaviour
{
    public Animator anim;
    public AudioSource au;

    public bool isChange;

    void Update()
    {
        if (isChange) { au.volume *= 0.99f; }
    }

    public void LoadNextScene(int sceneIndex)
    {
        StartCoroutine(LoadScene(sceneIndex));
    }

    IEnumerator LoadScene(int sceneIndex)
    {
        anim.SetTrigger("Start");
        isChange = true;
        yield return new WaitForSeconds(2.8f);
        SceneManager.LoadScene(sceneIndex);
    }
}
