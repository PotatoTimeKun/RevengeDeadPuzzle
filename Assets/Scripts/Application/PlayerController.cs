using Unity.Cinemachine;
using UnityEngine;

public enum GroundState
{
    Grounded,
    Jumping,
    Falling,
}

public class PlayerController : MonoBehaviour, ITickable
{
    [HideInInspector] public PlayerLogic PlayerLogic;
    private Rigidbody _rb;
    public Rigidbody Rigidbody => _rb;

    private PlayerMovement _movement;
    private PlayerJump _jump;
    private PlayerGrab _grab;

    private void Awake()
    {
        PlayerLogic = new PlayerLogic();
        _movement = new PlayerMovement(this);
        _jump = new PlayerJump(this);
        _grab = new PlayerGrab(this);
        PlayerLogic.OnDead += OnDead;
        PlayerLogic.OnDeathAnimationStart += OnDeathAnimation;
    }

    private void Start()
    {
        // リジッドボディの初期化
        _rb = GetComponent<Rigidbody>();
        if (_rb == null) {
            _rb = gameObject.AddComponent<Rigidbody>();
            _rb.freezeRotation = true;
        }

        _movement.Init();
        _jump.Init();
        _grab.Init();

        InputHandler.Instance.Player.Suicide += Suicide;
        GameUseCase.Instance.OnGameClear += OnGameClear;

        GameLoop.Instance.Register(this);
    }

    private void OnDestroy()
    {
        _movement.Dispose();
        _jump.Dispose();
        _grab.Dispose();

        InputHandler.Instance.Player.Suicide -= Suicide;
        if (GameUseCase.Instance != null) GameUseCase.Instance.OnGameClear -= OnGameClear;

        GameLoop.Instance.Unregister(this);
        PlayerLogic.OnDead -= OnDead;
        PlayerLogic.OnDeathAnimationStart -= OnDeathAnimation;
    }

    private void OnDead(){
        GameLoop.Instance.Unregister(this);
    }

    private void OnDeathAnimation(){
        switch (PlayerLogic.Type) {
            case Entity_Data.DeathType.Burned:
                AudioController.Instance.PlaySE(Audio_Data.SEType.DeathByFire);
                break;
            case Entity_Data.DeathType.Crushed:
                AudioController.Instance.PlaySE(Audio_Data.SEType.DeathByCrush);
                break;
            case Entity_Data.DeathType.Dismembered:
                AudioController.Instance.PlaySE(Audio_Data.SEType.DeathByDismemberment);
                break;
            case Entity_Data.DeathType.Frozen:
                AudioController.Instance.PlaySE(Audio_Data.SEType.DeathByFreeze);
                break;
            case Entity_Data.DeathType.None:
                AudioController.Instance.PlaySE(Audio_Data.SEType.DeathByDefault);
                break;
        }
    }

    private void OnGameClear(){
        GameLoop.Instance.Unregister(this);
    }

    public void Tick(float deltaTime)
    {
        if (transform.position.y < -10 && PlayerLogic.State == Entity_Data.PlayerState.Alive) {
            // 落下死
            PlayerLogic.Die(Entity_Data.DeathType.None, false);
            Destroy(gameObject, 10f);
        }

        _grab.Tick(deltaTime);

        if (_rb == null) _rb = GetComponent<Rigidbody>();

        _movement.Tick(deltaTime);
        _jump.Tick(deltaTime);
    }

    //自殺
    private void Suicide()
    {
        if (PlayerLogic.State != Entity_Data.PlayerState.Alive) return;
        PlayerLogic.Die(Entity_Data.DeathType.None, true);
        _rb.constraints = RigidbodyConstraints.None;
        _movement.ResetMove();
        transform.Rotate(30f, 0, 0, Space.Self);
    }

    private class PlayerMovement
    {
        private PlayerController _player;
        private float moveSpeed = 5f;
        private Vector2 _moveValue;

        public PlayerMovement(PlayerController player)
        {
            _player = player;
        }

        public void Init()
        {
            InputHandler.Instance.Player.Move += Move;
            _player.PlayerLogic.OnDeathAnimationStart += OnDead;
        }

        public void Dispose()
        {
            InputHandler.Instance.Player.Move -= Move;
            _player.PlayerLogic.OnDeathAnimationStart -= OnDead;
        }

        public void ResetMove()
        {
            _moveValue = Vector2.zero;
        }

        private void Move(Vector2 moveValue)
        {
            if (_player.PlayerLogic.State != Entity_Data.PlayerState.Alive) return;
            _moveValue = moveValue * moveSpeed;
            if(moveValue.x == 0 && moveValue.y == 0) return;
            // 2Dの(x, y)を3Dの(x, 0, z)に変換
            Vector3 direction = new Vector3(_moveValue.x, 0, _moveValue.y);
            // その方向を向く回転データを作成して代入
            _player.transform.rotation = Quaternion.LookRotation(direction);
        }

        private void OnDead()
        { // 生きていないときは移動させない
            _moveValue = Vector2.zero;
        }

