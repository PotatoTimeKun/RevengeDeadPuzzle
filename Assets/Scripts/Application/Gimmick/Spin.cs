using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField]
    private float _speed = 1.0f;

    private void Update()
    {
        transform.Rotate(0, _speed * Time.deltaTime, 0);
    }
}