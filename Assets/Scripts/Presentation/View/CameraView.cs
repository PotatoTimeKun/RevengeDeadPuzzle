using Unity.Cinemachine;
using UnityEngine;

public class CameraView : MonoBehaviour, ITickable
{
    private PlayerController _controller;
    private static Vector3 defaultFollow;
    private static bool hasDefaultFollow = false;
    private Transform _eyeAnchor;
    private bool _isFirstPerson = true;

    private CinemachineCamera _vcam;
    private CinemachineCamera vcam
    {
        get { 
            if (_vcam == null) {
                _vcam = FindAnyObjectByType<CinemachineCamera>();
            }
            return _vcam; 
        }
        set { _vcam = value; }
    }

    private CinemachineFollow _follow;
    private CinemachineFollow follow
    {
        get { 
            if (_follow == null) {
                _follow = FindAnyObjectByType<CinemachineFollow>();
            }
            return _follow;
         }
        set { _follow = value; }
    }

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
        vcam.Follow = transform;
        vcam.LookAt = transform;
        if (!hasDefaultFollow && follow != null)
        {
            defaultFollow = follow.FollowOffset;
            hasDefaultFollow = true;
        }
        GameLoop.Instance.Register(this);
    }

    private void OnDestroy() {
        GameLoop.Instance.Unregister(this);
    }

    private void To3rdPerson()
    {
        if (_controller == null || !_isFirstPerson) return;
        vcam.Follow = _controller.transform;
        vcam.LookAt = _controller.transform;

        ResetCameraOffset();
        _isFirstPerson = false;
    }

    private void To1stPerson()
    {
        if (_controller == null || _isFirstPerson) return;
        vcam.Follow = _eyeAnchor;
        vcam.LookAt = _eyeAnchor;

        SetCameraOffset(Vector3.zero);
        _isFirstPerson = true;
    }

    private void PlayDeathEffect(Entity_Data.DeathType type)
    {
        // 処理
    }

    private void SetCameraOffset(Vector3 offset)
    {
        if (_controller == null)
        {
            Debug.LogWarning("PlayerControllerが存在しません！");
            return;
        }
        if (follow == null)
        {
            Debug.LogWarning("CinemachineFollowが存在しません！");
            return;
        }

        follow.FollowOffset = offset;
    }

    private void ResetCameraOffset()
    {
        if (_controller == null)
        {
            Debug.LogWarning("PlayerControllerが存在しません！");
            return;
        }
        if (follow == null)
        {
            Debug.LogWarning("CinemachineFollowが存在しません！");
            return;
        }
        follow.FollowOffset = defaultFollow;
    }

    private bool _isDead = false;
    private bool _deathAnimationPlayed = false;
    public void Tick(float deltaTime)
    {
        if (_isDead) return;
        if (GameUseCase.Instance.IsGameOver) {
            // ゲームオーバー時は三人称に
            To3rdPerson();
            return;
        }
        if (_controller.PlayerLogic.State == Entity_Data.PlayerState.Alive) {
            // 生きていたら三人称に
            To3rdPerson();
            return;
        }
        if (_controller.PlayerLogic.State == Entity_Data.PlayerState.Dead) {
            _isDead = true;
            GameLoop.Instance.Unregister(this);
            return;
        }
        // 死亡アニメーション中は1人称に
        if (_controller.PlayerLogic.State != Entity_Data.PlayerState.DeathAnimationWait) return;
        if (_deathAnimationPlayed) return;
        _deathAnimationPlayed = true;
        PlayDeathEffect(_controller.PlayerLogic.Type);
        To1stPerson();
    }
}