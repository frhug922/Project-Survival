using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CollectibleContentData
{
    public string ColtID;            
    public string ColtName;         
    public string ColtProb;         
    public string ColtDcom;         
    public string ColtDescription;  
    public string ColtSprite;      
}

public class CollectibleContentTable : TableBase
{
    private const string _csvUrl =
        "https://docs.google.com/spreadsheets/d/e/2PACX-1vRV9et_Ahp7R443Ghr-ZIq1Z57pcoQASDfGF3EZL1m09eMur6X1V9HkM0FcWRbqaEGRCbuQQUnB9QHM/pub?gid=444784163&single=true&output=csv";

    public List<CollectibleContentData> TCollectibleContents { get; private set; }
        = new List<CollectibleContentData>();

    public override IEnumerator Load()
    {
        using (var www = UnityWebRequest.Get(_csvUrl))
        {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
                yield break;

            var lines = www.downloadHandler.text
                .Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            TCollectibleContents.Clear();
            for (int i = 1; i < lines.Length; i++)
            {
                var fields = CsvParser.ParseLine(lines[i]);
                if (fields.Count < 6)
                    continue;

                TCollectibleContents.Add(new CollectibleContentData
                {
                    ColtID          = fields[0],
                    ColtName        = fields[1],
                    ColtProb        = fields[2],
                    ColtDcom        = fields[3],
                    ColtDescription = fields[4],
                    ColtSprite      = fields[5]
                });
            }
        }
    }
}
