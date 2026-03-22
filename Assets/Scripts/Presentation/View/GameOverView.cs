using UnityEngine;
using UnityEngine.UI;

public class GameOverView : MonoBehaviour
{
    void Awake()
    {
        InputHandler.Instance.SetInputState(InputState.Menu);

        InputHandler.Instance.Menu.Cancel += RetryAction;
    }
    [SerializeField] private Button _retryButton;
    void Start() {
    if (_retryButton != null) _retryButton.onClick.AddListener(RetryAction);
}

    private void OnDestroy()
    {
        InputHandler.Instance.Menu.Cancel -= RetryAction;
    }

    private void RetryAction()
    {
        Debug.Log("GameOverView: Retry Action Triggered");
        GameUseCase.Instance.RestartGame();
    }
}