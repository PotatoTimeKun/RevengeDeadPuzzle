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
    public PhysicsMaterial zeroFriction;
    #endregion

    #region //プライベート変数
    private float moveSpeed = 5f;
    private float jumpPower = 10f;
    private Rigidbody rb;
    private Collider collider;
    private PlayerLogic playerLogic;
    private PlayerController grabbedObject;
    private GroundState groundState;
    private bool isGrabbing = false;
    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        playerLogic = new PlayerLogic(this);
        InputHandler.Instance.SetInputState(InputState.Player);
        collider.material = zeroFriction;
    }

    public void Tick(float deltaTime)
    {
        if (rb.linearVelocity.y < -0.1f && groundState != GroundState.Jumping)
        {
            groundState = GroundState.Falling;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        groundState = GroundState.Grounded;
    }

    //移動
    public void Move(Vector2 moveValue)
    {
        Vector2 _moveValue = moveValue * moveSpeed;
        Vector3 velocity = new Vector3(_moveValue.x, rb.linearVelocity.y, _moveValue.y);
        rb.linearVelocity = transform.rotation * velocity;
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
        collider.material = null;
        rb.AddForce(transform.forward, ForceMode.Impulse);
    }
}
