using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange_Title : MonoBehaviour
{
    public Animator anim;
    public AudioSource au;
    public AudioSource bgm;
    public AudioSource rain;
    public GameObject startParticle;
    public float time;
    public bool isStart;

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > 5.0f)
        {
            if (Input.anyKeyDown && !isStart)
            {
                isStart = true;
                Instantiate(startParticle, new Vector3(0, 1.5f, 0), Quaternion.identity);
                LoadNextScene(1);
            }
        }
        else
        {
            if(rain.volume <= 0.95f) { rain.volume += 0.004f; }
            if(bgm.volume <= 1.0f) { bgm.volume += 0.006f; }
        }

        if (isStart)
        {
            bgm.volume *= 0.993f;
            rain.volume *= 0.993f;
            GetComponent<AudioSource>().volume *= 0.99f;
        }
    }

    public void LoadNextScene(int sceneIndex)
    {
        GlobalControl.Instance.HP = 0;
        GlobalControl.Instance.CatchCat = false;
        Player.stepCheck = new bool[] { false, false, false, false, false };
        au.Play();
        StartCoroutine(LoadScene(sceneIndex));
        //au.volume -= 0.01f;
    }

    IEnumerator LoadScene(int sceneIndex)
    {
        anim.SetTrigger("Start");
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene(sceneIndex);
    }
}
