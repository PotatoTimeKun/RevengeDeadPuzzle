using UnityEngine;

public class GenIronPanel : MonoBehaviour
{
    [SerializeField]
    [Tooltip("生成するオブジェクトのプレハブ")]
    private GameObject _ironPanelPrefab;

    /// <summary>
    /// オブジェクトを自身の位置に生成する
    /// </summary>
    public void Generate()
    {
        if (_ironPanelPrefab == null)
        {
            Debug.LogWarning($"{name}: _ironPanelPrefab がインスペクターで設定されていません。");
            return;
        }

        // オブジェクトを自身の位置、自身の回転で生成
        Instantiate(_ironPanelPrefab, transform.position, transform.rotation);
    }
}
