using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR

// インスペクター表示にエラーが出るので旧方式に変更

using UnityEditor;
[CustomEditor(typeof(Button))]
public class ButtonEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // UI Toolkit (新方式) を使わず、IMGUI (旧方式) で描画する
        serializedObject.Update();
        
        // 従来の「標準的な見た目」でリストを描画
        DrawDefaultInspector();
        
        serializedObject.ApplyModifiedProperties();
    }
}
#endif

public class Button : MonoBehaviour
{
    [SerializeField]
    [Tooltip("ボタンが押されたときに実行されるイベント")]
    private UnityEvent onPressed;

    [SerializeField]
    [Tooltip("ボタンが離されたときに実行されるイベント")]
    private UnityEvent onReleased;

    private List<PlayerController> _pressingPlayers = new List<PlayerController>();
    private bool _isPressed = false;

    [SerializeField]
    private GameObject _buttonObject;
    [SerializeField]
    private GameObject _defaultPosition;
    [SerializeField]
    private GameObject _pressedPosition;
    private Vector3 _targetPosition;
    [SerializeField]
    [Tooltip("ボタンが離されても戻らない")]
    private bool _isOneTime = false;

    public enum TargetType {
        Death,
        Alive,
        Both
    }

    [SerializeField]
    [Tooltip("ボタンを押すことができる対象")]
    private TargetType _targetType;

    private void ProcessEnter(Collider other)
    {
        PlayerController player = GetPlayerController(other.gameObject);
        if (player == null || player.PlayerLogic == null) return;
        
        if (!IsValidTarget(player)) return;

        if (!_pressingPlayers.Contains(player))
        {
            _pressingPlayers.Add(player);
            CheckState();
        }
    }

    private void ProcessExit(Collider other)
    {
        if (_isOneTime) return;
        PlayerController player = GetPlayerController(other.gameObject);
        if (player == null) return;

        if (_pressingPlayers.Remove(player))
        {
            CheckState();
        }
    }

    private bool IsValidTarget(PlayerController player)
    {
        if (player == null || player.PlayerLogic == null) return false;
        if (_targetType == TargetType.Death && player.PlayerLogic.State != Entity_Data.PlayerState.Dead) return false;
        if (_targetType == TargetType.Alive && player.PlayerLogic.State == Entity_Data.PlayerState.Dead) return false;
        return true;
    }

    private void CheckState()
    {
        if (_pressingPlayers.Count > 0 && !_isPressed)
        {
            _isPressed = true;
            _targetPosition = _pressedPosition.transform.position;
            onPressed?.Invoke();
        }
        else if (_pressingPlayers.Count == 0 && _isPressed)
        {
            if (_isOneTime) return;
            _isPressed = false;
            _targetPosition = _defaultPosition.transform.position;
            onReleased?.Invoke();
        }
    }

    private PlayerController GetPlayerController(GameObject obj)
    {
        PlayerController player = obj.GetComponentInParent<PlayerController>();
        return player;
    }

    private ButtonPressedObject _buttonPressedObject;

    private void Start()
    {
        _buttonPressedObject = _buttonObject.GetComponent<ButtonPressedObject>();
        _buttonPressedObject.OnButtonPressed += ProcessEnter;
        _buttonPressedObject.OnButtonReleased += ProcessExit;
        _targetPosition = _defaultPosition.transform.position;
    }

    private void Update()
    {
        if (!_isOneTime)
        {
            bool changed = false;
            for (int i = _pressingPlayers.Count - 1; i >= 0; i--)
            {
                PlayerController p = _pressingPlayers[i];
                if (p == null || !IsValidTarget(p))
                {
                    _pressingPlayers.RemoveAt(i);
                    changed = true;
                }
            }
            if (changed)
            {
                CheckState();
            }
        }

        _buttonObject.transform.position = Vector3.Lerp(_buttonObject.transform.position, _targetPosition, Time.deltaTime);
    }
}
