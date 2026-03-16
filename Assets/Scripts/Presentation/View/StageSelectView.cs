using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectView : MonoBehaviour
{
    public static StageSelectView Instance { get; private set; }
    private void Awake() {
        Instance = this;
    }

    public static void OpenScene() {
        SceneManager.LoadScene("StageSelect");
    }
}
