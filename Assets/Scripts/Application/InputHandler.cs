using System;
using UnityEngine;

[SerializeField]
public enum InputState
{
    None,
    Player,
    Menu,
}
public class InputHandler
{
    private static InputHandler _instance;
    public static InputHandler Instance
    {
        get
        {
            if (_instance == null) _instance = new();
            return _instance;
        }
    }

    private InputState _currentInputState = InputState.None;
    public InputState CurrentInputState
    {
        get { return _currentInputState; }
        set { _currentInputState = value; }
    }

    public Action<bool> OnInputMethodChange;
    private bool _isGamepad;
    public bool IsGamepad { 
        get { return _isGamepad; }
        set { 
            if (_isGamepad != value) {
                _isGamepad = value;
                OnInputMethodChange?.Invoke(_isGamepad);
            }
        }
    }


    [SerializeField]
    public class PlayerAction
    {
        public Action<Vector2> Move;
        public Action Jump;
        public Action Drag;
        public Action Suicide;
        public Action Menu;
    }

    [SerializeField]
    public class MenuAction
    {
        public Action<Vector2> Move;
        public Action Submit;
        public Action Cancel;
    }

    public PlayerAction Player = new();
    public MenuAction Menu = new();

    public void SetInputState(InputState state)
    {
        CurrentInputState = state;
    }
}
