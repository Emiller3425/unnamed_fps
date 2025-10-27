using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

[RequireComponent(typeof(RectTransform))]
public class HitMarker : MonoBehaviour
{
    private RectTransform hitMarkerRectTransform;
    void Awake()
    {
        hitMarkerRectTransform = GetComponent<RectTransform>();
        hitMarkerRectTransform.anchoredPosition = Vector2.zero;
        HideHitMarker();
    }

    void Start()
    {
        hitMarkerRectTransform = GetComponent<RectTransform>();
        hitMarkerRectTransform.anchoredPosition = Vector2.zero;
    }

    public async void ShowHitMarker()
    {
        gameObject.SetActive(true);
        await Task.Delay(75);
        HideHitMarker();
    }

    public void HideHitMarker()
    {
        gameObject.SetActive(false);
    }

    public void SetPosition(Vector2 newPosition)
    {
        hitMarkerRectTransform.anchoredPosition = newPosition;
    }

}