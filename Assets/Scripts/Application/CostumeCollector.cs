using UnityEngine;
using System.Collections.Generic;

public class CostumeCollector : MonoBehaviour
{
    public CostumeRegistry CostumeRegistry;

    private static CostumeCollector _instance;
    public static CostumeCollector Instance{
        get{
            return _instance;
        }
    }
    [System.NonSerialized]
    public List<string> UnlockedIdList;

    private void Awake()
    {
        _instance = this;
        CostumeData data = SaveDataStore.Instance.LoadUnlockedCostumes();
        if (data != null && data.UnlockedIdList != null)
        {
            UnlockedIdList = data.UnlockedIdList;
        }
        ModifySaveData();
    }

    private void ModifySaveData(){
        // デバッグ等でコスチュームが削除された場合に対応
        for (int i = UnlockedIdList.Count - 1; i >= 0; i--) {
            var costume = CostumeRegistry.GetById(UnlockedIdList[i]);
            if (costume == null) {
                UnlockedIdList.RemoveAt(i);
            }
        }
    }
    
    public string UnlockRandomId()
    { // コスチューム解放
        if (CostumeRegistry == null || CostumeRegistry.AllCostume == null || CostumeRegistry.AllCostume.Count == 0)
        {
            Debug.LogWarning("CostumeRegistryが設定されていないか、コスチュームが一つも登録されていません！");
            return null;
        }
        // 死体のIDを除外
        var restrictedIds = new HashSet<string> { "None", "Burned", "Frozen", "Crushed", "Dismembered" };
        var costumes = CostumeRegistry.AllCostume.FindAll(c => 
            c != null && 
            !restrictedIds.Contains(c.Id));
        // ランダムなコスチュームのIDを返す
        var randomCostume = costumes[Random.Range(0, costumes.Count)];
        if (!UnlockedIdList.Contains(randomCostume.Id)) UnlockedIdList.Add(randomCostume.Id);
        SaveDataStore.Instance.SaveAll(); // 解放したコスチュームの保存
        return randomCostume.Id;
    }

    public void Unlock(string id){
        if (!UnlockedIdList.Contains(id)) UnlockedIdList.Add(id);
        SaveDataStore.Instance.SaveAll(); // 解放したコスチュームの保存
    }
}
