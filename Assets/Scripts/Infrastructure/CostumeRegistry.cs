using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR

// インスペクター表示にエラーが出るので旧方式に変更

using UnityEditor;
[CustomEditor(typeof(CostumeRegistry))]
public class CostumeRegistryEditor : Editor
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

[CreateAssetMenu(fileName = "CostumeRegistry", menuName = "Scriptable Objects/CostumeRegistry")]
public class CostumeRegistry : ScriptableObject
{
    public List<CostumeDef> AllCostume = new();

    [System.NonSerialized]
    private Dictionary<string, CostumeDef> AllCostumeDic = new();

    void OnEnable(){
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
