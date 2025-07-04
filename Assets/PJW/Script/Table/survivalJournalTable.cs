using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SurvivalJournalData
{
    public string DayNum;       
    public string SuvProb;     
    public string SuvNonProb; 
    public string Confirm;      
}

public class SurvivalJournalTable : TableBase
{
    private const string _csvUrl =
        "https://docs.google.com/spreadsheets/d/e/2PACX-1vRV9et_Ahp7R443Ghr-ZIq1Z57pcoQASDfGF3EZL1m09eMur6X1V9HkM0FcWRbqaEGRCbuQQUnB9QHM/pub?gid=1450856882&single=true&output=csv";

    public List<SurvivalJournalData> TSurvivalJournal { get; private set; }
        = new List<SurvivalJournalData>();

    public override IEnumerator Load()
    {
        using (var www = UnityWebRequest.Get(_csvUrl))
        {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
                yield break;

            var lines = www.downloadHandler.text
                .Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            TSurvivalJournal.Clear();

            for (int i = 1; i < lines.Length; i++)
            {
                var fields = CsvParser.ParseLine(lines[i]);
                if (fields.Count < 3)
                    continue;

                TSurvivalJournal.Add(new SurvivalJournalData
                {
                    DayNum     = fields[0],
                    SuvProb    = fields[1],
                    SuvNonProb = fields[2],
                    Confirm    = fields.Count > 3 ? fields[3] : string.Empty
                });
            }
        }
    }
}
