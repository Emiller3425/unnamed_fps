using UnityEngine;

public class HealthBarUIManager : MonoBehaviour
{
    public UnityEngine.UI.Image border;
    public UnityEngine.UI.Image red;
    public UnityEngine.UI.Image green;
    public static HealthBarUIManager Instance { get; private set; }
    public RectTransform greenRectTransform;
    public RectTransform chunkRectTransform;
    private float greenWidthMax;
    private float chunkStartPosition;
    private float chunkEndPosition;
    private float chunkCollapseSpeed = 2f;
    private float maxChunkPauseTime = 0.5f;
    private bool shouldChunkPause = true;
    private float currentChunkPauseTime;
    private void Awake()
    {
        Transform greenTransform = transform.Find("Green");
        Transform yellowTransform = transform.Find("Chunk");

        greenRectTransform = greenTransform.GetComponentInChildren<RectTransform>();
        chunkRectTransform = yellowTransform.GetComponentInChildren<RectTransform>();

        greenWidthMax = greenRectTransform.rect.width;
        chunkRectTransform.sizeDelta = Vector2.zero;

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

    private void Update()
    {
        if (chunkRectTransform.sizeDelta.x > 0f)
        {
            if (shouldChunkPause)
            {
                currentChunkPauseTime = maxChunkPauseTime;
                shouldChunkPause = false;
            } else if (currentChunkPauseTime > 0f)
            {
                currentChunkPauseTime -= Time.deltaTime;
            } else
            {
                chunkRectTransform.sizeDelta = new Vector2(chunkRectTransform.sizeDelta.x - chunkCollapseSpeed, chunkRectTransform.sizeDelta.y);
            }
        } else
        {
            shouldChunkPause = true;
        }
    }

    private void HealthAdded(float healing, float maxHealth, float currentHealth)
    {
        currentHealth += healing;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        ApplyToGreen(maxHealth, currentHealth);
        
        // clear chunking if we healed
        SetHealthbarChunking(0f, 0f);
    }

    private void HealthSubtracted(float damage, float maxHealth, float currentHealth)
    {
        chunkEndPosition = greenRectTransform.anchoredPosition.x + greenRectTransform.sizeDelta.x;
        ApplyToGreen(maxHealth, currentHealth);
        chunkStartPosition = greenRectTransform.anchoredPosition.x + greenRectTransform.rect.width;
        SetHealthbarChunking(chunkStartPosition, chunkEndPosition);
    }

    private void ApplyToGreen(float maxHealth, float currentHealth)
    {
        greenRectTransform.sizeDelta = new Vector2(currentHealth / maxHealth * greenWidthMax, greenRectTransform.rect.height);
    }

    private void SetHealthbarChunking(float chunkStartPosition, float chunkEndPosition)
    { 
        chunkRectTransform.anchoredPosition = new Vector3(chunkStartPosition, greenRectTransform.anchoredPosition.y);
        chunkRectTransform.sizeDelta = new Vector2(chunkEndPosition - chunkStartPosition, greenRectTransform.rect.height);
    }

    private void OnDestroy()
    {
        GameEvents.current.OnHealthAdded -= HealthAdded;
        GameEvents.current.OnHealthSubtracted -= HealthSubtracted;
    }
}