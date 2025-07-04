using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class ItemData
{
    public string ItemID;
    public string ItemName;
    public string ItemType;
    public string ItemTooltip;
    public string ItemEnergy;
    public string ItemWeight;
    public string MaxPayloadPerPanel;
    public string IsDecomposable;
    public string ItemStats;
    public string SpritePath;
    public string PrefabPath;
}

public class ItemTable : TableBase
{
    private const string _csvUrl =
        "https://docs.google.com/spreadsheets/d/e/2PACX-1vRV9et_Ahp7R443Ghr-ZIq1Z57pcoQASDfGF3EZL1m09eMur6X1V9HkM0FcWRbqaEGRCbuQQUnB9QHM/pub?gid=1916368705&single=true&output=csv";

    public List<ItemData> TItem { get; private set; } = new List<ItemData>();

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

            TItem.Clear();
            for (int i = 1; i < lines.Length; i++)
            {
                var fields = CsvParser.ParseLine(lines[i]);
                if (fields.Count < 11)
                {
                    continue;
                }

                TItem.Add(new ItemData
                {
                    ItemID             = fields[0],
                    ItemName           = fields[1],
                    ItemType           = fields[2],
                    ItemTooltip        = fields[3],
                    ItemEnergy         = fields[4],
                    ItemWeight         = fields[5],
                    MaxPayloadPerPanel = fields[6],
                    IsDecomposable     = fields[7],
                    ItemStats          = fields[8],
                    SpritePath         = fields[9],
                    PrefabPath         = fields[10]
                });
            }
        }
    }
}
