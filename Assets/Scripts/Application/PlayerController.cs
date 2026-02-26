using UnityEngine;

public enum GroundState
{
    Grounded,   // 地面（通常）
    Jumping,    // ジャンプ中
    Falling,    // 落下中
    //OnSlope,    // 斜面（挙動を変える場合）
    //OnWall      // 壁張り付き
}

public class PlayerController : MonoBehaviour, ITickable
{
    public float moveSpeed = 5f;
    public float jumpPower = 10f;
    public HitCheck ground;
    public Rigidbody rb;
    private PlayerLogic playerLogic;
    private PlayerController grabbedObject;
    private GroundState groundState;
    private bool isGrabbing = false;

    private void Awake()
    {
        playerLogic = new PlayerLogic(this);
        InputHandler.Instance.SetInputState(InputState.Player);
    }

    public void Tick(float deltaTime)
    {
        {
            if (groundState != GroundState.Grounded)
            {
                groundState = GroundState.Grounded;
            }
        }
    }

    //移動
    public void Move(Vector2 moveValue)
    {
        Debug.Log(moveValue);
        Vector2 _moveValue = moveValue * moveSpeed;
        Vector2 velocity = new Vector3(_moveValue.x, 0, _moveValue.y);
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

    //掴むと離す
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
    }
}
