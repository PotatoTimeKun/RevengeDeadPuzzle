using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CostumeRegistry", menuName = "Scriptable Objects/CostumeRegistry")]
public class CostumeRegistry : ScriptableObject
{
    public List<CostumeDef> AllCostume;

    private Dictionary<string, CostumeDef> AllCostumeDic;


    private void OnEnable()
    {
        Initialize();
    }

    private void Initialize()
    {
        AllCostumeDic = null;
        if (AllCostume == null)
        {
            Debug.LogWarning($"{name}: コスチュームが一つも設定されていません！");
            return;
        }

        AllCostumeDic = new();
        foreach (var costume in AllCostume)
        {
            if (costume == null) continue;
            if (string.IsNullOrEmpty(costume.Id))
            {
                Debug.LogWarning($"{name}: IDが空の要素があります");
                continue;
            }
            if (AllCostumeDic.ContainsKey(costume.Id))
            {
                Debug.LogError($"{name}: ID '{costume.Id}' が重複しています！確認してください。");
                continue;
            }

            AllCostumeDic.Add(costume.Id, costume);
        }
    }

    public GameObject GetById(string id)
    {
        if (AllCostumeDic == null) Initialize();

        if (AllCostumeDic.ContainsKey(id))
        {
            return AllCostumeDic[id].Prefab;
        }
        return null;
    }
}
