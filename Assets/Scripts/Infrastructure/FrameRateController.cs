using UnityEngine;

public class FrameRateController : MonoBehaviour
{
    public static FrameRateController Instance { get; private set; }
    private int _targetFrameRate = 60;
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        // 垂直同期（VSync）をオフにする（0 = 同期しない）
        QualitySettings.vSyncCount = 0;

        // 目標とするフレームレートを設定
        Application.targetFrameRate = _targetFrameRate;
    }

    public void SetTargetFrameRate(int frameRate)
    {
        _targetFrameRate = frameRate;
        Application.targetFrameRate = _targetFrameRate;
    }
}