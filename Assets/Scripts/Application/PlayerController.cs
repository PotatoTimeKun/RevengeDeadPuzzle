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
    [HideInInspector] public HitCheck Ground;
    [HideInInspector] public PlayerLogic PlayerLogic;
    private float moveSpeed = 5f;
    private float jumpPower = 6f;
    private Rigidbody _rb;
    private PlayerController _grabbedObject;
    private GroundState _groundState;
    private bool _isGrabbing = false;
    private float _grabRange = 1.5f;
    private Transform _grabAnchor;
    private float _throwForce = 10f;
    private void Awake()
    {
        PlayerLogic = new PlayerLogic();
    }

    private void Start()
    {
        // ヒットチェックの初期化
        Ground = GetComponentInChildren<HitCheck>();
        Ground.IsHit += OnHitGround;

        // リジッドボディの初期化
        _rb = GetComponent<Rigidbody>();
        if (_rb == null) {
            _rb = gameObject.AddComponent<Rigidbody>();
            _rb.freezeRotation = true;
        }

        // 掴む位置のアンカーの初期化
        GameObject anchorObj = new GameObject("GrabAnchor");
        _grabAnchor = anchorObj.transform;
        _grabAnchor.SetParent(transform);
        _grabAnchor.localPosition = new Vector3(0, 1f, 1.8f); // プレイヤーの少し前方に配置

        // 入力イベントの登録
        InputHandler.Instance.Player.Move += Move;
        InputHandler.Instance.Player.Jump += Jump;
        InputHandler.Instance.Player.Drag += Grab;
        InputHandler.Instance.Player.Suicide += Suicide;

        GameLoop.Instance.Register(this);
    }
    private void OnDestroy()
    {
        // 入力イベントの登録解除
        InputHandler.Instance.Player.Move -= Move;
        InputHandler.Instance.Player.Jump -= Jump;
        InputHandler.Instance.Player.Drag -= Grab;
        InputHandler.Instance.Player.Suicide -= Suicide;

        GameLoop.Instance.Unregister(this);
    }

    public void Tick(float deltaTime)
    {
        // 死亡時に掴んでいた死体を離す
        if (PlayerLogic.State == Entity_Data.PlayerState.DeathAnimationWait && _isGrabbing)
        {
            Grab();
        }
        // 死亡したらループから外す
        if (PlayerLogic.State == Entity_Data.PlayerState.Dead)
        {
            GameLoop.Instance.Unregister(this);
            return;
        }

        if (_rb == null) _rb = GetComponent<Rigidbody>();

        // 生きていないときは移動させない
        if (PlayerLogic.State != Entity_Data.PlayerState.Alive)
        {
            _moveValue = Vector2.zero;
        }

        // 移動
        Vector3 velocity = new Vector3(_moveValue.x, 0, _moveValue.y);
        if (velocity.sqrMagnitude > 0.001f && _rb.SweepTest(velocity.normalized, out RaycastHit hit, 0.1f, QueryTriggerInteraction.Ignore))
        {
            velocity = Vector3.ProjectOnPlane(velocity, hit.normal);
        }
        velocity.y = _rb.linearVelocity.y;
        _rb.linearVelocity = velocity;
        if (_rb.linearVelocity.y < -0.1f && _groundState != GroundState.Jumping)
        {
            _groundState = GroundState.Falling;
        }
    }
    private void OnHitGround(bool isHit, Collider other)
    {
        if (isHit)
        {
            _groundState = GroundState.Grounded;
        }
        else
        {
            _groundState = GroundState.Jumping;
        }
    }

    private Vector2 _moveValue;
    //移動
    public void Move(Vector2 moveValue)
    {
        if (PlayerLogic.State != Entity_Data.PlayerState.Alive) return;
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
        if (PlayerLogic.State != Entity_Data.PlayerState.Alive) return;
        switch (_groundState)
        {
            case GroundState.Grounded:
                _groundState = GroundState.Jumping;
                _rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
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
        if (PlayerLogic.State != Entity_Data.PlayerState.Alive) return;
        if (!_isGrabbing)
        {
            StartGrab();
        }
        else
        {
            EndGrab();
        }
    }

    private void StartGrab()
    {
        // 周囲の死体を検索して掴む処理
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _grabRange);
        foreach (var hitCollider in hitColliders)
        {
            PlayerController target = hitCollider.GetComponentInParent<PlayerController>();
            if (target == null || target == this || target.PlayerLogic.State != Entity_Data.PlayerState.Dead) continue;

            _grabbedObject = target;
            _isGrabbing = true;

            // 掴んだオブジェクトをアンカーに固定
            _grabbedObject.transform.SetParent(_grabAnchor);
            _grabbedObject.transform.localPosition = _grabbedObject.transform.localRotation * new Vector3(0, -1.0f, 0);

            // 物理挙動を無効化して持ち運びやすくする
            if (_grabbedObject._rb != null) _grabbedObject._rb.isKinematic = true;
            PlayerView view = _grabbedObject.GetComponent<PlayerView>();
            Collider collider = view.currentCostumeObj.GetComponent<Collider>();
            if (collider != null) collider.isTrigger = true;

            break;
        }
    }

    private void EndGrab()
    {
        if (_grabbedObject != null)
        {
            // 物理挙動を元に戻す
            if (_grabbedObject._rb != null)
            {
                _grabbedObject._rb.isKinematic = false;
            
                // 死因が「切断」の場合は前方に吹き飛ばす
                if (_grabbedObject.PlayerLogic.Type == Entity_Data.DeathType.Dismembered)
                {
                    _grabbedObject._rb.AddForce(transform.forward * _throwForce + Vector3.up * (_throwForce * 0.5f), ForceMode.Impulse);
                }
            }

            // 親子関係を解除してその場に少し浮かせて置く
            _grabbedObject.transform.SetParent(null);
            _grabbedObject.transform.position += Vector3.up * 0.5f;

            PlayerView view = _grabbedObject.GetComponent<PlayerView>();
            Collider collider = view.currentCostumeObj.GetComponent<Collider>();
            if (collider != null) collider.isTrigger = false;
        }

        _grabbedObject = null;
        _isGrabbing = false;
    }

    //自殺
    public void Suicide()
    {
        if (PlayerLogic.State != Entity_Data.PlayerState.Alive) return;
        PlayerLogic.Die(Entity_Data.DeathType.None, true);
        _rb.constraints = RigidbodyConstraints.None;
        _moveValue = new Vector2(0,0);
        transform.Rotate(30f, 0, 0, Space.Self);
    }
}
