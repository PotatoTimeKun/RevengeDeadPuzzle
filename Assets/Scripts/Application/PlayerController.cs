using Unity.Cinemachine;
using UnityEngine;

public enum GroundState
{
    Grounded,
    Jumping,
    Falling,
    //OnSlope,
    //OnWall
}

public class PlayerController : MonoBehaviour, ITickable
{
    #region //インスペクター設定
    [HideInInspector] public HitCheck ground;
    [HideInInspector] public PlayerLogic PlayerLogic;
    [HideInInspector] public GameUseCase GameUseCase;
    private CinemachineCamera _vcam;
    public CinemachineCamera vcam
    {
        get { 
            if (_vcam == null) {
                _vcam = FindAnyObjectByType<CinemachineCamera>();
            }
            return _vcam; 
        }
        set { _vcam = value; }
    }

    private CinemachineFollow _follow;
    public CinemachineFollow follow
    {
        get { 
            if (_follow == null) {
                _follow = FindAnyObjectByType<CinemachineFollow>();
            }
            return _follow;
         }
        set { _follow = value; }
    }
    #endregion

    #region //プライベート変数
    private float moveSpeed = 5f;
    private float jumpPower = 6f;
    private Rigidbody rb;
    private PlayerController grabbedObject;
    private GroundState groundState;
    private bool isGrabbing = false;
    private float _grabRange = 1.5f;
    private Transform _grabAnchor;
    private float _throwForce = 10f;
    #endregion
    private Renderer[] _allRenderers;
    private void Awake()
    {
        PlayerLogic = new PlayerLogic(this);
    }

    public void Initialize(GameUseCase gameUseCase)
    {
        GameUseCase = gameUseCase;
    }

    private void Start()
    {
        ground = GetComponentInChildren<HitCheck>();
        ground.IsHit += OnHitGround;
        rb = GetComponent<Rigidbody>();
        if (rb == null) {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.freezeRotation = true;
        }

        _allRenderers = GetComponentsInChildren<Renderer>(true);
        InputHandler.Instance.SetInputState(InputState.Player);

        if (_grabAnchor == null)
        {
            GameObject anchorObj = new GameObject("GrabAnchor");
            _grabAnchor = anchorObj.transform;
            _grabAnchor.SetParent(transform);
            _grabAnchor.localPosition = new Vector3(0, 1f, 1f); // プレイヤーの少し前方に配置
        }

        GameLoop.Instance.Register(this);
    }

    private void OnDisable()
    {
        ground.IsHit -= OnHitGround;
    }
    public void SetModelVisibility(bool visible)
    {
        if (_allRenderers == null) return;
    
        foreach (var r in _allRenderers)
        {
            if (r != null)
            {
                r.enabled = visible;
            }
        }
    }

    public void Tick(float deltaTime)
    {
        if (PlayerLogic.State == Entity_Data.PlayerState.DeathAnimationWait && isGrabbing)
        {
            Grab();
        }
        if (PlayerLogic.State == Entity_Data.PlayerState.Dead)
        {
            GameLoop.Instance.Unregister(this);
            return;
        }
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }

        if (PlayerLogic.State != Entity_Data.PlayerState.Alive)
        {
            _moveValue = Vector2.zero;
        }

        Vector3 velocity = new Vector3(_moveValue.x, 0, _moveValue.y);
        if (velocity.sqrMagnitude > 0.001f && rb.SweepTest(velocity.normalized, out RaycastHit hit, 0.1f, QueryTriggerInteraction.Ignore))
        {
            velocity = Vector3.ProjectOnPlane(velocity, hit.normal);
        }
        velocity.y = rb.linearVelocity.y;
        rb.linearVelocity = velocity;
        if (rb.linearVelocity.y < -0.1f && groundState != GroundState.Jumping)
        {
            groundState = GroundState.Falling;
        }
    }
    private void OnHitGround(bool isHit, Collider other)
    {
        if (isHit)
        {
            groundState = GroundState.Grounded;
        }
        else
        {
            groundState = GroundState.Jumping;
        }
    }

    private Vector2 _moveValue;
    //移動
    public void Move(Vector2 moveValue)
    {
        _moveValue = moveValue * moveSpeed;
        if(moveValue.x == 0 && moveValue.y == 0) return;
        // 2Dの(x, y)を3Dの(x, 0, z)に変換
        Vector3 direction = new Vector3(_moveValue.x, 0, _moveValue.y);
        // その方向を向く回転データを作成して代入
        transform.rotation = Quaternion.LookRotation(direction);
    }
    //ジャンプ
    public void Jump()
    {
        switch (groundState)
        {
            case GroundState.Grounded:
                groundState = GroundState.Jumping;
                rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
                break;

            case GroundState.Jumping:
                break;

            case GroundState.Falling:
                break;
        }
    }

    //掴む・離す
    public void Grab()
    {
        if (!isGrabbing)
        {
            // 周囲の死体を検索して掴む処理
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, _grabRange);
            foreach (var hitCollider in hitColliders)
            {
                PlayerController target = hitCollider.GetComponent<PlayerController>();
                if (target != null && target != this && target.PlayerLogic.State == Entity_Data.PlayerState.Dead)
                {
                    grabbedObject = target;
                    isGrabbing = true;

                    // 掴んだオブジェクトをアンカーに固定
                    grabbedObject.transform.SetParent(_grabAnchor);
                    grabbedObject.transform.localPosition = grabbedObject.transform.localRotation * new Vector3(0, -1.0f, 0);

                    // 物理挙動を無効化して持ち運びやすくする
                    if (grabbedObject.rb != null)
                    {
                        grabbedObject.rb.isKinematic = true;
                    }

                    Collider collider = hitCollider.GetComponent<Collider>();
                    if (collider != null)
                    {
                        collider.isTrigger = true;
                    }
                    break;
                }
            }
        }
        else
        {
            if (grabbedObject != null)
            {
                // 物理挙動を元に戻す
                if (grabbedObject.rb != null)
                {
                    grabbedObject.rb.isKinematic = false;
                
                    // 死因が「切断」の場合は前方に吹き飛ばす
                    if (grabbedObject.PlayerLogic.Type == Entity_Data.DeathType.Dismembered)
                    {
                        grabbedObject.rb.AddForce(transform.forward * _throwForce + Vector3.up * (_throwForce * 0.5f), ForceMode.Impulse);
                    }
                }

                // 親子関係を解除してその場に少し浮かせて置く
                grabbedObject.transform.SetParent(null);
                grabbedObject.transform.position += Vector3.up * 0.5f;

                Collider collider = grabbedObject.GetComponent<Collider>();
                if (collider != null)
                {
                    collider.isTrigger = false;
                }
            }

            grabbedObject = null;
            isGrabbing = false;
        }
    }

    //自殺
    public void Suicide()
    {
        rb.constraints = RigidbodyConstraints.None;
        _moveValue = new Vector2(0,0);
        transform.Rotate(30f, 0, 0, Space.Self);
    }
}
