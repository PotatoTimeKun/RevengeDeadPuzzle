using UnityEngine;

public class PlayerView : MonoBehaviour , ITickable
{
    private PlayerController _controller;

    private string _costumeId;
    private void Awake()
    {
        _controller = GetComponent<PlayerController>();
    }

    private void Start()
    {
        GameLoop.Instance.Register(this);
        _costumeId = _controller.PlayerLogic.CostumeId;
        SetCostume(_costumeId);
    }

    private void OnDisable()
    {
        GameLoop.Instance.Unregister(this);
    }

    private Renderer[] _allRenderers;

    private GameObject _currentCostumeObj;

    private void SetCostume(string costumeId)
    {
        // コスチュームのプレハブをResources等からロードしてインスタンス化
        GameObject newPrefab = CostumeCollector.Instance.CostumeRegistry.GetById(costumeId);
        if (newPrefab == null)
        {
            Debug.LogError($"[View] Costume Prefab not found: {costumeId}");
            return;
        }

        // 古いコスチュームオブジェクトがあれば破棄
        if (_currentCostumeObj != null)
        {
            Destroy(_currentCostumeObj);
        }

        // 新しいコスチュームを子オブジェクトとして生成
        _currentCostumeObj = Instantiate(newPrefab, transform);
        _currentCostumeObj.transform.localPosition = Vector3.zero;
        _currentCostumeObj.transform.localRotation = Quaternion.identity;

        // コスチュームプレハブに付随している不要なコンポーネントを削除
        var c = _currentCostumeObj.GetComponent<PlayerController>();
        if (c != null) DestroyImmediate(c);
        
        var pv = _currentCostumeObj.GetComponent<PlayerView>();
        if (pv != null) DestroyImmediate(pv);
        
        var cv = _currentCostumeObj.GetComponent<CameraView>();
        if (cv != null) DestroyImmediate(cv);

        // 物理挙動が二重にならないように子オブジェクトのRigidbodyのプロパティを親にコピーして削除
        var childRb = _currentCostumeObj.GetComponent<Rigidbody>();
        if (childRb != null) {
            var parentRb = gameObject.GetComponent<Rigidbody>();
            if (parentRb == null) parentRb = gameObject.AddComponent<Rigidbody>();
            
            // 設定を親にコピー
            parentRb.mass = childRb.mass;
            parentRb.linearDamping = childRb.linearDamping;
            parentRb.angularDamping = childRb.angularDamping;
            parentRb.useGravity = childRb.useGravity;
            parentRb.isKinematic = childRb.isKinematic;
            parentRb.interpolation = childRb.interpolation;
            parentRb.collisionDetectionMode = childRb.collisionDetectionMode;
            parentRb.constraints = childRb.constraints;

            DestroyImmediate(childRb);
        }

        // PlayerControllerの参照を更新（HitCheckなど）
        _controller.Ground = _currentCostumeObj.GetComponentInChildren<HitCheck>();

        _allRenderers = _currentCostumeObj.GetComponentsInChildren<Renderer>(true);
        SetModelVisibility(_isVisible);

        Debug.Log($"[View] Visual Updated: {costumeId} (子オブジェクトとして生成完了)");
    }

    public Collider GetCollider()
    {
        return _currentCostumeObj.GetComponent<Collider>();
    }

    private bool _isVisible = true;
    private void SetModelVisibility(bool visible)
    {
        _isVisible = visible;
        if (_allRenderers == null) return;
    
        foreach (var r in _allRenderers)
        {
            if (r != null) r.enabled = visible;
        }
    }

    private bool _deathCostumeChanged = false;
    public void Tick(float deltaTime){
        // コスチュームを変更
        if (_controller.PlayerLogic.CostumeId != _costumeId) {
            SetCostume(_controller.PlayerLogic.CostumeId);
            _costumeId = _controller.PlayerLogic.CostumeId;
        }
        if (GameUseCase.Instance.IsGameOver) {
            // ゲームオーバー時はモデルを表示
            SetModelVisibility(true);
            return;
        }
        if (_controller.PlayerLogic.State == Entity_Data.PlayerState.Alive) {
            // 生存時にモデルを表示
            SetModelVisibility(true);
        }
        if (_controller.PlayerLogic.State == Entity_Data.PlayerState.DeathAnimationWait) {
            // 死亡アニメーション中にモデルを非表示
            SetModelVisibility(false);
        }
        if (_controller.PlayerLogic.State == Entity_Data.PlayerState.Dead) {
            // 死亡時にコスチュームを変更
            if (_deathCostumeChanged) return;
            _deathCostumeChanged = true;
            SetModelVisibility(true);
            if (_controller.PlayerLogic.Type == Entity_Data.DeathType.None) return;
            SetCostume(_controller.PlayerLogic.Type.ToString());
            _controller.PlayerLogic.CostumeId = _controller.PlayerLogic.Type.ToString();
            _costumeId = _controller.PlayerLogic.Type.ToString();
        }
    }
}