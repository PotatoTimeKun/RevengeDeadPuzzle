using UnityEngine;
using System.Collections.Generic;

public class CostumeCollector
{
    public CostumeRegistry CostumeRegistry;
    private CostumeCollector(){}
    private static CostumeCollector _instance;
    public static CostumeCollector Instance{
        get{
            if (_instance == null) _instance = new CostumeCollector();
            return _instance;
        }
    }
    public List<string> UnlockedIdList = new List<string>();
    public string UnlockRandomId()
    {
        if (CostumeRegistry == null || CostumeRegistry.AllCostume == null || CostumeRegistry.AllCostume.Count == 0)
        {
            Debug.LogWarning("CostumeRegistryが設定されていないか、コスチュームが一つも登録されていません！");
            return null;
        }
        var lockedCostumes = CostumeRegistry.AllCostume.FindAll(c => c != null && !UnlockedIdList.Contains(c.Id));
        if (lockedCostumes.Count == 0)
        {
            Debug.Log("すべてのコスチュームがアンロックされています！");
            return null;
        }
        var randomCostume = lockedCostumes[Random.Range(0, lockedCostumes.Count)];
        UnlockedIdList.Add(randomCostume.Id);
        Debug.Log($"新しいコスチュームをアンロック: {randomCostume.Id}");
        return randomCostume.Id;
    }
}
