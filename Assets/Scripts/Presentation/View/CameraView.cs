using Unity.Cinemachine;
using UnityEngine;

public class CameraView : MonoBehaviour
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

        To3rdPerson();

        _controller.PlayerLogic.OnDeathAnimationStart += OnDeathAnimationStart;
        GameUseCase.Instance.OnGameOver += OnGameOver;
    }

    private void OnDestroy() {
        if (_controller != null && _controller.PlayerLogic != null)
        {
            _controller.PlayerLogic.OnDeathAnimationStart -= OnDeathAnimationStart;
        }
        if (GameUseCase.Instance != null)
        {
            GameUseCase.Instance.OnGameOver -= OnGameOver;
        }
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
        if (GameUseCase.Instance.IsGameOver) return;
        switch (type)
        {
            case Entity_Data.DeathType.Burned:
                BurnedEffect.Play();
                break;
            case Entity_Data.DeathType.Frozen:
                FrozenEffect.Play();
                break;
            case Entity_Data.DeathType.Crushed:
                CrushEffect.Play();
                break;
            case Entity_Data.DeathType.Dismembered:
                DismemberedEffect.Play();
                break;
            default:
                DefaultDeathEffect.Play();
                break;
        }
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

    private bool _deathAnimationPlayed = false;

    private void OnDeathAnimationStart()
    {
        if (_deathAnimationPlayed) return;
        _deathAnimationPlayed = true;
        GameUseCase.Instance.OnGameOver -= OnGameOver;
        _controller.PlayerLogic.OnDeathAnimationStart -= OnDeathAnimationStart;
        PlayDeathEffect(_controller.PlayerLogic.Type);
        if (GameUseCase.Instance.IsGameOver) return;
        To1stPerson();
    }

    private void OnGameOver()
    {
        To3rdPerson();
    }
}