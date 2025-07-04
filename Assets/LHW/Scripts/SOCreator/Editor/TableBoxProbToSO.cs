using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TableBoxProbToSO : MonoBehaviour
{
    [MenuItem("Utilities/Generate BoxProb")]
    public static void GenerateBoxProb()
    {
        var boxProbTable = TableManager.Instance.GetTable<BoxProbTable>(TableType.BoxProb);
        List<BoxProbData> boxProbs = boxProbTable.TBoxProb;

        BoxProbSO boxProb = ScriptableObject.CreateInstance<BoxProbSO>();

        foreach (BoxProbData s in boxProbs)
        {
            int.TryParse(s.DayNum, out int boxProb_DayNum);
            float.TryParse(s.BoxType1Prob, out float boxProb_BoxType1);
            float.TryParse(s.BoxType2Prob, out float boxProb_BoxType2);
            float.TryParse(s.BoxType3Prob, out float boxProb_BoxType3);

            boxProb._boxProbs.Add(new BoxProb { DayNum = boxProb_DayNum, BoxType1Prob = 0.01f * boxProb_BoxType1, BoxType2Prob = 0.01f * boxProb_BoxType2, BoxType3Prob = 0.01f * boxProb_BoxType3 });
        }
        AssetDatabase.CreateAsset(boxProb, $"Assets/08.ScriptableObjects/BoxProb/BoxProb.asset");

        AssetDatabase.SaveAssets();
    }
}
