using UnityEngine;

public enum GroundState
{
    Grounded,   // 地面（通常）
    Jumping,    // ジャンプ中
    Falling,    // 落下中
    //OnSlope,    // 斜面（挙動を変える場合）
    //OnWall      // 壁張り付き
}

public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    public PlayerLogic playerLogic;
    private PlayerController grabbedObject;
    private GroundState groundState;
    private bool isGrabbing = false;

    //移動
    public void Move(Vector2 moveValue)
    {
        Vector2 velocity = new Vector3(moveValue.x, 0, moveValue.y);
        transform.position = transform.rotation * velocity;
    }

    //ジャンプ
    public void Jump()
    {

        switch (groundState)
        {
            case GroundState.Grounded:
                groundState = GroundState.Jumping;
                rb.AddForce(Vector3.up * 10f, ForceMode.Impulse);
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
            //掴む処理
        }
        else
        {
            //離す処理
        }
    }

    //自殺
    public void Suicide()
    {
        rb.constraints = RigidbodyConstraints.None;
        enabled = false;
    }
}
