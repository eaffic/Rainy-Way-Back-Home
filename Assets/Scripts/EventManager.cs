using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void EventCat()
    {
        StartCoroutine("FindCat");
    }
    IEnumerator FindCat()
    {
        yield return null;
        MessageManager.instance.ShowMessageText("あっ、ねこ！！！");
    }

    public void WorriedCatText()
    {
        StartCoroutine("WorriedCat");
    }
    IEnumerator WorriedCat()
    {
        yield return null;
        MessageManager.instance.ShowMessageText("ねこがしんぱい...");
    }

    public void CatchCatText()
    {
        StartCoroutine("CatchCat");
    }
    IEnumerator CatchCat()
    {
        yield return null;
        MessageManager.instance.ShowMessageText("おちついたかな...");
    }

    public void EscapeCatText()
    {
        StartCoroutine("EscapeCat");
    }
    IEnumerator EscapeCat()
    {
        yield return null;
        MessageManager.instance.ShowMessageText("にげちゃった...");
        yield return new WaitForSeconds(3.5f);
        MessageManager.instance.ShowMessageText("いそがなきゃ...");
    }
}


