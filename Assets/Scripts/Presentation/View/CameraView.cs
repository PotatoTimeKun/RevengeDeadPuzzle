using Unity.Cinemachine;
using UnityEngine;

public class CameraView : MonoBehaviour, ITickable
{
    private PlayerController _controller;
    private CinemachineCamera _vcam;

    private void Awake()
    {
        _vcam = gameObject.AddComponent<CinemachineCamera>();
    }

    public void Initialize(PlayerController controller)
    {
        if (_vcam == null)
        {
            Debug.LogWarning("CinemachineCameraが見つかりません！");
            return;
        }
        if (controller == null)
        {
            Debug.LogWarning("PlayerControllerが存在しません！");
            return;
        }
        _controller = controller;
        Transform transform = controller.gameObject.transform;
        _vcam.Follow = transform;
        _vcam.LookAt = transform;
    }
    public void To3rdPerson()
    {
        if (_controller == null)
        {
            Debug.LogWarning("PlayerControllerが存在しません！");
            return;
        }


    }

    public void To1stPerson()
    {
        if (_controller == null)
        {
            Debug.LogWarning("PlayerControllerが存在しません！");
            return;
        }


    }

    public void PlayDeathEffect(Entity_Data.DeathType type)
    {

    }

    private void Start()
    {
        GameLoop.Instance.Register(this);
    }

    public void Tick(float deltaTime)
    {

    }
}
