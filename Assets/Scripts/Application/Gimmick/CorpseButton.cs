using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR

// インスペクター表示にエラーが出るので旧方式に変更

using UnityEditor;
[CustomEditor(typeof(CorpseButton))]
public class CorpseButtonEditor : Editor
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

public class CorpseButton : MonoBehaviour
{
    [SerializeField]
    [Tooltip("ボタンが押されたときに実行されるイベント")]
    private UnityEvent onPressed;

    [SerializeField]
    [Tooltip("ボタンが離されたときに実行されるイベント")]
    private UnityEvent onReleased;

    private int corpseCountOnButton = 0;

    private void OnTriggerEnter(Collider other)
    {
        ProcessEnter(other.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        ProcessEnter(collision.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        ProcessExit(other.gameObject);
    }

    private void OnCollisionExit(Collision collision)
    {
        ProcessExit(collision.gameObject);
    }

    private void ProcessEnter(GameObject hitObject)
    {
        PlayerController player = GetPlayerController(hitObject);
        if (player != null && player.PlayerLogic != null)
        {
            if (player.PlayerLogic.State == Entity_Data.PlayerState.Dead)
            {
                corpseCountOnButton++;
                if (corpseCountOnButton == 1)
                {
                    onPressed?.Invoke();
                }
            }
        }
    }

    private void ProcessExit(GameObject hitObject)
    {
        PlayerController player = GetPlayerController(hitObject);
        if (player != null && player.PlayerLogic != null)
        {
            if (player.PlayerLogic.State == Entity_Data.PlayerState.Dead)
            {
                corpseCountOnButton--;
                if (corpseCountOnButton <= 0)
                {
                    corpseCountOnButton = 0;
                    onReleased?.Invoke();
                }
            }
        }
    }

    private PlayerController GetPlayerController(GameObject obj)
    {
        PlayerController player = obj.GetComponent<PlayerController>();
        if (player == null)
        {
            player = obj.GetComponentInParent<PlayerController>();
        }
        return player;
    }
}
