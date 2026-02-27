using UnityEngine;

public enum GroundState
{
    Grounded,   // �n�ʁi�ʏ�j
    Jumping,    // �W�����v��
    Falling,    // ������
    //OnSlope,    // �Ζʁi������ς���ꍇ�j
    //OnWall      // �ǒ���t��
}

public class PlayerController : MonoBehaviour, ITickable
{
    public float moveSpeed = 5f;
    public float jumpPower = 10f;
    public HitCheck ground;
    public Rigidbody rb;
    public Collider collider;
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
        if (rb.linearVelocity.y < -0.1f && groundState != GroundState.Jumping)
        {
            groundState = GroundState.Falling;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        groundState = GroundState.Grounded;
    }

    //�ړ�
    public void Move(Vector2 moveValue)
    {
        Vector2 _moveValue = moveValue * moveSpeed;
        Vector3 velocity = new Vector3(_moveValue.x, 0, _moveValue.y);
        rb.linearVelocity = transform.rotation * velocity;
    }

    //�W�����v
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

    //�͂ނƗ���
    public void Grab()
    {
        if (!isGrabbing)
        {
            isGrabbing = true;
            //�͂ޏ���

        }
        else
        {
            isGrabbing = false;
            //��������
        }
    }

    //���E
    public void Suicide()
    {
        rb.constraints = RigidbodyConstraints.None;
        collider.material = null;
    }
}
