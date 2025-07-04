using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Create Item base on Table Data.
/// How to use : Play Scene with Table manager - check the editor bar - Utilities - Generate Item
/// Should not be contained in the build file.
/// </summary>
public class TableItemToSO
{
    [MenuItem("Utilities/Generate Item")]
    public static void GenerateItems()
    {
        var itemTable = TableManager.Instance.GetTable<ItemTable>(TableType.Item);
        List<ItemData> tItem = itemTable.TItem;

        foreach (ItemData s in tItem)
        {
            ItemSO item = ScriptableObject.CreateInstance<ItemSO>();
            int.TryParse(s.ItemID, out item.ItemId);
            item.Name = s.ItemName;
            Enum.TryParse<ItemType>(s.ItemType, true, out item.Type);
            item.Description = s.ItemTooltip;
            float.TryParse(s.ItemWeight, out item.Weight);
            int.TryParse(s.ItemEnergy, out item.Energy);
            int.TryParse(s.MaxPayloadPerPanel, out item.MaxStackSize);
            bool.TryParse(s.IsDecomposable, out item.IsDecomposable);
            int.TryParse(s.ItemStats, out item.ItemStats);

            // If Item Path is selected, path will be edited.
            item.Icon = AssetDatabase.LoadAssetAtPath<Sprite>($"Assets/05.Images/Items/{s.SpritePath}");
            item.Prefab = AssetDatabase.LoadAssetAtPath<GameObject> ($"Assets/03.Prefabs/Objects/{s.PrefabPath}.prefab");

            if (item.Icon == null)
                Debug.LogWarning($"Sprite not found at path: {s.SpritePath}");
            if (item.Prefab == null)
                Debug.LogWarning($"Prefab not found at path: {s.PrefabPath}");

            AssetDatabase.CreateAsset(item, $"Assets/08.ScriptableObjects/Item/{item.Name}.asset");
        }

        AssetDatabase.SaveAssets();
    }
}