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
    public StageRegistry StageRegistry;

    private void Awake() {
        if (_instance == null) _instance = this;
        else Destroy(gameObject);
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
