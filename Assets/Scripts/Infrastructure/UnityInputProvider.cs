using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class UnityInputProvider
{
    private static UnityInputProvider _instance;
    public static UnityInputProvider Instance
    {
        get
        {
            if (_instance == null) _instance = new();
            return _instance;
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
}
