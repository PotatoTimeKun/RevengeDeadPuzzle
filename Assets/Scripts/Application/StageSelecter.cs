using UnityEngine;
using System.Collections.Generic;

public class StageSelecter
{
    private static StageSelecter _instance;
    public static StageSelecter Instance{
        get{
            if (_instance == null) _instance = new StageSelecter();
            return _instance;
        }
    }
    public List<string> UnlockedStageList = new List<string>();

    private StageSelecter()
    {
        StageProgressData data = SaveDataStore.Instance.LoadStageProgress();
        if (data != null && data.UnlockedIdList != null)
        {
            UnlockedStageList = data.UnlockedIdList;
        }
    }

    public void UnlockStage(string id)
    {
        if (!UnlockedStageList.Contains(id))
        {
            UnlockedStageList.Add(id);
            Debug.Log($"新しいステージをアンロック: {id}");
        }
        else
        {
            Debug.Log($"ステージ '{id}' はすでにアンロックされています。");
        }
    }
}
