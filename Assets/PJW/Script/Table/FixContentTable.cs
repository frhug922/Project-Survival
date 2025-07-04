using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FixContentData
{
    public string ProdID;
    public string SpritePath;
    public string ProdType;
    public string ProdName;
    public string ProdDescription;
    public string ProdEng;
}

public class FixContentTable : TableBase
{
    private const string _csvUrl =
        "https://docs.google.com/spreadsheets/d/e/2PACX-1vRV9et_Ahp7R443Ghr-ZIq1Z57pcoQASDfGF3EZL1m09eMur6X1V9HkM0FcWRbqaEGRCbuQQUnB9QHM/pub?gid=1225095714&single=true&output=csv";

    public List<FixContentData> FixContents { get; private set; }
        = new List<FixContentData>();

    public override IEnumerator Load()
    {
        using (var www = UnityWebRequest.Get(_csvUrl))
        {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
                yield break;

            var lines = www.downloadHandler.text
                .Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            FixContents.Clear();

            for (int i = 1; i < lines.Length; i++)
            {
                var fields = CsvParser.ParseLine(lines[i]);
                if (fields.Count < 6)
                    continue;

                FixContents.Add(new FixContentData
                {
                    ProdID         = fields[0],
                    SpritePath     = fields[1],
                    ProdType       = fields[2],
                    ProdName       = fields[3],
                    ProdDescription= fields[4],
                    ProdEng        = fields[5]
                });
            }
        }
    }
}
