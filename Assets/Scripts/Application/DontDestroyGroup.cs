using UnityEngine;

public class DontDestroyGroup : MonoBehaviour
{
    private static DontDestroyGroup _instance;
    public static DontDestroyGroup Instance{
        get{
            return _instance;
        }
    }
    private void Awake() {
        if (_instance == null) _instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
}
