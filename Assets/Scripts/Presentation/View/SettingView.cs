using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingView : MonoBehaviour
{
    public static SettingView Instance { get; private set; }
    private void Awake() {
        Instance = this;
    }

    public static void OpenScene() {
        SceneManager.LoadScene("Setting", LoadSceneMode.Additive);
    }

    public void CloseScene() {
        SceneManager.UnloadSceneAsync(gameObject.scene);
    }
}
