using System;
using System.Runtime.InteropServices;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

// TODO: make cross hair widen when walking, and after shot. instaed of hidding middle, should be within crosshair bounds

[RequireComponent(typeof(RectTransform))]
public class Crosshairs : MonoBehaviour
{
    private RectTransform crossHairRectTransform;
    void Awake()
    {
        crossHairRectTransform = GetComponent<RectTransform>();
        crossHairRectTransform.anchoredPosition = Vector2.zero;
    }

    void Start()
    {
        crossHairRectTransform = GetComponent<RectTransform>();
        crossHairRectTransform.anchoredPosition = Vector2.zero;
    }

    public void ShowCrosshairs()
    {
        gameObject.SetActive(true);
    }

    public void HideCrosshairs()
    {
        gameObject.SetActive(false);
    }

    public void SetPosition(Vector2 newPosition)
    {
        crossHairRectTransform.anchoredPosition = newPosition;
    }

}