using Unity.VisualScripting;
using UnityEngine;

public class HitCheck : MonoBehaviour
{
    private bool isHit = false;

    public bool IsHit()
    {
        return isHit;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Object")
        {
            isHit = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Object")
        {
            isHit = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Object")
        {
            isHit = false;
        }
    }
}
