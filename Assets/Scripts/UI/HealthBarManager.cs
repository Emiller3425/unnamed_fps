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
    void Start()
    {
        currentHealth = maxHealth;
        Transform greenTransform = transform.Find("Green");
        greenRectTransform = greenTransform.GetComponentInChildren<RectTransform>();
        greenWidthMax = greenRectTransform.rect.width;
    }

    void applyDamage(float damage)
    {
        currentHealth -= damage;
        applyToGreen();
    }
    void applyHealing(float healing)
    {
        currentHealth += healing;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        applyToGreen();
    }

    void applyToGreen()
    {
        Debug.Log(currentHealth / maxHealth * greenWidthMax);
        greenRectTransform.sizeDelta = new Vector2(currentHealth / maxHealth * greenWidthMax, greenRectTransform.rect.height);
    }
}