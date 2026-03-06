using Unity.Cinemachine;
using UnityEngine;

public class CameraView : MonoBehaviour, ITickable
{
    private PlayerController _controller;
    private static Vector3 defaultFollow;
    private static bool hasDefaultFollow = false;
    private Transform _eyeAnchor;
    private bool _isFirstPerson = true;

    private void Start()
    {
        _controller = GetComponent<PlayerController>();
        if (_controller == null)
        {
            Debug.LogWarning("PlayerControllerが存在しません！");
            return;
        }
        GameObject eyeObj = new GameObject("TemporaryEye");
        _eyeAnchor = eyeObj.transform;
        _eyeAnchor.SetParent(_controller.transform);
        _eyeAnchor.localPosition = new Vector3(0, 1.5f, 0.2f); 
        _eyeAnchor.localRotation = Quaternion.identity;
        Transform transform = _controller.gameObject.transform;
        _controller.vcam.Follow = transform;
        _controller.vcam.LookAt = transform;
        if (!hasDefaultFollow && _controller.follow != null)
        {
            defaultFollow = _controller.follow.FollowOffset;
            hasDefaultFollow = true;
        }
        GameLoop.Instance.Register(this);
    }

    public void To3rdPerson()
    {
        if (_controller == null || !_isFirstPerson) return;
        
        _controller.SetModelVisibility(true);
        _controller.vcam.Follow = _controller.transform;
        _controller.vcam.LookAt = _controller.transform;

        ResetCameraOffset();
        _isFirstPerson = false;
    }

    public void To1stPerson()
    {
        if (_controller == null || _isFirstPerson) return;
        
        _controller.SetModelVisibility(false);
        _controller.vcam.Follow = _eyeAnchor;
        _controller.vcam.LookAt = _eyeAnchor;

        SetCameraOffset(Vector3.zero);
        _isFirstPerson = true;
    }

    public void PlayDeathEffect(Entity_Data.DeathType type)
    {
        // 処理
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

    private bool _isDead = false;
    public void Tick(float deltaTime)
    {
        if (_isDead) return;
        if (_controller.PlayerLogic.State == Entity_Data.PlayerState.Alive) {
            // 生きていたら三人称に
            To3rdPerson();
            return;
        }
        if (_controller.PlayerLogic.State == Entity_Data.PlayerState.Dead) {
            // 死亡したら死体が見えるようにする
            _controller.SetModelVisibility(true);
            _isDead = true;
            GameLoop.Instance.Unregister(this);
            return;
        }
        // 死亡アニメーション中は1人称に
        if (_controller.PlayerLogic.State != Entity_Data.PlayerState.DeathAnimationWait) return;
        To1stPerson();
    }
}