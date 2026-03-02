using UnityEngine;

public class PlayerView : MonoBehaviour
{
    private PlayerController _controller;

    public void Initialize(PlayerController controller)
    {
        _controller = controller;
    }

    public void SetCostume(string costumeId)
    {
        // コスチュームのクラスが完成後埋める
        Debug.Log($"[View] Visual Updated: {costumeId}");
    }

    // 死んだあと死因のコスチュームに合わせたり初期化時にコスチュームを変更したりする処理を追加する
}