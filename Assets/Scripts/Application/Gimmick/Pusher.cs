using UnityEngine;

public class Pusher : MonoBehaviour
{
    [SerializeField]
    [Tooltip("動かすオブジェクト")]
    private Transform _targetObject;

    [SerializeField]
    [Tooltip("位置A")]
    private Transform _posA;

    [SerializeField]
    [Tooltip("位置B")]
    private Transform _posB;

    [SerializeField]
    [Tooltip("移動速度")]
    private float _speed = 2.0f;

    private float _t;
    private Rigidbody _rb;

    private void Start()
    {
        if (_targetObject != null)
        {
            _rb = _targetObject.GetComponent<Rigidbody>();
        }
    }

    private void FixedUpdate()
    {
        if (_targetObject == null || _posA == null || _posB == null) return;

        float distance = Vector3.Distance(_posA.position, _posB.position);
        if (distance <= 0) return;

        // 常に一定速度（_speed）で移動するための時間を加算
        // 0~1の範囲を往復するように Mathf.PingPong を使用
        _t += Time.deltaTime * (_speed / distance);
        float lerpValue = Mathf.PingPong(_t, 1f);

        // 必要に応じて動きを滑らかにするなら SmoothStep を通す
        // lerpValue = Mathf.SmoothStep(0, 1, lerpValue);

        Vector3 nextPosition = Vector3.Lerp(_posA.position, _posB.position, lerpValue);

        if (_rb != null)
        {
            _rb.MovePosition(nextPosition);
        }
        else
        {
            _targetObject.position = nextPosition;
        }
    }
}
