using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(Application.View.UI.UIButton))]
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

namespace Application.View.UI
{
    public class UIButton : UISelectableItem
    {
        [SerializeField, Tooltip("Event invoked when the button is submitted.")]
        public UnityEvent OnSubmitEvent;

        public override void OnSubmit()
        {
            OnSubmitEvent?.Invoke();
        }
    }
}
