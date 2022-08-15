using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageWindow : MonoBehaviour
{
    public enum EndShow { LEFT, RIGHT, CAT }

    [SerializeField]
    private Transform player;

    private RectTransform myRectTfm;
    private Vector3 offsetR = new Vector3(2.2f, 1.6f, 0);
    private Vector3 offsetL = new Vector3(-2.3f, 2.0f, 0);
    private Vector3 offsetC = new Vector3(1.5f, -0.3f, 0);
    public EndShow end = EndShow.RIGHT;

    void Awake()
    {
        myRectTfm = GetComponent<RectTransform>();
    }

    void OnEnable()
    {
        if (end == EndShow.RIGHT)
        {
            myRectTfm.position = RectTransformUtility.WorldToScreenPoint(Camera.main, player.position + offsetR);
        }
        else if (end == EndShow.LEFT)
        {
            myRectTfm.position = RectTransformUtility.WorldToScreenPoint(Camera.main, player.position + offsetL);
        }
        else
        {
            myRectTfm.position = RectTransformUtility.WorldToScreenPoint(Camera.main, player.position + offsetC);
        }
    }

    void Update()
    {
        if (end == EndShow.RIGHT)
        {
            myRectTfm.position = RectTransformUtility.WorldToScreenPoint(Camera.main, player.position + offsetR);
        }
        else if(end == EndShow.LEFT)
        {
            myRectTfm.position = RectTransformUtility.WorldToScreenPoint(Camera.main, player.position + offsetL);
        }
        else
        {
            myRectTfm.position = RectTransformUtility.WorldToScreenPoint(Camera.main, player.position + offsetC);
        }
    }

    public void SetTextBox(int dir)
    {
        if(dir == 1) { end = EndShow.RIGHT; }
        else if(dir == 2) { end = EndShow.LEFT; }
        else if(dir == 3) { end = EndShow.CAT; }
    }
}

