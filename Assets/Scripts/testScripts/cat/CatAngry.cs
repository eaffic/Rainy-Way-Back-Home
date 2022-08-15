using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatAngry : MonoBehaviour
{
    [SerializeField]
    private Transform cat;

    private RectTransform myRectTfm;
    private Vector3 offset = new Vector3(0, 1.2f, 0);

    void Awake()
    {
        myRectTfm = GetComponent<RectTransform>();
    }

    void OnEnable()
    {
        myRectTfm.position = RectTransformUtility.WorldToScreenPoint(Camera.main, cat.position + offset);
    }

    void Update()
    {
        myRectTfm.position = RectTransformUtility.WorldToScreenPoint(Camera.main, cat.position + offset);
    }
}

