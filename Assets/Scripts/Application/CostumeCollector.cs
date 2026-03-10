using UnityEngine;
using System.Collections.Generic;

public class CostumeCollector : MonoBehaviour
{
    public CostumeRegistry CostumeRegistry;

    private static CostumeCollector _instance;
    public static CostumeCollector Instance{
        get{
            if (_instance == null) _instance = new CostumeCollector();
            return _instance;
        }
    }
    [System.NonSerialized]
    public List<string> UnlockedIdList;

    private void Awake()
    {
        CostumeData data = SaveDataStore.Instance.LoadUnlockedCostumes();
        if (data != null && data.UnlockedIdList != null)
        {
            UnlockedIdList = data.UnlockedIdList;
        }
    }
    public string UnlockRandomId()
    { // ランダムなコスチュームのIDを返す
        if (CostumeRegistry == null || CostumeRegistry.AllCostume == null || CostumeRegistry.AllCostume.Count == 0)
        {
            Debug.LogWarning("CostumeRegistryが設定されていないか、コスチュームが一つも登録されていません！");
            return null;
        }
        var restrictedIds = new HashSet<string> { "None", "Burned", "Frozen", "Crushed", "Dismembered" };
        // 死体のIDを除外
        var costumes = CostumeRegistry.AllCostume.FindAll(c => 
            c != null && 
            !restrictedIds.Contains(c.Id));
        var randomCostume = costumes[Random.Range(0, costumes.Count)];
        if (!UnlockedIdList.Contains(randomCostume.Id)) UnlockedIdList.Add(randomCostume.Id);
        return randomCostume.Id;
    }
}
