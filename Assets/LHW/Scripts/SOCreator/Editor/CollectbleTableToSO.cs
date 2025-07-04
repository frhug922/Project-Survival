using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CollectbleTableToSO : MonoBehaviour
{
    private static string _collectibleCSVPath = "°æ·Î";
    [MenuItem("Utilities/Generate Collectible")]
    public static void GenerateCollectibles()
    {
        string[] allLines = File.ReadAllLines(Application.dataPath + _collectibleCSVPath);

        foreach (string s in allLines)
        {
            string[] splitData = s.Split(",");

            if (splitData.Length != 6)
            {
                Debug.LogWarning($"{s} could not be imported.");
                return;
            }

            CollectionSO collection = ScriptableObject.CreateInstance<CollectionSO>();
            int.TryParse(splitData[0], out collection.CollectionId);
            collection.CollectionName = splitData[1];
            collection.CollectionDescription = splitData[2];
            Enum.TryParse<CollectionType>(splitData[3], true, out collection.CollectionType);
            string spritePath = splitData[4];
            string silhouettePath = splitData[5];

            // If Item Path is selected, path will be edited.
            collection.CollectionIcon = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);
            collection.SilhouetteSprite = AssetDatabase.LoadAssetAtPath<Sprite>(silhouettePath);

            if (collection.CollectionIcon == null)
                Debug.LogWarning($"Sprite not found at path: {spritePath}");
            if (collection.SilhouetteSprite == null)
                Debug.LogWarning($"Prefab not found at path: {silhouettePath}");

            AssetDatabase.CreateAsset(collection, $"Assets/LHW/ItemData/{collection.CollectionName}.asset");
        }

        AssetDatabase.SaveAssets();
    }
}