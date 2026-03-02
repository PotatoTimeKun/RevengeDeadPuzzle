using UnityEngine;

[CreateAssetMenu(fileName = "CostumeDef", menuName = "Scriptable Objects/CostumeDef")]
public class CostumeDef : ScriptableObject
{
    public string Id = "";
    public GameObject Prefab;
}
