using Unity.Cinemachine;
using UnityEngine;

public class CameraView : MonoBehaviour, ITickable
{
    private PlayerController _controller;
    private Vector3 defaultFollow;

    public void Initialize(PlayerController controller)
    {
        if (controller == null)
        {
            Debug.LogWarning("PlayerControllerが存在しません！");
            return;
        }
        _controller = controller;
        Transform transform = controller.gameObject.transform;
        controller.vcam.Follow = transform;
        controller.vcam.LookAt = transform;
        defaultFollow = controller.follow.FollowOffset;
    }
    public void To3rdPerson()
    {
        if (_controller == null)
        {
            Debug.LogWarning("PlayerControllerが存在しません！");
            return;
        }

        ResetCameraOffset();
    }

    public void To1stPerson()
    {
        if (_controller == null)
        {
            Debug.LogWarning("PlayerControllerが存在しません！");
            return;
        }

        SetCameraOffset(new Vector3(0, 2f, 2f));
    }

    public void PlayDeathEffect(Entity_Data.DeathType type)
    {

    }

    public void SetCameraOffset(Vector3 offset)
    {
        if (_controller == null)
        {
            Debug.LogWarning("PlayerControllerが存在しません！");
            return;
        }
        if (_controller.follow == null)
        {
            Debug.LogWarning("CinemachineFollowが存在しません！");
            return;
        }

        _controller.follow.FollowOffset = offset;
    }

    public void ResetCameraOffset()
    {
        if (_controller == null)
        {
            Debug.LogWarning("PlayerControllerが存在しません！");
            return;
        }
        if (_controller.follow == null)
        {
            Debug.LogWarning("CinemachineFollowが存在しません！");
            return;
        }
        _controller.follow.FollowOffset = defaultFollow;
    }

    private void Start()
    {
        GameLoop.Instance.Register(this);
    }

    public void Tick(float deltaTime)
    {
        // 死んだときに処理を実行
        if (_controller.PlayerLogic.State != Entity_Data.PlayerState.DeathAnimationWait) return;
        To1stPerson();
    }
}
