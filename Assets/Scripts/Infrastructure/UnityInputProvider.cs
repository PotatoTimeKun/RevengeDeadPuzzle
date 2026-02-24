using UnityEngine;
using UnityEngine.InputSystem;

[SerializeField]
public enum InputState
{
    None,
    Player,
    Menu,
}
public class UnityInputProvider : MonoBehaviour
{
    public static UnityInputProvider Instance { get; private set; }

    private InputHandler inputHandler;
    private InputActions inputActions;
    private InputState _currentInputState = InputState.None;
    public InputState CurrentInputState
    {
        get { return _currentInputState; }
        set { _currentInputState = value; }
    }

    private UnityInputProvider()
    {
        inputHandler = InputHandler.Instance;
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
        inputActions.Player.Move.performed += HandlePlayerMove;
        inputActions.Player.Jump.performed += HandlePlayerJump;
        inputActions.Player.Drag.performed += HandlePlayerDrag;
        inputActions.Player.Suicide.performed -= HandlePlayerSuicide;
        inputActions.Player.Menu.performed -= HandlePlayerMenu;

        inputActions.Menu.Move.started -= HandleMenuMove;
        inputActions.Menu.Submit.performed -= HandleMenuSubmit;
        inputActions.Menu.Cancel.performed -= HandleMenuCancel;

        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();

        inputActions.Player.Move.performed -= HandlePlayerMove;
        inputActions.Player.Jump.performed -= HandlePlayerJump;
        inputActions.Player.Drag.performed -= HandlePlayerDrag;
        inputActions.Player.Suicide.performed -= HandlePlayerSuicide;
        inputActions.Player.Menu.performed -= HandlePlayerMenu;

        inputActions.Menu.Move.started -= HandleMenuMove;
        inputActions.Menu.Submit.performed -= HandleMenuSubmit;
        inputActions.Menu.Cancel.performed -= HandleMenuCancel;
    }

    private void HandlePlayerMove(InputAction.CallbackContext context)
    {
        if (currentInputState != InputState.Player)
        {
            inputHandler.Player.Move?.Invoke(Vector2.zero);
            return;
        }
        Vector2 move = context.ReadValue<Vector2>();
        inputHandler.Player.Move?.Invoke(move);
    }

    private void HandlePlayerJump(InputAction.CallbackContext context)
    {
        if (currentInputState != InputState.Player) return;
        inputHandler.Player.Jump?.Invoke();
    }

    private void HandlePlayerDrag(InputAction.CallbackContext context)
    {
        if (currentInputState != InputState.Player) return;
        inputHandler.Player.Drag?.Invoke();
    }

    private void HandlePlayerSuicide(InputAction.CallbackContext context)
    {
        if (currentInputState != InputState.Player) return;
        inputHandler.Player.Suicide?.Invoke();
    }

    private void HandlePlayerMenu(InputAction.CallbackContext context)
    {
        if (currentInputState != InputState.Player) return;
        inputHandler.Player.Menu?.Invoke();
    }

    private void HandleMenuMove(InputAction.CallbackContext context)
    {
        if (currentInputState != InputState.Menu)
        {
            inputHandler.Menu.Move?.Invoke(Vector2.zero);
            return;
        }
        Vector2 move = context.ReadValue<Vector2>();
        inputHandler.Menu.Move?.Invoke(move);
    }

    private void HandleMenuSubmit(InputAction.CallbackContext context)
    {
        if (currentInputState != InputState.Menu) return;
        inputHandler.Menu.Submit?.Invoke();
    }

    private void HandleMenuCancel(InputAction.CallbackContext context)
    {
        if (currentInputState != InputState.Menu) return;
        inputHandler.Menu.Cancel?.Invoke();
    }
}
