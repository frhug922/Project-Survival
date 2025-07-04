using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MissionAndRewardData
{
    public string DayNum;          
    public string Description;      
    public string Needs;          
    public string Reward1;       
    public string Reward1Count;     
    public string Reward2;        
    public string Reward2Count;   
}

public class MissionAndRewardTable : TableBase
{
    private const string _csvUrl =
        "https://docs.google.com/spreadsheets/d/e/2PACX-1vRV9et_Ahp7R443Ghr-ZIq1Z57pcoQASDfGF3EZL1m09eMur6X1V9HkM0FcWRbqaEGRCbuQQUnB9QHM/pub?gid=1824056272&single=true&output=csv";

    public List<MissionAndRewardData> TMissionAndReward { get; private set; }
        = new List<MissionAndRewardData>();

    public override IEnumerator Load()
    {
        using (var www = UnityWebRequest.Get(_csvUrl))
        {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
                yield break;

            var lines = www.downloadHandler.text
                .Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            TMissionAndReward.Clear();
            for (int i = 1; i < lines.Length; i++)
            {
                var fields = CsvParser.ParseLine(lines[i]);
                if (fields.Count < 7)
                    continue;

                TMissionAndReward.Add(new MissionAndRewardData
                {
                    DayNum          = fields[0],
                    Description     = fields[1],
                    Needs           = fields[2],
                    Reward1         = fields[3],
                    Reward1Count    = fields[4],
                    Reward2         = fields[5],
                    Reward2Count    = fields[6]
                });
            }
        }
    }
}
