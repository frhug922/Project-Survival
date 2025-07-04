using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

    public class AssemblyContentData
{
    public string ProdID;     
    public string ItemID;     
    public string ProdType;   
    public string ProdEng; 
}
    public class AssemblyContentTable : TableBase
{
    private const string _csvUrl =
        "https://docs.google.com/spreadsheets/d/e/2PACX-1vQ0S5NJiTAdAIQgyLnWWUgkU51n7gGnJ6VpVFgySXltxBH2e2s8Icq9kM3gxA9Wsm0xeVWjOOAq2t9H/pub?gid=1733571471&single=true&output=csv";

    public List<AssemblyContentData> Contents { get; private set; }
        = new List<AssemblyContentData>();

    public override IEnumerator Load()
    {
        using (var www = UnityWebRequest.Get(_csvUrl))
        {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                yield break;
            }

            var lines = www.downloadHandler.text
                .Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            Contents.Clear();
            for (int i = 1; i < lines.Length; i++)
            {
                var fields = CsvParser.ParseLine(lines[i]);
                if (fields.Count < 4)
                {
                    continue;
                }

                Contents.Add(new AssemblyContentData
                {
                    ProdID   = fields[0],
                    ItemID   = fields[1],
                    ProdType = fields[2],
                    ProdEng  = fields[3]
                });
            }
        }
    }
}
