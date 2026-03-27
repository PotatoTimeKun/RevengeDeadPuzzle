using UnityEngine;
using System.Collections.Generic;

public class StageSelecter : MonoBehaviour
{
    private static StageSelecter _instance;
    public static StageSelecter Instance{
        get{
            return _instance;
        }
    }
    [HideInInspector] public List<string> UnlockedStageList = new();
    [HideInInspector] public List<ScoreData> ScoreDataList = new();
    public StageRegistry StageRegistry;

    private void Awake() {
        if (_instance == null) _instance = this;
        else {
            Destroy(gameObject);
            return;
        }
        StageProgressData data = SaveDataStore.Instance.LoadStageProgress();
        if (data != null && data.UnlockedIdList != null)
        {
            UnlockedStageList = data.UnlockedIdList;
            ScoreDataList = data.ScoreDataList;
        }
        ModifySaveData();
    }

    private void ModifySaveData(){
        // デバッグ等でステージが削除された場合に対応
        for (int i = UnlockedStageList.Count - 1; i >= 0; i--) {
            var stage = StageRegistry.GetById(UnlockedStageList[i]);
            if (stage == null) {
                UnlockedStageList.RemoveAt(i);
            }
        }
        UnlockedStageList.Add(StageRegistry.AllStages[0].Id);
    }

    private void UnlockStage(string id)
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

    private void AddScore(string stageId,bool time,bool count,bool type){
        var scoreData = ScoreDataList.Find(x => x.StageId == stageId);
        if (scoreData == null)
        {
            scoreData = new ScoreData { StageId = stageId, IsClear = true, TimeTarget = time, CountTarget = count, TypeTarget = type };
            ScoreDataList.Add(scoreData);
            return;
        }
        scoreData.TimeTarget = scoreData.TimeTarget || time;
        scoreData.CountTarget = scoreData.CountTarget || count;
        scoreData.TypeTarget = scoreData.TypeTarget || type;
    }

    public void ClearStage(string stageId,bool time,bool count,bool type){
        string nextStageId = StageRegistry.GetNextStageId(stageId);
        if (nextStageId != null) UnlockStage(nextStageId);
        AddScore(stageId,time,count,type);
        SaveDataStore.Instance.SaveAll();
    }
}
