using UnityEngine;
using UnityEngine.InputSystem;

[SerializeField]
public enum InputState
{
    None,
    Player,
    Menu,
}
public class InputHandler : MonoBehaviour
{
    public static InputHandler Instance {  get; private set; }
    InputState currentInputState = InputState.None;
    private UnityInputProvider inputProvider;
    private InputActions inputActions;

    private InputHandler()
    {
        inputProvider = UnityInputProvider.Instance;
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        inputActions.Player.Move.performed += OnPlayerMove;
        inputActions.Player.Jump.performed += OnPlayerJump;
        inputActions.Player.Drag.performed += OnPlayerDrag;
        inputActions.Player.Suicide.performed += OnPlayerSuicide;
        inputActions.Player.Menu.performed += OnPlayerMenu;

        inputActions.Menu.Move.started += OnMenuMove;
        inputActions.Menu.Submit.performed += OnMenuSubmit;
        inputActions.Menu.Cancel.performed += OnMenuCancel;

        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();

        inputActions.Player.Move.performed -= OnPlayerMove;
        inputActions.Player.Jump.performed -= OnPlayerJump;
        inputActions.Player.Drag.performed -= OnPlayerDrag;
        inputActions.Player.Suicide.performed -= OnPlayerSuicide;
        inputActions.Player.Menu.performed -= OnPlayerMenu;

        inputActions.Menu.Move.started -= OnMenuMove;
        inputActions.Menu.Submit.performed -= OnMenuSubmit;
        inputActions.Menu.Cancel.performed -= OnMenuCancel;
    }

    public void SetInputState(InputState state = InputState.None)
    {
        currentInputState = state;
    }

    public void OnPlayerMove(InputAction.CallbackContext context)
    {
        if (currentInputState != InputState.Player)
        {
            inputProvider.Player.Move?.Invoke(Vector2.zero);
            return;
        }
        Vector2 move = context.ReadValue<Vector2>();
        inputProvider.Player.Move?.Invoke(move);
    }

    public void OnPlayerJump(InputAction.CallbackContext context)
    {
        if (currentInputState != InputState.Player) return;
        inputProvider.Player.Jump?.Invoke();
    }

    public void OnPlayerDrag(InputAction.CallbackContext context)
    {
        if (currentInputState != InputState.Player) return;
        inputProvider.Player.Drag?.Invoke();
    }

    public void OnPlayerSuicide(InputAction.CallbackContext context)
    {
        if (currentInputState != InputState.Player) return;
        inputProvider.Player.Suicide?.Invoke();
    }

    public void OnPlayerMenu(InputAction.CallbackContext context)
    {
        if (currentInputState != InputState.Player) return;
        inputProvider.Player.Menu?.Invoke();
    }

    public void OnMenuMove(InputAction.CallbackContext context)
    {
        if (currentInputState != InputState.Menu)
        {
            inputProvider.Menu.Move?.Invoke(Vector2.zero);
            return;
        }
        Vector2 move = context.ReadValue<Vector2>();
        inputProvider.Menu.Move?.Invoke(move);
    }

    public void OnMenuSubmit(InputAction.CallbackContext context)
    {
        if (currentInputState != InputState.Menu) return;
        inputProvider.Menu.Submit?.Invoke();
    }

    public void OnMenuCancel(InputAction.CallbackContext context)
    {
        if (currentInputState != InputState.Menu) return;
        inputProvider.Menu.Cancel?.Invoke();
    }
}
