using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class FoodProbData
{
    public string DayNum;       
    public string MealProb;     
    public string MealNonProb;  
    public string ItemID;       
}

public class FoodProbTable : TableBase
{
    private const string _csvUrl =
        "https://docs.google.com/spreadsheets/d/e/2PACX-1vRV9et_Ahp7R443Ghr-ZIq1Z57pcoQASDfGF3EZL1m09eMur6X1V9HkM0FcWRbqaEGRCbuQQUnB9QHM/pub?gid=1474219337&single=true&output=csv";

    public List<FoodProbData> TFoodProb { get; private set; }
        = new List<FoodProbData>();

    public override IEnumerator Load()
    {
        using (var www = UnityWebRequest.Get(_csvUrl))
        {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
                yield break;

            var lines = www.downloadHandler.text
                .Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            TFoodProb.Clear();
            for (int i = 1; i < lines.Length; i++)
            {
                var fields = CsvParser.ParseLine(lines[i]);
                if (fields.Count < 4)
                    continue;

                TFoodProb.Add(new FoodProbData
                {
                    DayNum      = fields[0],
                    MealProb    = fields[1],
                    MealNonProb = fields[2],
                    ItemID      = fields[3]
                });
            }
        }
    }
}
