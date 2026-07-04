using TMPro;
using UnityEngine;

public class EquipmentUIManager : MonoBehaviour
{
    public static EquipmentUIManager Instance { get; private set; }
    [HideInInspector] public TextMeshProUGUI equipmentUIText;

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
        equipmentUIText = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        GameEvents.current.OnEquipmentChanged += UpdateEquipmentUI;
    }

    private void UpdateEquipmentUI(int equipmentCount)
    {
       if (equipmentUIText != null) 
        {
            equipmentUIText.text = $"{equipmentCount}";
        } else
        {
            Debug.LogError("Equipment UI Text is null, check component hierachy");
        }
    }

    private void OnDestroy()
    {
        GameEvents.current.OnEquipmentChanged -= UpdateEquipmentUI;
    }

}