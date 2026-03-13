using UnityEngine;

public class PlayerView : MonoBehaviour , ITickable
{
    private PlayerController _controller;
    public CostumeRegistry CostumeRegistry;

    private void Awake()
    {
        _controller = GetComponent<PlayerController>();
        SetCostume("Default");
    }

    private void Start()
    {
        GameLoop.Instance.Register(this);
    }

    private void OnDisable()
    {
        GameLoop.Instance.Unregister(this);
    }

    private GameObject _currentCostumeObj;
    public GameObject currentCostumeObj
    {
        get { return _currentCostumeObj; }
        private set { _currentCostumeObj = value; }
    }

    public void SetCostume(string costumeId)
    {
        // コスチュームのプレハブをResources等からロードしてインスタンス化
        GameObject newPrefab = CostumeRegistry.GetById(costumeId);
        if (newPrefab == null)
        {
            Debug.LogError($"[View] Costume Prefab not found: {costumeId}");
            return;
        }

        // 古いコスチュームオブジェクトがあれば破棄
        if (currentCostumeObj != null)
        {
            Destroy(_currentCostumeObj);
        }

        // 新しいコスチュームを子オブジェクトとして生成
        currentCostumeObj = Instantiate(newPrefab, transform);
        currentCostumeObj.transform.localPosition = Vector3.zero;
        currentCostumeObj.transform.localRotation = Quaternion.identity;

        // コスチュームプレハブに付随している不要なコンポーネントを削除
        var c = currentCostumeObj.GetComponent<PlayerController>();
        if (c != null) DestroyImmediate(c);
        
        var pv = currentCostumeObj.GetComponent<PlayerView>();
        if (pv != null) DestroyImmediate(pv);
        
        var cv = currentCostumeObj.GetComponent<CameraView>();
        if (cv != null) DestroyImmediate(cv);

        // 物理挙動が二重にならないように子オブジェクトのRigidbodyのプロパティを親にコピーして削除
        var childRb = currentCostumeObj.GetComponent<Rigidbody>();
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
        _controller.ground = _currentCostumeObj.GetComponentInChildren<HitCheck>();

        Debug.Log($"[View] Visual Updated: {costumeId} (子オブジェクトとして生成完了)");
    }

    private bool _deathCostumeChanged = false;
    public void Tick(float deltaTime){
        if (_controller.PlayerLogic.State != Entity_Data.PlayerState.Dead) return;
        if (_deathCostumeChanged) return;
        SetCostume(_controller.PlayerLogic.Type.ToString());
        _deathCostumeChanged = true;
    }
}