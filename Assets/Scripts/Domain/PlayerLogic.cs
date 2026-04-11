// UnityEngine禁止

using Unity.VisualScripting;
using UnityEngine;
using System;

public class PlayerLogic : ITickable
{
    public PlayerLogic(){
        // コスチューム解放
        string costumeId = CostumeCollector.Instance.UnlockRandomId();
        CostumeId = costumeId;
        GameLoop.Instance.Register(this);
    }
    ~PlayerLogic(){
        GameLoop.Instance.Unregister(this);
    }
    
    public Entity_Data.PlayerState State;
    public Entity_Data.DeathType Type;
    private string _costumeId = "Default";
    public string CostumeId {
        get { return _costumeId; }
        set {
            _costumeId = value;
            OnCostumeChange?.Invoke();
        }
    }
    public Action OnCostumeChange;
    private float _deathAnimationTimer = 0;
    private float DEATH_ANIMATION_LENGTH = 3; // 死亡アニメーションの長さ
    public Action OnDead;
    public void Tick(float deltaTime){
        // 指定時間後にアニメーション状態を解除
        if (State != Entity_Data.PlayerState.DeathAnimationWait) return;
        _deathAnimationTimer += deltaTime;
        if (_deathAnimationTimer >= DEATH_ANIMATION_LENGTH && State == Entity_Data.PlayerState.DeathAnimationWait) {
            State = Entity_Data.PlayerState.Dead;
            OnDead?.Invoke();
        }
    }
    public Action OnDeathAnimationStart;
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
        if (GameUseCase.Instance.Mental.CurrentValue <= 0) {
            GameUseCase.Instance.Score.IsClear = false;
        }
        // 死体のコスチュームを設定
        if(deathType != Entity_Data.DeathType.None) {
            CostumeId = deathType.ToString();
            CostumeCollector.Instance.Unlock(CostumeId);
        }
        OnDeathAnimationStart?.Invoke();
    }
}
