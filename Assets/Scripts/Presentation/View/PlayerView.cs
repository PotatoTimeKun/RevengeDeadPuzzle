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
}