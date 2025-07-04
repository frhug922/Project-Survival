// Assets/Scripts/ItemExample.cs
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class ItemExample : MonoBehaviour
{
    private AssemblyContentTable    _assemblyContentTable;
    private BoxProbTable            _boxProbTable;
    private BoxSetUpTable           _boxSetupTable;
    private CollectibleContentTable _collectibleTable;
    private FixContentTable         _fixContentTable;
    private FoodProbTable           _foodProbTable;
    private ItemTable               _itemTable;
    private MissionAndRewardTable   _missionTable;
    private MonsterTable            _monsterTable;
    private StoryLineContentTable   _storyLineTable;
    private SurvivalJournalTable    _journalTable;

    private IEnumerator Start()
    {
        yield return new WaitUntil(() =>
            TableManager.Instance.GetTable<AssemblyContentTable>(TableType.AssemblyContent)?.Contents            != null &&
            TableManager.Instance.GetTable<BoxProbTable>(TableType.BoxProb)?.TBoxProb                         != null &&
            TableManager.Instance.GetTable<BoxSetUpTable>(TableType.BoxSetup)?.TBoxSetup                       != null &&
            TableManager.Instance.GetTable<CollectibleContentTable>(TableType.CollectibleContent)?.TCollectibleContents != null &&
            TableManager.Instance.GetTable<FixContentTable>(TableType.FixContent)?.FixContents                 != null &&
            TableManager.Instance.GetTable<FoodProbTable>(TableType.FoodProb)?.TFoodProb                       != null &&
            TableManager.Instance.GetTable<ItemTable>(TableType.Item)?.TItem                                  != null &&
            TableManager.Instance.GetTable<MissionAndRewardTable>(TableType.MissionAndReward)?.TMissionAndReward != null &&
            TableManager.Instance.GetTable<MonsterTable>(TableType.Monster)?.TMonster                         != null &&
            TableManager.Instance.GetTable<StoryLineContentTable>(TableType.StoryLineContent)?.TStoryLineContent != null &&
            TableManager.Instance.GetTable<SurvivalJournalTable>(TableType.SurvivalJournal)?.TSurvivalJournal  != null
        );

        _assemblyContentTable  = TableManager.Instance.GetTable<AssemblyContentTable>(TableType.AssemblyContent);
        _boxProbTable          = TableManager.Instance.GetTable<BoxProbTable>(TableType.BoxProb);
        _boxSetupTable         = TableManager.Instance.GetTable<BoxSetUpTable>(TableType.BoxSetup);
        _collectibleTable      = TableManager.Instance.GetTable<CollectibleContentTable>(TableType.CollectibleContent);
        _fixContentTable       = TableManager.Instance.GetTable<FixContentTable>(TableType.FixContent);
        _foodProbTable         = TableManager.Instance.GetTable<FoodProbTable>(TableType.FoodProb);
        _itemTable             = TableManager.Instance.GetTable<ItemTable>(TableType.Item);
        _missionTable          = TableManager.Instance.GetTable<MissionAndRewardTable>(TableType.MissionAndReward);
        _monsterTable          = TableManager.Instance.GetTable<MonsterTable>(TableType.Monster);
        _storyLineTable        = TableManager.Instance.GetTable<StoryLineContentTable>(TableType.StoryLineContent);
        _journalTable          = TableManager.Instance.GetTable<SurvivalJournalTable>(TableType.SurvivalJournal);

        // 디버그 출력
        Debug.Log("=== AssemblyContentTable.Contents에서 ProdID 뽑기 ===");
        foreach (var ac in _assemblyContentTable.Contents)
            Debug.Log($"AssemblyContent → ProdID: {ac.ProdID}");

        Debug.Log("=== BoxProbTable.TBoxProb에서 DayNum 뽑기 ===");
        foreach (var bp in _boxProbTable.TBoxProb)
            Debug.Log($"BoxProb → DayNum: {bp.DayNum}");

        Debug.Log("=== BoxSetUpTable.TBoxSetup에서 ItemID 뽑기 ===");
        foreach (var bs in _boxSetupTable.TBoxSetup)
            Debug.Log($"BoxSetUp → ItemID: {bs.ItemID}");

        Debug.Log("=== CollectibleContentTable.TCollectibleContents에서 ColtID 뽑기 ===");
        foreach (var cc in _collectibleTable.TCollectibleContents)
            Debug.Log($"CollectibleContent → ColtID: {cc.ColtID}");

        Debug.Log("=== FixContentTable.FixContents에서 ProdID 뽑기 ===");
        foreach (var fc in _fixContentTable.FixContents)
            Debug.Log($"FixContent → ProdID: {fc.ProdID}");

        Debug.Log("=== FoodProbTable.TFoodProb에서 DayNum 뽑기 ===");
        foreach (var fp in _foodProbTable.TFoodProb)
            Debug.Log($"FoodProb → DayNum: {fp.DayNum}");

        Debug.Log("=== ItemTable.TItem에서 ItemID 뽑기 ===");
        foreach (var item in _itemTable.TItem)
            Debug.Log($"ItemTable → ItemID: {item.ItemID}");

        Debug.Log("=== MissionAndRewardTable.TMissionAndReward에서 DayNum 뽑기 ===");
        foreach (var mr in _missionTable.TMissionAndReward)
            Debug.Log($"MissionAndReward → DayNum: {mr.DayNum}");

        Debug.Log("=== MonsterTable.TMonster에서 MonID 뽑기 ===");
        foreach (var m in _monsterTable.TMonster)
            Debug.Log($"Monster → MonID: {m.MonID}");

        Debug.Log("=== StoryLineContentTable.TStoryLineContent에서 Chapter 뽑기 ===");
        foreach (var sl in _storyLineTable.TStoryLineContent)
            Debug.Log($"StoryLineContent → Chapter: {sl.Chapter}");

        Debug.Log("=== SurvivalJournalTable.TSurvivalJournal에서 DayNum 뽑기 ===");
        foreach (var sj in _journalTable.TSurvivalJournal)
            Debug.Log($"SurvivalJournal → DayNum: {sj.DayNum}");
    }
}
