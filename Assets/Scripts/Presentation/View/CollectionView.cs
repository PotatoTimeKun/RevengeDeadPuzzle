using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class CollectionView : MonoBehaviour
{
    [Header("UI Element")]
    public Text ProgressText;

    [Header("Spawning Settings")]
    public Transform PagesRoot;      // ページ生成の基準点
    public float ItemSpacingX = 2.0f; // 横のアイテム間隔
    public float ItemSpacingY = 2.0f; // 縦（上段・下段）のアイテム間隔
    public float PageSpacingX = 20.0f; // ページ間のカメラ移動距離

    private int _currentPage = 0;
    private int _maxPage = 0;
    private Camera _mainCamera;
    private Vector3 _initialCameraPosition;

    private List<GameObject> _spawnedCostumes = new();

    private void Start()
    {
        _mainCamera = Camera.main;
        if (_mainCamera != null) {
            _initialCameraPosition = _mainCamera.transform.position;
        }

        InputHandler.Instance.SetInputState(InputState.Menu);
        InputHandler.Instance.Menu.Move += OnMenuMove;
        InputHandler.Instance.Menu.Cancel += BackToTitle;

        GenerateCollection();
    }

    private void OnDestroy()
    {
        if (InputHandler.Instance != null && InputHandler.Instance.Menu != null)
        {
            InputHandler.Instance.Menu.Move -= OnMenuMove;
            InputHandler.Instance.Menu.Cancel -= BackToTitle;
        }

        if (_mainCamera != null) {
            _mainCamera.transform.position = _initialCameraPosition;
        }
    }

    private void GenerateCollection()
    {
        var allCostumes = CostumeCollector.Instance.CostumeRegistry.AllCostume;
        var unlockedIds = CostumeCollector.Instance.UnlockedIdList;

        if (allCostumes == null) return;

        int unlockedCount = 0;
        foreach(var def in allCostumes) {
            if (unlockedIds.Contains(def.Id)) unlockedCount++;
        }

        if (ProgressText != null) {
            ProgressText.text = $"解放済み {unlockedCount}/{allCostumes.Count}";
        }

        _maxPage = Mathf.CeilToInt((float)allCostumes.Count / 10);
        if (_maxPage == 0) _maxPage = 1;

        for (int i = 0; i < allCostumes.Count; i++)
        {
            var def = allCostumes[i];
            bool isUnlocked = unlockedIds.Contains(def.Id);

            if (!isUnlocked) continue; // 未解放のものはスペースを空けるため生成しない

            int page = i / 10;
            int indexInPage = i % 10;
            int row = indexInPage < 5 ? 0 : 1; // 0=上段(0～4), 1=下段(5～9)
            int col = indexInPage % 5;

            // X軸：各ページごとのオフセット + 中心基準での列オフセット
            float xPos = (page * PageSpacingX) + ((col - 2) * ItemSpacingX);
            // Y軸：上段は上、下段は下のオフセット
            float yPos = row == 0 ? ItemSpacingY / 2 : -ItemSpacingY / 2;

            Vector3 spawnPos = new Vector3(xPos, yPos, 0) + (PagesRoot != null ? PagesRoot.position : Vector3.zero);

            GameObject obj = Instantiate(def.Prefab, spawnPos, def.Prefab.transform.rotation);
            obj.transform.Rotate(0, 150, 0);
            if (PagesRoot != null) obj.transform.SetParent(PagesRoot);
            
            var spin = obj.AddComponent<Spin>();
            spin.Speed = 20f;

            _spawnedCostumes.Add(obj);
        }

        UpdateCameraPosition();
    }

    private void OnMenuMove(Vector2 move)
    {
        // 左右の入力でページ切り替え
        if (move.x > 0.5f) {
            _currentPage++;
            if (_currentPage >= _maxPage) _currentPage = 0; // ループ
            UpdateCameraPosition();
        } 
        else if (move.x < -0.5f) {
            _currentPage--;
            if (_currentPage < 0) _currentPage = _maxPage - 1; // ループ
            UpdateCameraPosition();
        }
    }

    private Vector3 _targetPos;
    private void UpdateCameraPosition()
    {
        if (_mainCamera != null) {
            _targetPos = _initialCameraPosition + new Vector3(_currentPage * PageSpacingX, 0, 0);
        }
    }

    private void Update() {
        if (_mainCamera != null) {
            _mainCamera.transform.position = Vector3.Lerp(_mainCamera.transform.position, _targetPos, Time.deltaTime * 5f);
        }
    }

    // タイトルへ戻るボタン用メソッド
    public void BackToTitle()
    {
        SceneManager.LoadScene("Title");
    }

    public static void OpenScene()
    {
        SceneManager.LoadScene("Collection");
    }
}
