using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultView : MonoBehaviour
{
    public static ResultView Instance { get; private set; }
    private void Awake() {
        Instance = this;
    }

    public static void OpenScene() {
        SceneManager.LoadScene("Result");
    }
}
