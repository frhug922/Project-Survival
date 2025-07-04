using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System;

public class TableManager : MonoBehaviour
{
    #region Singleton
    public static TableManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        StartCoroutine(LoadAllTablesAndWrap());
    }
    #endregion

    private Dictionary<TableType, TableBase> _tables = new Dictionary<TableType, TableBase>();

    public List<System.Collections.IList> TItems { get; private set; }

    private IEnumerator LoadAllTablesAndWrap()
    {
        RegisterAndLoadTable(TableType.Item,            new ItemTable());
        RegisterAndLoadTable(TableType.BoxProb,         new BoxProbTable());
        RegisterAndLoadTable(TableType.AssemblyContent, new AssemblyContentTable());
        RegisterAndLoadTable(TableType.Monster,         new MonsterTable());
        RegisterAndLoadTable(TableType.BoxSetup,        new BoxSetUpTable());
        RegisterAndLoadTable(TableType.SurvivalJournal, new SurvivalJournalTable());
        RegisterAndLoadTable(TableType.CollectibleContent, new CollectibleContentTable());
        RegisterAndLoadTable(TableType.FoodProb,        new FoodProbTable());
        RegisterAndLoadTable(TableType.FixContent,      new FixContentTable());
        RegisterAndLoadTable(TableType.MissionAndReward, new MissionAndRewardTable());
        RegisterAndLoadTable(TableType.StoryLineContent, new StoryLineContentTable());
        
        // TODO : 테이블이 생기면 계속 추가

        yield return new WaitUntil(() =>
        {
            var it = GetTable<ItemTable>(TableType.Item)?.TItem;
            var bp = GetTable<BoxProbTable>(TableType.BoxProb)?.TBoxProb;
            var ac = GetTable<AssemblyContentTable>(TableType.AssemblyContent)?.Contents;
            var mt = GetTable<MonsterTable>(TableType.Monster)?.TMonster;
            var bs = GetTable<BoxSetUpTable>(TableType.BoxSetup)?.TBoxSetup;
            var sj = GetTable<SurvivalJournalTable>(TableType.SurvivalJournal)?.TSurvivalJournal; 
            var cc = GetTable<CollectibleContentTable>(TableType.CollectibleContent)?.TCollectibleContents;
            var fp = GetTable<FoodProbTable>(TableType.FoodProb)?.TFoodProb;
            var fc = GetTable<FixContentTable>(TableType.FixContent)?.FixContents;
            var mr = GetTable<MissionAndRewardTable>(TableType.MissionAndReward)?.TMissionAndReward;
            var slc = GetTable<StoryLineContentTable>(TableType.StoryLineContent)?.TStoryLineContent;

            return it != null
                && bp != null
                && ac != null
                && mt != null
                && bs != null
                && sj != null
                && cc != null
                && fp != null
                && fc != null
                && mr != null
                && slc != null;
        });

        TItems = new List<System.Collections.IList>()
        {
            GetTable<ItemTable>(TableType.Item).TItem,
            GetTable<BoxProbTable>(TableType.BoxProb).TBoxProb,
            GetTable<AssemblyContentTable>(TableType.AssemblyContent).Contents,
            GetTable<MonsterTable>(TableType.Monster).TMonster,
            GetTable<BoxSetUpTable>(TableType.BoxSetup).TBoxSetup,
            GetTable<SurvivalJournalTable>(TableType.SurvivalJournal).TSurvivalJournal,
            GetTable<CollectibleContentTable>(TableType.CollectibleContent).TCollectibleContents,
            GetTable<FoodProbTable>(TableType.FoodProb).TFoodProb,
            GetTable<FixContentTable>(TableType.FixContent).FixContents,
            GetTable<MissionAndRewardTable>(TableType.MissionAndReward).TMissionAndReward,
            GetTable<StoryLineContentTable>(TableType.StoryLineContent).TStoryLineContent
            // TODO : 테이블이 생기면 계속 추가
        };
    }

    private void RegisterAndLoadTable(TableType type, TableBase table)
    {
        _tables[type] = table;
        StartCoroutine(table.Load());
    }

    /// <summary>
    /// 로드된 테이블 인스턴스를 꺼낼 때 사용
    /// </summary>
    public T GetTable<T>(TableType tableType) where T : TableBase
    {
        if (_tables.TryGetValue(tableType, out var table))
            return table as T;

        Debug.LogError($"Can't Find TableType : {tableType}");
        return null;
    }
}

// ItemData 리스트
//var itemTable = TableManager.Instance.GetTable<ItemTable>(TableType.Item);
//List<ItemData> tItem = itemTable.TItem;

// BoxProbData 리스트 (첫 번째 리스트)
// var boxProbTable = TableManager.Instance.GetTable<BoxProbTable>(TableType.BoxProb);
// List<BoxProbData> boxProbs = boxProbTable.BoxProbs;

// 전체 꺼내기
// List<System.Collections.IList> allLists = TableManager.Instance.TItems;

//ItemData 리스트만 꺼내기
//List<ItemData> onlyItems = allLists.OfType<List<ItemData>>().FirstOrDefault();

// 3) 그중 BoxProbData 리스트만 꺼내기
//List<BoxProbData> onlyProbs = allLists.OfType<List<BoxProbData>>().FirstOrDefault();