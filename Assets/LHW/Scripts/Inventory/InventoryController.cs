using TMPro;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] InventorySlotUnit[] slots;
    [SerializeField] TMP_Text _weightText;
    [SerializeField] InventoryManager _inventoryManager;

    private void Start()
    {
        InventoryManager.OnInventorySlotChanged += UpdateUISlot;
        InventoryManager.OnInventorySlotChanged += UpdateWeightText;
        UpdateUISlot();
        UpdateWeightText();
    }

    private void OnEnable()
    {        
        InventoryManager.OnInventorySlotChanged += UpdateUISlot;
        InventoryManager.OnInventorySlotChanged += UpdateWeightText;
        UpdateUISlot();
        UpdateWeightText();
    }

    private void OnDisable()
    {
        InventoryManager.OnInventorySlotChanged -= UpdateUISlot;
        InventoryManager.OnInventorySlotChanged -= UpdateWeightText;
    }

    private void UpdateUISlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].UpdateUI(i);
        }
    }

    private void UpdateWeightText()
    {        
        PlayerStats stats = GameManager.Instance.PlayerStats;

        float weight = stats.CurrentInventoryWeight;
        float maxWeight = stats.MaxInventoryWeight;
        
        _weightText.text = $"kg {weight.ToString()} / {maxWeight.ToString()}";
    }
}