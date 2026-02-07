using System;
using System.Runtime.InteropServices;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;


//TODO: Add progressing xp bar
public class ExperienceBarUIManager : MonoBehaviour
{
    public UnityEngine.UI.Image border;
    public UnityEngine.UI.Image white;
    public UnityEngine.UI.Image blue;
    public static ExperienceBarUIManager Instance { get; private set; }
    public RectTransform blueRectTransform;
    private float blueWidthMax;
    private void Awake()
    {
        Transform blueTransform = transform.Find("Blue");
        blueRectTransform = blueTransform.GetComponentInChildren<RectTransform>();
        blueWidthMax = blueRectTransform.rect.width;

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        GameEvents.current.OnExperienceAdded += ExperienceAdded;
    }

    private void ExperienceAdded(float maxExperiencePoints, float currentExperiencePoints)
    {
        ApplyToBlue(maxExperiencePoints, currentExperiencePoints);
    }

    private void ApplyToBlue(float maxExperiencePoints, float currentExperiencePoints)
    {
        blueRectTransform.sizeDelta = new Vector2(currentExperiencePoints / maxExperiencePoints * blueWidthMax, blueRectTransform.rect.height);
    }

    private void OnDestroy()
    {
        GameEvents.current.OnExperienceAdded -= ExperienceAdded;
    }
}