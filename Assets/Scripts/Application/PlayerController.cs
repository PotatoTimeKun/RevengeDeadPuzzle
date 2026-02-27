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
    public HitCheck ground;
    #endregion

    #region //プライベート変数
    private float moveSpeed = 5f;
    private float jumpPower = 6f;
    private Rigidbody rb;
    private Collider collider;
    private PlayerLogic playerLogic;
    private PlayerController grabbedObject;
    private GroundState groundState;
    private bool isGrabbing = false;
    #endregion

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        playerLogic = new PlayerLogic(this);
        InputHandler.Instance.SetInputState(InputState.Player);
        GameLoop.Instance.Register(this);
    }
    private void OnEnable()
    {
        ground.IsHit += OnHitGround;
    }

    private void OnDisable()
    {
        ground.IsHit -= OnHitGround;
    }

    public void Tick(float deltaTime)
    {
        Vector3 velocity = new Vector3(_moveValue.x, 0, _moveValue.y);
        if (velocity.sqrMagnitude > 0.001f && rb.SweepTest(velocity.normalized, out RaycastHit hit, 0.1f))
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
            isGrabbing = true;
            //掴む処理
        }
        else
        {
            isGrabbing = false;
            //離す処理
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
