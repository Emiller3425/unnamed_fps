using System;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

public class ViewBobbingPivotPosition : MonoBehaviour
{
    public Vector3 defaultOffset;
    public Vector3 adsOffset;
    public bool adsEnabled;
    void Start()
    {
        // Sets view bobbing pivot for weapon on entity, defaults to non-ads pivot
        transform.localPosition = defaultOffset;
    }

    void Update()
    {
        if (adsEnabled)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, adsOffset, 0.1f);
        } else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, defaultOffset, 0.1f);
        }
    }

}