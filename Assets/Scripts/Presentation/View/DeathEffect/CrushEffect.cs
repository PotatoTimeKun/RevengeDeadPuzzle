using UnityEngine;

public class CrushEffect : MonoBehaviour
{
    private static CrushEffect _instance;
    private bool _isPlaying = false;

    [SerializeField] private GameObject _effectObject;

    public static void Play(){
        if (_instance == null) return;
        _instance._isPlaying = true;
        _instance._effectObject.SetActive(true);
        _instance.Invoke(nameof(StopAfterAnimation), 3.0f);
    }

    private void Awake() {
        _instance = this;
        if (!_isPlaying) {
            _effectObject.SetActive(false);
        }
    }

    private void StopAfterAnimation(){
        _isPlaying = false;
        _effectObject.SetActive(false);
    }
}
