using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class ExperienceBarUIManager : MonoBehaviour
{
    public UnityEngine.UI.Image border;
    public UnityEngine.UI.Image white;
    public UnityEngine.UI.Image blue;
    public static ExperienceBarUIManager Instance { get; private set; }
    public RectTransform blueRectTransform;
    private float blueWidthMax;
    private bool isAnimating = false;
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

    private void ExperienceAdded(float maxExperiencePoints, float currentExperiencePoints, float previousExperiencePoints, int currentLevel)
    {
        StartCoroutine(handleAnimations(maxExperiencePoints, currentExperiencePoints, previousExperiencePoints, currentLevel));
    }

    private void ApplyToBlue(float maxExperiencePoints, float currentExperiencePoints)
    {
        blueRectTransform.sizeDelta = new Vector2(currentExperiencePoints / maxExperiencePoints * blueWidthMax, blueRectTransform.rect.height);
    }

    private IEnumerator handleAnimations(float maxExperiencePoints, float currentExperiencePoints, float previousExperiencePoints, int currentLevel)
    {
        while (isAnimating)
        {
            yield return null;
        }

       StartCoroutine(AnimateExperienceBar(maxExperiencePoints, currentExperiencePoints, previousExperiencePoints, currentLevel));
    }

    private IEnumerator AnimateExperienceBar(float maxExperiencePoints, float currentExperiencePoints, float previousExperiencePoints, int currentLevel)
    {
        isAnimating = true;
        float targetExperiencePoints;
        float experiencePointsIterator = previousExperiencePoints;
        // Init case where both are 0
        if (currentExperiencePoints == 0 && previousExperiencePoints == 0 )
        {
            ApplyToBlue(maxExperiencePoints, 0);
            isAnimating = false;
            yield break;
        }

        // Player gets more than the max experiencePoints, set target to max
        if (currentExperiencePoints < previousExperiencePoints || currentExperiencePoints > maxExperiencePoints)
        { 
            targetExperiencePoints = maxExperiencePoints;
        } else
        {
            targetExperiencePoints = currentExperiencePoints;
        }
        // Animate experience bar
        while (experiencePointsIterator < targetExperiencePoints)
        {
            experiencePointsIterator += Time.deltaTime * 100f;
            ApplyToBlue(maxExperiencePoints, experiencePointsIterator);
            yield return null;
        }
        // Set back to zero for this animation if we reach the max
        if (targetExperiencePoints == maxExperiencePoints)
        {
            ApplyToBlue(maxExperiencePoints, 0);
            GameEvents.current.LevelChanged(currentLevel + 1);
        }
        isAnimating = false;
    }

    private void OnDestroy()
    {
        GameEvents.current.OnExperienceAdded -= ExperienceAdded;
    }
}