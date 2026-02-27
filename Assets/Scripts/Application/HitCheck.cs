using Unity.VisualScripting;
using UnityEngine;
using System;

public class HitCheck : MonoBehaviour
{
    public Action<bool, Collider> IsHit;


    private void OnTriggerEnter(Collider other)
    {
        IsHit?.Invoke(true, other);
    }

    private void OnTriggerStay(Collider other)
    {
        IsHit?.Invoke(true, other);
    }

    private void OnTriggerExit(Collider other)
    {
        IsHit?.Invoke(false, other);
    }
}
