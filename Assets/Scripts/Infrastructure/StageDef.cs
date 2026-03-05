using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR

// インスペクター表示にエラーが出るので旧方式に変更

using UnityEditor;
[CustomEditor(typeof(StageDef))]
public class StageDefEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // UI Toolkit (新方式) を使わず、IMGUI (旧方式) で描画する
        serializedObject.Update();
        
        // 従来の「標準的な見た目」でリストを描画
        DrawDefaultInspector();
        
        serializedObject.ApplyModifiedProperties();
    }
}
#endif

[CreateAssetMenu(fileName = "StageDef", menuName = "Scriptable Objects/StageDef")]
public class StageDef : ScriptableObject
{
    public string Id = "";
    public string DisplayName = "";
    public string Scene = "";
    public int MaxMental;
    public int TimerSecondTarget = 0;
    public int DeathCountTarget = 0;
    public List<Entity_Data.DeathType> AcceptedDeathTypeTarget = new();
    public string DeathTypeTargetExplanation = "";
}
