using UnityEngine;

public class InputNavigator : MonoBehaviour
{
    public GameObject KeyboardGuide;
    public GameObject GamepadGuide;

    private void Update() {
        if(InputHandler.Instance.IsGamepad) {
            KeyboardGuide.SetActive(false);
            GamepadGuide.SetActive(true);
        } else {
            KeyboardGuide.SetActive(true);
            GamepadGuide.SetActive(false);
        }
    }
}
