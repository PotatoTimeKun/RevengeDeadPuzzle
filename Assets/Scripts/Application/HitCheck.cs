using Unity.VisualScripting;
using UnityEngine;

public class HitCheck : MonoBehaviour
{
    private bool isHit = false;
    private string currentTag = "Object";

    public bool IsHit(string tag = "")
    {
        if (tag != "")
            currentTag = tag;
        return isHit;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == currentTag)
        {
            isHit = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == currentTag)
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
