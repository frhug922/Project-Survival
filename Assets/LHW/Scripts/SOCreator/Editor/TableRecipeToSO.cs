using UnityEditor;
using UnityEngine;
using System.IO;

/// <summary>
/// Create Recipe base on Table Data.(Will be fixed when table is created)
/// How to use : check the editor bar - Utilities - Generate Item
/// Should not be contained in the build file.
/// </summary>
public class TableRecipeToSO : MonoBehaviour
{
    private static string _itemCSVPath = "/LHW/Scripts/Crafting/Editor/CSV/TestRecipe.csv";
    [MenuItem("Utilities/Generate Recipe")]
    public static void GenerateRecipe()
    {
        string[] allLines = File.ReadAllLines(Application.dataPath + _itemCSVPath);        

        foreach (string s in allLines)
        {
            string[] splitData = s.Split(",");

            CraftingRecipe recipe = ScriptableObject.CreateInstance<CraftingRecipe>();
            float.TryParse(splitData[0], out recipe.craftingTime);           

            string lastItemDataPath = splitData[splitData.Length - 1];
            recipe.resultItem = AssetDatabase.LoadAssetAtPath<ItemSO>(lastItemDataPath);
                        
            if (recipe.resultItem == null)
                Debug.LogWarning($"SO not found at path: {lastItemDataPath}");

            AssetDatabase.CreateAsset(recipe, $"Assets/LHW/RecipeData/{recipe.resultItem.Name}.asset");
        }

        AssetDatabase.SaveAssets();
    }
}