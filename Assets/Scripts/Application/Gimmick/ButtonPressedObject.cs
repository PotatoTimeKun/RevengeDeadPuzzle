using UnityEngine;
using System;

public class ButtonPressedObject : MonoBehaviour
{
    public event Action<Collider> OnButtonPressed;
    public event Action<Collider> OnButtonReleased;

    private PlayerController _beforePlayer;
    private float _beforeEnterTime;
    private float _beforeExitTime;
    
    private void OnTriggerEnter(Collider other)
    {
        // 0.1秒以内に同じプレイヤーが複数回トリガーされた場合は無視
        if (_beforePlayer == GetPlayerController(other.gameObject) && Time.time - _beforeEnterTime < 0.1f) return;
        _beforePlayer = GetPlayerController(other.gameObject);
        _beforeEnterTime = Time.time;
        OnButtonPressed?.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (_beforePlayer == GetPlayerController(other.gameObject) && Time.time - _beforeExitTime < 0.1f) return;
        _beforePlayer = GetPlayerController(other.gameObject);
        _beforeExitTime = Time.time;
        OnButtonReleased?.Invoke(other);
    }

    private PlayerController GetPlayerController(GameObject obj)
    {
        PlayerController player = obj.GetComponentInParent<PlayerController>();
        return player;
    }
}
