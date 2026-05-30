using TMPro;
using UnityEngine;

public class LevelUIManager : MonoBehaviour
{
    public static LevelUIManager Instance { get; private set; }
    [HideInInspector] public TextMeshProUGUI levelUIText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        levelUIText = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        GameEvents.current.OnLevelChanged += UpdateLevelUI;
    }

    private void UpdateLevelUI(int currentLevel)
    {
       if (levelUIText != null) 
        {
            levelUIText.text = $"Lvl: {currentLevel}";
        } else
        {
            Debug.LogError("Level UI Text is null, check component hierachy");
        }
    }

    private void OnDestroy()
    {
        GameEvents.current.OnLevelChanged -= UpdateLevelUI;
    }

}