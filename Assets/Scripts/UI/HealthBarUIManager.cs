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

public class HealthBarUIManager : MonoBehaviour
{
    public UnityEngine.UI.Image border;
    public UnityEngine.UI.Image red;
    public UnityEngine.UI.Image green;
    public static HealthBarUIManager Instance { get; private set; }
    public RectTransform greenRectTransform;
    private float greenWidthMax;
    void Awake()
    {
        Transform greenTransform = transform.Find("Green");
        greenRectTransform = greenTransform.GetComponentInChildren<RectTransform>();
        greenWidthMax = greenRectTransform.rect.width;

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        GameEvents.current.OnHealthAdded += HealthAdded;
        GameEvents.current.OnHealthSubtracted += HealthSubtracted;
    }

    private void HealthAdded(float healing, float maxHealth, float currentHealth)
    {
         Debug.Log("Update Player Health On Load");
         Debug.Log($"{healing} {maxHealth} {currentHealth}");
        currentHealth += healing;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        ApplyToGreen(maxHealth, currentHealth);
    }

    private void HealthSubtracted(float damage, float maxHealth, float currentHealth)
    {
        currentHealth -= damage;
        ApplyToGreen(maxHealth, currentHealth);
    }

    private void ApplyToGreen(float maxHealth, float currentHealth)
    {
        greenRectTransform.sizeDelta = new Vector2(currentHealth / maxHealth * greenWidthMax, greenRectTransform.rect.height);
    }

    private void OnDestroy()
    {
        GameEvents.current.OnHealthAdded -= HealthAdded;
        GameEvents.current.OnHealthSubtracted -= HealthSubtracted;
    }
}