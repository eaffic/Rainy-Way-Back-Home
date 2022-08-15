using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageManager : MonoBehaviour
{
    public GameObject messageWindow;
    public AudioSource au;
    public Text messageText;
    public bool nextText = false;
    public bool newText;
    public int count;

    public static MessageManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Update()
    {
        if (!newText) { HideMessageWindow(); }
    }

    public void ShowMessageWindow()
    {
        messageText.text = "";
        messageWindow.SetActive(true);
    }

    public void HideMessageWindow()
    {
        nextText = true;
        messageWindow.SetActive(false);
    }

    public void ShowMessageText(string msg)
    {
        nextText = false;
        StartCoroutine(MessageCount());
        StartCoroutine(DestroyMessageWindow(msg));
    }

    IEnumerator NovelText(string msg)
    {
        string str = "";
        for (int i = 0; i < msg.Length; i++)
        {
            str += msg.Substring(i, 1);
            au.Play();
            messageText.text = str;
            yield return new WaitForSeconds(0.06f);
        }
    }

    IEnumerator DestroyMessageWindow(string msg)
    {

        ShowMessageWindow();

        StartCoroutine(NovelText(msg));
        //yield return new WaitForSeconds(3.0f);
        yield return null;
    }

    IEnumerator MessageCount()
    {
        count++;
        newText = true;
        yield return new WaitForSeconds(2.5f);
        count--;
        if (count == 0) { newText = false; }
    }


    public bool CanNextText()
    {
        return nextText;
    }
}
