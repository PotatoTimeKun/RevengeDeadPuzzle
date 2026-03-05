using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(StageRegistry))]
public class StageRegistryEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawDefaultInspector();
        serializedObject.ApplyModifiedProperties();
    }
}
#endif

[CreateAssetMenu(fileName = "StageRegistry", menuName = "Scriptable Objects/StageRegistry")]
public class StageRegistry : ScriptableObject
{
    public List<StageDef> AllStages = new();

    private Dictionary<string, StageDef> _stageCache;

    private void OnEnable() => Initialize();
    private void OnValidate() => Initialize();

    private void Initialize()
    {
        _stageCache = new();
        
        if (AllStages == null) return;

        foreach (var stage in AllStages)
        {
            // 規約：Nullチェックとバリデーション
            if (stage == null || string.IsNullOrEmpty(stage.Id)) continue;

            if (_stageCache.ContainsKey(stage.Id))
            {
                Debug.LogError($"{name}: ID '{stage.Id}' が重複しています。ファイル名: {stage.name}");
                continue;
            }
            _stageCache.Add(stage.Id, stage);
        }
    }

    public StageDef GetById(string id)
    {
        if (_stageCache == null) Initialize();
        
        if (_stageCache.TryGetValue(id, out var stage))
        {
            return stage;
        }
        
        Debug.LogWarning($"{name}: ID '{id}' のステージは見つかりません。");
        return null;
    }

    public string GetNextStageId(string currentId)
    {
        for (int i = 0; i < AllStages.Count; i++)
        {
            if (AllStages[i] != null && AllStages[i].Id == currentId)
            {
                int nextIndex = i + 1;
                if (nextIndex < AllStages.Count && AllStages[nextIndex] != null)
                {
                    return AllStages[nextIndex].Id;
                }
                break;
            }
        }
        return null;
    }
}