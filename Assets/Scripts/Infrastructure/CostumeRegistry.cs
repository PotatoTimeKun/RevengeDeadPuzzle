using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CostumeRegistry", menuName = "Scriptable Objects/CostumeRegistry")]
public class CostumeRegistry : ScriptableObject
{
    public List<CostumeDef> AllCostume = new();

    public void GetById(string id)
    {

    }
}
