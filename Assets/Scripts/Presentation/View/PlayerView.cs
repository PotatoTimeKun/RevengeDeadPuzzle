using UnityEngine;

public class PlayerView : MonoBehaviour
{
    private PlayerLogic _logic;

    public void Initialize(PlayerLogic logic)
    {
        _logic = logic;
    }

    public void SetCostume(string costumeId)
    {
        // コスチュームのクラスが完成後埋める
        Debug.Log($"[View] Visual Updated: {costumeId}");
    }
}