using UnityEngine;

public class Spin : MonoBehaviour
{
    public float Speed = 20.0f;

    private void Update()
    {
        transform.Rotate(0, Speed * Time.deltaTime, 0);
    }
}