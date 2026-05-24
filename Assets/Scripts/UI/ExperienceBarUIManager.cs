using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


//TODO: Adjust Coroutine logic to handle when we receive over the max amount of xp needed
// for level up
public class ExperienceBarUIManager : MonoBehaviour
{
    public UnityEngine.UI.Image border;
    public UnityEngine.UI.Image white;
    public UnityEngine.UI.Image blue;
    public static ExperienceBarUIManager Instance { get; private set; }
    public RectTransform blueRectTransform;
    private float blueWidthMax;
    private Queue<IEnumerator> animationQueue;
    private void Awake()
    {
        Transform blueTransform = transform.Find("Blue");
        blueRectTransform = blueTransform.GetComponentInChildren<RectTransform>();
        blueWidthMax = blueRectTransform.rect.width;

        animationQueue = new Queue<IEnumerator>();

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

    private void ExperienceAdded(float maxExperiencePoints, float currentExperiencePoints, float previousExperiencePoints)
    {
        Debug.Log($"Max XP: {maxExperiencePoints}, Current XP: {currentExperiencePoints}, Previous XP: {previousExperiencePoints}");
        StartCoroutine(AnimateExperienceBar(maxExperiencePoints, currentExperiencePoints, previousExperiencePoints));
    }

    private void ApplyToBlue(float maxExperiencePoints, float currentExperiencePoints)
    {
        blueRectTransform.sizeDelta = new Vector2(currentExperiencePoints / maxExperiencePoints * blueWidthMax, blueRectTransform.rect.height);
    }

    private IEnumerator AnimateExperienceBar(float maxExperiencePoints, float currentExperiencePoints, float previousExperiencePoints)
    {
        float targetExperiencePoints;
        float experiencePointsIterator = previousExperiencePoints;
        if (currentExperiencePoints == 0 && previousExperiencePoints == 0 )
        {
            ApplyToBlue(maxExperiencePoints, 0);
            yield break;
        }
        if (currentExperiencePoints < previousExperiencePoints || currentExperiencePoints > maxExperiencePoints)
        { 
            targetExperiencePoints = maxExperiencePoints;
        } else
        {
            targetExperiencePoints = currentExperiencePoints;
        }
        Debug.Log($"Target XP: {targetExperiencePoints}");
        while (experiencePointsIterator < targetExperiencePoints)
        {
            experiencePointsIterator += Time.deltaTime * 100f;
            ApplyToBlue(maxExperiencePoints, experiencePointsIterator);
            yield return null;
        }
        if (targetExperiencePoints == maxExperiencePoints)
        {
            ApplyToBlue(maxExperiencePoints, 0);
        }
    }

    private void OnDestroy()
    {
        GameEvents.current.OnExperienceAdded -= ExperienceAdded;
    }
}