using UnityEngine;

public class RainbowMaterial : MonoBehaviour
{
    private Material _material;
    private float _offset;

    void Start()
    {
        _material = GetComponent<Renderer>().material;
    }

    void Update()
    {
        _offset = _offset + Time.deltaTime * 0.5f;
        if (_offset > 1f) _offset = 0f;
        _material.color = Color.HSVToRGB(_offset, 1f, 1f);
    }
}
