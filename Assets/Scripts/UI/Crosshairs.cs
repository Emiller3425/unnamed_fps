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
        GameEvents.current.OnSetCrossHairActivated += SetCrossHairActivated;
        GameEvents.current.OnSetCrossHairDeactivated += SetCrossHairDeactivated;
    }

    public void SetCrossHairActivated()
    {
        gameObject.SetActive(true);
    }

    public void SetCrossHairDeactivated()
    {
        gameObject.SetActive(false);
    }

    public void SetCrossHairPosition(Vector2 newPosition)
    {
        crossHairRectTransform.anchoredPosition = newPosition;
    }

    private void OnDestoy()
    {
        GameEvents.current.OnSetCrossHairActivated -= SetCrossHairActivated;
        GameEvents.current.OnSetCrossHairDeactivated -= SetCrossHairDeactivated;
    }

}