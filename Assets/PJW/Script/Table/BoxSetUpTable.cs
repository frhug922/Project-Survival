using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BoxSetUpData
{
    public string ItemID;
    public string ProbType1;
    public string ProbType2;
    public string ProbType3;
    public string ItemCount;
}

public class BoxSetUpTable : TableBase
{
    private const string _csvUrl =
        "https://docs.google.com/spreadsheets/d/e/2PACX-1vRV9et_Ahp7R443Ghr-ZIq1Z57pcoQASDfGF3EZL1m09eMur6X1V9HkM0FcWRbqaEGRCbuQQUnB9QHM/pub?gid=554406526&single=true&output=csv";

    public List<BoxSetUpData> TBoxSetup { get; private set; }
        = new List<BoxSetUpData>();

    public override IEnumerator Load()
    {
        using (var www = UnityWebRequest.Get(_csvUrl))
        {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
                yield break;

            var lines = www.downloadHandler.text
                .Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            TBoxSetup.Clear();
            for (int i = 1; i < lines.Length; i++)
            {
                var fields = CsvParser.ParseLine(lines[i]);
                if (fields.Count < 5)
                    continue;

                TBoxSetup.Add(new BoxSetUpData
                {
                    ItemID     = fields[0],
                    ProbType1  = fields[1],
                    ProbType2  = fields[2],
                    ProbType3  = fields[3],
                    ItemCount  = fields[4]
                });
            }
        }
    }
}
