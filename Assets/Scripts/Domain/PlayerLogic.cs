// UnityEngine禁止

using Unity.VisualScripting;
using UnityEngine;

public class PlayerLogic : ITickable
{
    public PlayerLogic(){
        GameLoop.Instance.Register(this);
    }
    ~PlayerLogic(){
        GameLoop.Instance.Unregister(this);
    }
    
    public Entity_Data.PlayerState State;
    public Entity_Data.DeathType Type;
    public string CostumeId = "Default";
    private float _deathAnimationTimer = 0;
    private float DEATH_ANIMATION_LENGTH = 3; // 死亡アニメーションの長さ
    public void Tick(float deltaTime){
        // 指定時間後にアニメーション状態を解除
        if (State != Entity_Data.PlayerState.DeathAnimationWait) return;
        _deathAnimationTimer += deltaTime;
        if (_deathAnimationTimer >= DEATH_ANIMATION_LENGTH) State = Entity_Data.PlayerState.Dead;
    }
    public void Die(Entity_Data.DeathType deathType, bool isSuicide){
        if (State == Entity_Data.PlayerState.Goal) return; // ゴールしていたら死なない
        Type = deathType;
        State = Entity_Data.PlayerState.DeathAnimationWait;
        // メンタルを減らす
        GameUseCase.Instance.Mental.Decrease(1);
        if (isSuicide) {
            // 自殺の場合はメンタルをさらに減らす
            GameUseCase.Instance.Mental.Decrease(1);
        }
        // スコアに反映
        GameUseCase.Instance.Score.AddDeath(deathType);
    }
}
