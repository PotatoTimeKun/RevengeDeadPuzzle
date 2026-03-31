using UnityEngine;
using System.Collections.Generic;

public class CorpseTransporter : MonoBehaviour
{
    [SerializeField]
    [Tooltip("転送先のTransform")]
    private Transform _destination;

    private readonly HashSet<Collider> _currentColliders = new HashSet<Collider>();

    private void OnTriggerEnter(Collider other)
    {
        if (!_currentColliders.Contains(other))
        {
            _currentColliders.Add(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_currentColliders.Contains(other))
        {
            _currentColliders.Remove(other);
        }
    }

    /// <summary>
    /// オブジェクトを転送先の位置に移動させる
    /// </summary>
    public void Transport()
    {
        if (_destination == null)
        {
            Debug.LogWarning($"{name}: _destination が設定されていません。");
            return;
        }

        // 現在接触しているオブジェクトの中から死んでいるプレイヤーを探して移動させる
        // ループ中にコレクションが変更されないよう、nullチェックや死体判定を行う
        foreach (var collider in _currentColliders)
        {
            if (collider == null) continue;

            PlayerController player = collider.GetComponentInParent<PlayerController>();
            if (player != null && player.PlayerLogic != null)
            {
                // 死んでいるプレイヤーのみを移動
                if (player.PlayerLogic.State == Entity_Data.PlayerState.Dead)
                {
                    player.transform.position = _destination.position;
                }
            }
        }
    }
}
