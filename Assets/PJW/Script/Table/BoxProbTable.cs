using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;
using UnityEngine.Networking;

public class BoxProbData
{
    public string   DayNum;
    public string BoxType1Prob;
    public string BoxType2Prob;
    public string BoxType3Prob;
}
public class BoxProbTable : TableBase
{
    private const string _csvUrl = 
        "https://docs.google.com/spreadsheets/d/e/2PACX-1vRV9et_Ahp7R443Ghr-ZIq1Z57pcoQASDfGF3EZL1m09eMur6X1V9HkM0FcWRbqaEGRCbuQQUnB9QHM/pub?gid=1136769341&single=true&output=csv";

    public List<BoxProbData> TBoxProb { get; private set; }
        = new List<BoxProbData>();

    public override IEnumerator Load()
    {
        using (var www = UnityWebRequest.Get(_csvUrl))
        {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                yield break;
            }

            var lines = www.downloadHandler.text.Split(new[] { "\r\n", "\n" }, System.StringSplitOptions.RemoveEmptyEntries);

            TBoxProb.Clear();
            for (int i = 1; i < lines.Length; i++)
            {
                var fields = CsvParser.ParseLine(lines[i]);
                if (fields.Count < 4)
                {
                    continue;
                }

                TBoxProb.Add(new BoxProbData
                {
                    DayNum       = fields[0],
                    BoxType1Prob = fields[1],
                    BoxType2Prob = fields[2],
                    BoxType3Prob = fields[3]
                });
            }
        }
    }
}
