using System.Collections.Generic;
using UnityEngine;

public class GameLoop : MonoBehaviour
{
    // 更新リスト（ITickableを実装したクラスをここに入れる）
    private readonly List<ITickable> _tickables = new List<ITickable>();

    // 登録用メソッド
    public void Register(ITickable tickable)
    {
        if (!_tickables.Contains(tickable))
        {
            _tickables.Add(tickable);
        }
    }

    // 解除用メソッド
    public void Unregister(ITickable tickable)
    {
        _tickables.Remove(tickable);
    }

    // Unity標準のUpdateをここでだけ使う
    private void Update()
    {
        float deltaTime = Time.deltaTime;

        // 登録されているすべてのTickを順番に実行
        // 逆順ループにすることで、ループ中に要素が削除されてもエラーを防げる
        for (int i = _tickables.Count - 1; i >= 0; i--)
        {
            _tickables[i].Tick(deltaTime);
        }
    }
}