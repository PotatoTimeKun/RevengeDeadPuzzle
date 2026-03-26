using UnityEngine;

public class GameOverEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;
    private PlayerController _playerController;
    void Start()
    {
        _playerController = transform.parent.GetComponent<PlayerController>();
    }

    private bool _isPlayed = false;
    void Update()
    {
        if (_playerController.PlayerLogic.State == Entity_Data.PlayerState.Dead && GameUseCase.Instance.IsGameOver && !_isPlayed) {
            _particleSystem.Play();
            _isPlayed = true;
        }
    }
}
