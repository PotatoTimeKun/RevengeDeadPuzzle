using UnityEngine;

public class SEPlayer : MonoBehaviour
{
    [SerializeField] private Audio_Data.SEType _seType;
    [SerializeField] private bool _isOneShot = true;
    [SerializeField] private bool _isPlayOnAwake = false;
    private AudioSource _audioSource;
    private void Awake() {
        _audioSource = GetComponent<AudioSource>();
    }
    private void Start() {
        _audioSource.loop = !_isOneShot;
        _audioSource.volume = AudioController.Instance.GetSEVolume();
        Debug.Log(_audioSource.volume);
        if (_isPlayOnAwake) {
            PlaySE();
        }
    }
    public void PlaySE(){
        _audioSource.volume = AudioController.Instance.GetSEVolume();
        if (_isOneShot) {
            _audioSource.PlayOneShot(AudioController.Instance.GetSE(_seType));
        } else {
            _audioSource.clip = AudioController.Instance.GetSE(_seType);
            _audioSource.Play();
            _audioSource.loop = true;
        }
    }
    public void StopSE(){
        _audioSource.Stop();
    }
}
