using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "StageDef", menuName = "Scriptable Objects/StageDef")]
public class StageDef : ScriptableObject
{
    public string Id = "";
    public string DisplayName = "";
    public string Scene = "";
    public int TimerSecondTarget = 0;
    public int DeathCountTarget = 0;
    public List<Entity_Data.DeathType> AcceptedDeathTypeTarget = new();
    public string DeathTypeTargetExplanation = "";
}
