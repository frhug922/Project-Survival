using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class StoryLineContentData
{
    public string Chapter;          
    public string RelatedItemID;    
    public int    ID;              
    public int    DialogType;       
    public string Name;            
    public string Dialog;
}

public class StoryLineContentTable : TableBase
{
    private const string _csvUrl =
        "https://docs.google.com/spreadsheets/d/e/2PACX-1vRV9et_Ahp7R443Ghr-ZIq1Z57pcoQASDfGF3EZL1m09eMur6X1V9HkM0FcWRbqaEGRCbuQQUnB9QHM/pub?gid=1846831833&single=true&output=csv";

    public List<StoryLineContentData> TStoryLineContent { get; private set; }
        = new List<StoryLineContentData>();

    public override IEnumerator Load()
    {
        using (var www = UnityWebRequest.Get(_csvUrl))
        {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
                yield break;

            var lines = www.downloadHandler.text
                .Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            TStoryLineContent.Clear();
            for (int i = 1; i < lines.Length; i++)
            {
                var fields = CsvParser.ParseLine(lines[i]);
                if (fields.Count < 6)
                    continue;

                TStoryLineContent.Add(new StoryLineContentData
                {
                    Chapter       = fields[0],
                    RelatedItemID = fields[1],
                    ID            = int.Parse(fields[2]),
                    DialogType    = int.Parse(fields[3]),
                    Name          = fields[4],
                    Dialog        = fields[5]
                });
            }
        }
    }
}
