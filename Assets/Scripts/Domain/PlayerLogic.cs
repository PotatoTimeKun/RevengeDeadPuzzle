// UnityEngine禁止

using Unity.VisualScripting;
using UnityEngine;

public class PlayerLogic : ITickable
{
    private PlayerController _controller;

    // 入力の登録
    public PlayerLogic(PlayerController controller){
        GameLoop.Instance.Register(this);
        _controller = controller;
        InputHandler.Instance.Player.Move += Move;
        InputHandler.Instance.Player.Jump += Jump;
        InputHandler.Instance.Player.Drag += Grab;
        InputHandler.Instance.Player.Suicide += Suicide;
    }

    // 入力の削除
    ~PlayerLogic(){
        GameLoop.Instance.Unregister(this);
        InputHandler.Instance.Player.Move -= Move;
        InputHandler.Instance.Player.Jump -= Jump;
        InputHandler.Instance.Player.Drag -= Grab;
        InputHandler.Instance.Player.Suicide -= Suicide;
    }
    
    public Entity_Data.PlayerState State;
    public Entity_Data.DeathType Type;
    private float _deathAnimationTimer = 0;
    private float DEATH_ANIMATION_LENGTH = 3; // 死亡アニメーションの長さ
    public void Tick(float deltaTime){
        // 指定時間後にアニメーション状態を解除
        if (State != Entity_Data.PlayerState.DeathAnimationWait) return;
        _deathAnimationTimer += deltaTime;
        if (_deathAnimationTimer >= DEATH_ANIMATION_LENGTH) State = Entity_Data.PlayerState.Dead;
    }
    public void Die(Entity_Data.DeathType deathType){
        Type = deathType;
        State = Entity_Data.PlayerState.DeathAnimationWait;
        // メンタルを減らす
        GameUseCase.Instance.Mental.Decrease(1);
        // スコアに反映
        GameUseCase.Instance.Score.AddDeath(deathType);
    }
    private void Move(Vector2 moveValue){
        if (State != Entity_Data.PlayerState.Alive) return;
        _controller.Move(moveValue);
    }
    private void Jump(){
        if (State != Entity_Data.PlayerState.Alive) return;
        _controller.Jump();
    }
    private void Grab(){
        if (State != Entity_Data.PlayerState.Alive) return;
        _controller.Grab();
    }
    private void Suicide(){
        if (State != Entity_Data.PlayerState.Alive) return;
        Die(Entity_Data.DeathType.None);
        // メンタルを追加で減らす
        GameUseCase.Instance.Mental.Decrease(1);
        _controller.Suicide();
    }
}
