// UnityEngine禁止

using UnityEngine;

public class PlayerLogic : ITickable
{
    private PlayerController _controller;
    public PlayerLogic(PlayerController controller){
        _controller = controller;
        InputHandler.Instance.Player.Move += Move;
        InputHandler.Instance.Player.Jump += Jump;
        InputHandler.Instance.Player.Drag += Grab;
        InputHandler.Instance.Player.Suicide += Suicide;
    }
    ~PlayerLogic(){
        InputHandler.Instance.Player.Move -= Move;
        InputHandler.Instance.Player.Jump -= Jump;
        InputHandler.Instance.Player.Drag -= Grab;
        InputHandler.Instance.Player.Suicide -= Suicide;
    }
    public Entity_Data.PlayerState State;
    public Entity_Data.DeathType Type;
    public void Tick(float deltaTime){}
    public void Die(Entity_Data.DeathType deathType){
        Type = deathType;
        State = Entity_Data.PlayerState.DeathAnimationWait;

        // メンタルを減らす処理を書く

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

        // メンタルを追加で減らす処理を書く

        _controller.Suicide();
    }
}
