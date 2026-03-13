using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    private Light _light;
    public float minIntensity = 1.5f; // 最小の明るさ
    public float maxIntensity = 3.0f; // 最大の明るさ
    public float speed = 5.0f;        // 揺れる速さ

    void Start() => _light = GetComponent<Light>();

    void Update()
    {
        // パーリンノイズを使って自然なゆらぎを作る
        float noise = Mathf.PerlinNoise(Time.time * speed, 0);
        _light.intensity = Mathf.Lerp(minIntensity, maxIntensity, noise);
    }
}