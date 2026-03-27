using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(UIButton))]
public class UIButtonEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawDefaultInspector();
        serializedObject.ApplyModifiedProperties();
    }
}
#endif

public class UIButton : UISelectableItem
{
    [SerializeField, Tooltip("Event invoked when the button is submitted.")]
    public UnityEvent OnSubmitEvent;

    public override void OnSubmit()
    {
        OnSubmitEvent?.Invoke();
    }
}
