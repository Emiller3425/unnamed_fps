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


public class HealthBarManager : MonoBehaviour
{
    public UnityEngine.UI.Image border;
    public UnityEngine.UI.Image red;
    public UnityEngine.UI.Image green;
    private float maxHealth = 100f;
    private float currentHealth;
    private RectTransform greenRectTransform;
    private float greenWidthMax;

    void Awake()
    {
        Transform greenTransform = transform.Find("Green");
        greenRectTransform = greenTransform.GetComponentInChildren<RectTransform>();
        greenWidthMax = greenRectTransform.rect.width;
    }
    
    void Start()
    {
        currentHealth = maxHealth;
    }

    void ApplyDamage(float damage)
    {
        currentHealth -= damage;
        ApplyToGreen();
    }
    void ApplyHealing(float healing)
    {
        currentHealth += healing;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        ApplyToGreen();
    }

    void ApplyToGreen()
    {
         greenRectTransform.sizeDelta = new Vector2(currentHealth / maxHealth * greenWidthMax, greenRectTransform.rect.height);
    }
}