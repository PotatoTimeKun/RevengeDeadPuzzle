using Unity.Cinemachine;
using UnityEngine;


public class CameraView : MonoBehaviour, ITickable
{
    private PlayerController _controller;
    private Vector3 defaultFollow;
    private Transform _eyeAnchor;
    private bool _isFirstPerson;

    public void Initialize(PlayerController controller)
    {
        if (controller == null)
        {
            Debug.LogWarning("PlayerController�����݂��܂���I");
            return;
        }
        _controller = controller;
        GameObject eyeObj = new GameObject("TemporaryEye");
        _eyeAnchor = eyeObj.transform;
        _eyeAnchor.SetParent(_controller.transform);
        _eyeAnchor.localPosition = new Vector3(0, 1.5f, 0.2f); 
        _eyeAnchor.localRotation = Quaternion.identity;
        Transform transform = controller.gameObject.transform;
        controller.vcam.Follow = transform;
        controller.vcam.LookAt = transform;
        defaultFollow = controller.follow.FollowOffset;
    }
    public void To3rdPerson()
    {
        if (_controller == null || !_isFirstPerson) return;
        {
            _controller.SetModelVisibility(true);
            _controller.vcam.Follow = _controller.transform;
            _controller.vcam.LookAt = _controller.transform;

            ResetCameraOffset();
            _isFirstPerson = false;
        }

        ResetCameraOffset();
    }

    public void To1stPerson()
    {
        if (_controller == null || _isFirstPerson) return;
        {
            _controller.SetModelVisibility(false);
            _controller.vcam.Follow = _eyeAnchor;
            _controller.vcam.LookAt = _eyeAnchor;
        }

        SetCameraOffset(Vector3.zero);
        _isFirstPerson = true;
    }

    public void PlayDeathEffect(Entity_Data.DeathType type)
    {

    }

    public void SetCameraOffset(Vector3 offset)
    {
        if (_controller == null)
        {
            Debug.LogWarning("PlayerController�����݂��܂���I");
            return;
        }
        if (_controller.follow == null)
        {
            Debug.LogWarning("CinemachineFollow�����݂��܂���I");
            return;
        }

        _controller.follow.FollowOffset = offset;
    }

    public void ResetCameraOffset()
    {
        if (_controller == null)
        {
            Debug.LogWarning("PlayerController�����݂��܂���I");
            return;
        }
        if (_controller.follow == null)
        {
            Debug.LogWarning("CinemachineFollow�����݂��܂���I");
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
        // ���񂾂Ƃ��ɏ��������s
        if (_controller.PlayerLogic.State != Entity_Data.PlayerState.DeathAnimationWait) return;
        To1stPerson();
    }
}