        public void Tick(float deltaTime)
        {
            // 移動
            Vector3 velocity = new Vector3(_moveValue.x, 0, _moveValue.y);
            if (velocity.sqrMagnitude > 0.001f && _player.Rigidbody.SweepTest(velocity.normalized, out RaycastHit hit, 0.1f, QueryTriggerInteraction.Ignore))
            {
                // 動的なRigidbody（死体などのPhysicsオブジェクト）にはSlideせず、押し込めるようにする
                if (hit.rigidbody == null || hit.rigidbody.isKinematic)
                {
                    velocity = Vector3.ProjectOnPlane(velocity, hit.normal);
                }
            }
            velocity.y = _player.Rigidbody.linearVelocity.y;
            _player.Rigidbody.linearVelocity = velocity;
        }
    }

    private class PlayerJump
    {
        private PlayerController _player;
        private HitCheck _ground;
        private float jumpPower = 6f;
        private GroundState _groundState;

        public PlayerJump(PlayerController player)
        {
            _player = player;
        }

        public void Init()
        {
            // ヒットチェックの初期化
            _ground = _player.GetComponentInChildren<HitCheck>();
            if (_ground != null)
            {
                _ground.IsHit += OnHitGround;
            }

            InputHandler.Instance.Player.Jump += Jump;
        }

        public void Dispose()
        {
            if (_ground != null)
            {
                _ground.IsHit -= OnHitGround;
            }

            InputHandler.Instance.Player.Jump -= Jump;
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

        private void Jump()
        {
            if (_player.PlayerLogic.State != Entity_Data.PlayerState.Alive) return;
            switch (_groundState)
            {
                case GroundState.Grounded:
                    _groundState = GroundState.Jumping;
                    _player.Rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
                    AudioController.Instance.PlaySE(Audio_Data.SEType.Jump);
                    break;

                case GroundState.Jumping:
                    break;

                case GroundState.Falling:
                    break;
            }
        }

        public void Tick(float deltaTime)
        {
            if (_player.Rigidbody.linearVelocity.y < -0.1f && _groundState != GroundState.Jumping)
            {
                _groundState = GroundState.Falling;
            }
        }
    }

    private class PlayerGrab
    {
        private PlayerController _player;
        private PlayerController _grabbedObject;
        private bool _isGrabbing = false;
        private float _grabRange = 1.5f;
        private Transform _grabAnchor;
        private float _throwForce = 10f;

        public PlayerGrab(PlayerController player)
        {
            _player = player;
        }

        public void Init()
        {
            // 掴む位置のアンカーの初期化
            GameObject anchorObj = new GameObject("GrabAnchor");
            _grabAnchor = anchorObj.transform;
            _grabAnchor.SetParent(_player.transform);
            _grabAnchor.localPosition = new Vector3(0, 1f, 1.8f); // プレイヤーの少し前方に配置

            InputHandler.Instance.Player.Drag += Grab;
            _player.PlayerLogic.OnDeathAnimationStart += OnDead;
        }

        public void Dispose()
        {
            InputHandler.Instance.Player.Drag -= Grab;
            _player.PlayerLogic.OnDeathAnimationStart -= OnDead;
        }

        private void Grab()
        {
            if (_player.PlayerLogic.State != Entity_Data.PlayerState.Alive) return;
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
            Collider[] hitColliders = Physics.OverlapSphere(_player.transform.position, _grabRange);
            foreach (var hitCollider in hitColliders)
            {
                PlayerController target = hitCollider.GetComponentInParent<PlayerController>();
                if (target == null || target == _player || target.PlayerLogic.State != Entity_Data.PlayerState.Dead) continue;

                _grabbedObject = target;
                _isGrabbing = true;

                // 掴んだオブジェクトをアンカーに固定
                _grabbedObject.transform.SetParent(_grabAnchor);
                _grabbedObject.transform.localPosition = _grabbedObject.transform.localRotation * new Vector3(0, -1.0f, 0);

                // 物理挙動を無効化して持ち運びやすくする
                if (_grabbedObject.Rigidbody != null) _grabbedObject.Rigidbody.isKinematic = true;
                PlayerView view = _grabbedObject.GetComponent<PlayerView>();
                if (view != null)
                {
                    Collider collider = view.GetCollider();
                    if (collider != null) collider.isTrigger = true;
                }
                AudioController.Instance.PlaySE(Audio_Data.SEType.Grab);

                break;
            }
        }

        private void EndGrab()
        {
            if (_grabbedObject != null)
            {
                // 物理挙動を元に戻す
                if (_grabbedObject.Rigidbody != null)
                {
                    _grabbedObject.Rigidbody.isKinematic = false;
                
                    // 死因が「切断」の場合は前方に吹き飛ばす
                    if (_grabbedObject.PlayerLogic.Type == Entity_Data.DeathType.Dismembered)
                    {
                        _grabbedObject.Rigidbody.AddForce(_player.transform.forward * _throwForce + Vector3.up * (_throwForce * 0.5f), ForceMode.Impulse);
                    }
                }

                // 親子関係を解除してその場に少し浮かせて置く
                _grabbedObject.transform.SetParent(null);
                _grabbedObject.transform.position += Vector3.up * 0.5f;

                PlayerView view = _grabbedObject.GetComponent<PlayerView>();
                if (view != null)
                {
                    Collider collider = view.GetCollider();
                    if (collider != null) collider.isTrigger = false;
                }
                AudioController.Instance.PlaySE(Audio_Data.SEType.Release);
            }

            _grabbedObject = null;
            _isGrabbing = false;
        }

        private void OnDead()
        { // 死亡時に掴んでいた死体を離す
            EndGrab();
        }

        public void Tick(float deltaTime)
        {
        }
    }
}
