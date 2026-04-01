using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class TitleView : MonoBehaviour
{
    [Header("Costume Spawning Settings")]
    public Transform[] SpawnPoints;
    public float SpawnInterval = 2.0f; // 定期生成の間隔（秒）

    private float _spawnTimer = 0f;

    private void Start()
    {
        InputHandler.Instance.SetInputState(InputState.Menu);
        GenerateUnlockedCostumes();
    }

    private void Update()
    {
        _spawnTimer += Time.deltaTime;
        if (_spawnTimer >= SpawnInterval)
        {
            _spawnTimer = 0f;
            GenerateUnlockedCostumes();
        }
    }

    private void GenerateUnlockedCostumes()
    {
        if (SpawnPoints == null || SpawnPoints.Length == 0) return;

        var unlockedIds = CostumeCollector.Instance.UnlockedIdList;
        List<string> availableIds;

        // 出すものがない（未解放）ときはDefaultコスチュームを出す
        if (unlockedIds == null || unlockedIds.Count == 0)
        {
            availableIds = new List<string> { "Default" };
        }
        else
        {
            availableIds = new List<string>(unlockedIds);
        }
        
        for (int i = 0; i < SpawnPoints.Length; i++)
        {
            if (availableIds.Count == 0) break; 

            int randIndex = Random.Range(0, availableIds.Count);
            string selectedId = availableIds[randIndex];

            GameObject prefab = CostumeCollector.Instance.CostumeRegistry.GetById(selectedId);
            if (prefab != null)
            {
                Transform point = SpawnPoints[i];
                // ランダムな回転を初期化時につけて生成
                GameObject obj = Instantiate(prefab, point.position, Random.rotation);
                obj.transform.SetParent(point);
                obj.AddComponent<Rigidbody>();

                // 30秒経ったものは消えるようにする
                Destroy(obj, 30f);
            }
        }
    }

    // 各画面への遷移メソッド
    public void OnClickStageSelect()
    {
        StageSelectView.OpenScene();
    }

    public void OnClickCollection()
    {
        // CollectionViewを開く
        SceneManager.LoadScene("CollectionScene"); 
    }

    public void OnClickSetting()
    {
        SettingView.OpenScene();
    }
}
