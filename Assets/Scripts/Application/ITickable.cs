/// <summary>
/// 更新処理が必要なクラスに実装するインターフェース
/// </summary>
public interface ITickable
{
    // deltaTimeは前のフレームからの経過時間
    void Tick(float deltaTime);
}