using UnityEngine;
using System;

public class WoodView : MonoBehaviour
{
    [SerializeField] private GameObject fireEffect;

    public void PlayFireEffect()
    {
        fireEffect.SetActive(true);
    }

    void Start(){
        fireEffect.SetActive(false);
    }
}