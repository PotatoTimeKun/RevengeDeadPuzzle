using UnityEngine;
using System.Collections;

public class Bridge : MonoBehaviour
{
    [SerializeField] private GameObject _bridgeObject;
    [SerializeField] private Transform _upperPosition;
    [SerializeField] private Transform _lowerPosition;
    private float _moveDuration = 2.0f;

    private Coroutine _moveCoroutine;

    private void Start()
    {
        // 初期位置を下に指定
        if (_bridgeObject != null && _lowerPosition != null)
        {
            _bridgeObject.transform.position = _lowerPosition.position;
        }
    }

    /// <summary>
    /// 橋を上げる
    /// </summary>
    public void Rise()
    {
        MoveTo(_upperPosition.position);
    }

    /// <summary>
    /// 橋を下げる
    /// </summary>
    public void Down()
    {
        MoveTo(_lowerPosition.position);
    }

    private void MoveTo(Vector3 targetPosition)
    {
        if (_moveCoroutine != null)
        {
            StopCoroutine(_moveCoroutine);
        }
        _moveCoroutine = StartCoroutine(MoveToCoroutine(targetPosition));
    }

    private IEnumerator MoveToCoroutine(Vector3 targetPosition)
    {
        Vector3 startPosition = _bridgeObject.transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < _moveDuration)
        {
            float t = elapsedTime / _moveDuration;
            // 動きを滑らかにする（必要に応じてLerpの代わりに使用）
            t = Mathf.SmoothStep(0, 1, t);
            
            _bridgeObject.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _bridgeObject.transform.position = targetPosition;
        _moveCoroutine = null;
    }
}