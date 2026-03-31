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

    private int _countOnButton = 0;

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
        if (_targetType == TargetType.Death && player.PlayerLogic.State != Entity_Data.PlayerState.Dead) {
            return;
        }
        if (_targetType == TargetType.Alive && player.PlayerLogic.State == Entity_Data.PlayerState.Dead) {
            return;
        }
        _countOnButton++;
        if (_countOnButton == 1)
        {
            _targetPosition = _pressedPosition.transform.position;
            onPressed?.Invoke();
        }
    }

    private void ProcessExit(Collider other)
    {
        if (_isOneTime) return;
        PlayerController player = GetPlayerController(other.gameObject);
        if (player == null || player.PlayerLogic == null) return;
        if (_targetType == TargetType.Death && player.PlayerLogic.State != Entity_Data.PlayerState.Dead) {
            return;
        }
        if (_targetType == TargetType.Alive && player.PlayerLogic.State == Entity_Data.PlayerState.Dead) {
            return;
        }
        _countOnButton--;
        if (_countOnButton <= 0)
        {
            _countOnButton = 0;
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
        Debug.Log(_countOnButton);
        _buttonObject.transform.position = Vector3.Lerp(_buttonObject.transform.position, _targetPosition, Time.deltaTime);
    }
}
