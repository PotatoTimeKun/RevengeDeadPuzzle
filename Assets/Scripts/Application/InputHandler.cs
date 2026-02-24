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
    private InputState currentInputState = InputState.None;
    private UnityInputProvider inputProvider;
    private InputActions inputActions;

    /*
     * 
     * 
     * 
     * 
     */

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

    public void SetInputState(InputState state = InputState.None)
    {
        currentInputState = state;
    }

    private void HandlePlayerMove(InputAction.CallbackContext context)
    {
        if (currentInputState != InputState.Player)
        {
            inputProvider.Player.Move?.Invoke(Vector2.zero);
            return;
        }
        Vector2 move = context.ReadValue<Vector2>();
        inputProvider.Player.Move?.Invoke(move);
    }

    private void HandlePlayerJump(InputAction.CallbackContext context)
    {
        if (currentInputState != InputState.Player) return;
        inputProvider.Player.Jump?.Invoke();
    }

    private void HandlePlayerDrag(InputAction.CallbackContext context)
    {
        if (currentInputState != InputState.Player) return;
        inputProvider.Player.Drag?.Invoke();
    }

    private void HandlePlayerSuicide(InputAction.CallbackContext context)
    {
        if (currentInputState != InputState.Player) return;
        inputProvider.Player.Suicide?.Invoke();
    }

    private void HandlePlayerMenu(InputAction.CallbackContext context)
    {
        if (currentInputState != InputState.Player) return;
        inputProvider.Player.Menu?.Invoke();
    }

    private void HandleMenuMove(InputAction.CallbackContext context)
    {
        if (currentInputState != InputState.Menu)
        {
            inputProvider.Menu.Move?.Invoke(Vector2.zero);
            return;
        }
        Vector2 move = context.ReadValue<Vector2>();
        inputProvider.Menu.Move?.Invoke(move);
    }

    private void HandleMenuSubmit(InputAction.CallbackContext context)
    {
        if (currentInputState != InputState.Menu) return;
        inputProvider.Menu.Submit?.Invoke();
    }

    private void HandleMenuCancel(InputAction.CallbackContext context)
    {
        if (currentInputState != InputState.Menu) return;
        inputProvider.Menu.Cancel?.Invoke();
    }
}
