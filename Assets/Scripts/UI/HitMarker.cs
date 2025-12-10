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
        GameEvents.current.OnSetHitMarkerActivated += SetHitMarkerActivated;
        GameEvents.current.OnSetHitMarkerDeactivated += SetHitMarkerDeactivated;
    }

    void Start()
    {
        SetHitMarkerDeactivated();
    }

    public async void SetHitMarkerActivated()
    {
        gameObject.SetActive(true);
        await Task.Delay(75);
        SetHitMarkerDeactivated();
    }

    public void SetHitMarkerDeactivated()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        GameEvents.current.OnSetHitMarkerActivated -= SetHitMarkerActivated;
        GameEvents.current.OnSetHitMarkerDeactivated -= SetHitMarkerDeactivated;
    }

}