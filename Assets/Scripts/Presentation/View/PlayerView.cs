using UnityEngine;

public class PlayerView : MonoBehaviour
{
    private PlayerController _controller;

    private string _costumeId;
    private void Awake()
    {
        _controller = GetComponent<PlayerController>();
    }

    private void Start()
    {
        _costumeId = _controller.PlayerLogic.CostumeId;
        SetCostume(_costumeId);

        _controller.PlayerLogic.OnCostumeChange += OnCostumeChange;
        _controller.PlayerLogic.OnDeathAnimationStart += OnDeathAnimationStart;
        _controller.PlayerLogic.OnDead += OnDead;
        GameUseCase.Instance.OnGameOver += OnGameOver;
    }

    private void OnDisable()
    {
        if (_controller != null && _controller.PlayerLogic != null)
        {
            _controller.PlayerLogic.OnCostumeChange -= OnCostumeChange;
            _controller.PlayerLogic.OnDeathAnimationStart -= OnDeathAnimationStart;
            _controller.PlayerLogic.OnDead -= OnDead;
        }
        if (GameUseCase.Instance != null)
        {
            GameUseCase.Instance.OnGameOver -= OnGameOver;
        }
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

        // 物理挙 মন্ত্রিসが二重にならないように子オブジェクトのRigidbodyのプロパティを親にコピーして削除
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

    private void OnCostumeChange()
    {
        if (_controller.PlayerLogic.CostumeId != _costumeId)
        {
            SetCostume(_controller.PlayerLogic.CostumeId);
            _costumeId = _controller.PlayerLogic.CostumeId;
        }
    }

    private void OnDeathAnimationStart()
    {
        SetModelVisibility(false);
    }

    private void OnDead()
    {
        if (_deathCostumeChanged) return;
        _deathCostumeChanged = true;
        SetModelVisibility(true);
        if (_controller.PlayerLogic.Type == Entity_Data.DeathType.None) return;
        _controller.PlayerLogic.CostumeId = _controller.PlayerLogic.Type.ToString();
    }

    private void OnGameOver()
    {
        SetModelVisibility(true);
    }
}