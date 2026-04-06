using UnityEngine;

public class DismemberedEffect : MonoBehaviour
{
    private static DismemberedEffect _instance;

    [SerializeField] private GameObject _effectObject;

    public static void Play(){
        if (_instance == null) return;
        _instance._effectObject.SetActive(true);
        _instance.Invoke(nameof(_instance.StopAfterAnimation), 3.0f);
    }

    private void Awake() {
        _instance = this;
        _effectObject.SetActive(false);
    }

    private void StopAfterAnimation(){
        _effectObject.SetActive(false);
    }
}
