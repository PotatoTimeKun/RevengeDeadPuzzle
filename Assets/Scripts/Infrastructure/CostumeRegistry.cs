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
            Debug.LogWarning("コスチュームが一つも設定されていません！");
            return;
        }

        AllCostumeDic = new();
        foreach (var costume in AllCostume)
        {
            AllCostumeDic.Add(costume.Id, costume);
        }
    }

    public CostumeDef GetById(string id)
    {
        if (AllCostumeDic.Count <= 0) Initialize();

        if (AllCostumeDic.ContainsKey(id))
        {
            return AllCostumeDic[id];
        }
        return null;
    }
}
