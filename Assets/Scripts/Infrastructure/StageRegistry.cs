using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageRegistry", menuName = "Scriptable Objects/StageRegistry")]
public class StageRegistry : ScriptableObject
{
    public List<StageDef> AllStage = new();
    
    public void GetById(string id)
    {

    }

    public void GetNextStageId(string Id)
    {

    }
}
