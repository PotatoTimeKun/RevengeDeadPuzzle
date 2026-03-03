using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CostumeRegistry", menuName = "Scriptable Objects/CostumeRegistry")]
public class CostumeRegistry : ScriptableObject
{
    public List<CostumeDef> AllCostume = new();

    private Dictionary<string, CostumeDef> AllCostumeDic = new();


    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        foreach(var costume in AllCostume)
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
