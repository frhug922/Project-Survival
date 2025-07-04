using UnityEngine;

public enum ItemType { Material, Usable, Equip }

/// <summary>
/// Scriptable Object that contains Item Data.
/// </summary>
[CreateAssetMenu(fileName = "New Item", menuName = "Assets/New Item")]
public class ItemSO : ScriptableObject
{
    public int ItemId;
    public string Name;
    public ItemType Type;
    public string Description;
    public int Energy;
    public float Weight;

    // if is stackable, input number larger than 1.
    // if is unStackable, input 1.
    public int MaxStackSize;
    public bool IsDecomposable;
    public int ItemStats;
    public Sprite Icon;
    public GameObject Prefab;
}