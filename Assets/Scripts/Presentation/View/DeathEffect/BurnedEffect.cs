using UnityEngine;
using UnityEngine.Video;

public class BurnedEffect : MonoBehaviour
{
    [SerializeField] private VideoPlayer _videoPlayer;

    private static BurnedEffect _instance;
    private bool _isPlaying = false;

    [SerializeField] private GameObject _effectObject;

    public static void Play(){
        if (_instance == null) return;
        _instance._isPlaying = true;
        _instance._effectObject.SetActive(true);
        _instance._videoPlayer.Play();
    }

    private void Awake()
    {
        _instance = this;
        _videoPlayer.loopPointReached += OnVideoEnd;

        if (!_isPlaying) {
            _effectObject.SetActive(false);
        }
    }

    private void OnVideoEnd(VideoPlayer vp){
        _isPlaying = false;
        _effectObject.SetActive(false);
    }
}