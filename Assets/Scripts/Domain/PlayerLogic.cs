// UnityEngine禁止

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
    public Tick(float deltaTime){}
    public Die(Entity_Data.DeathType deathType){
        Type = DeathType;
        State = Entity_Data.DeathType.DeathAnimationWait;

        // メンタルを減らす処理を書く
        
    }
    private Move(){
        if (State != Entity_Data.DeathType.Alive) return;
        _controller.Move();
    }
    private Jump(){
        if (State != Entity_Data.DeathType.Alive) return;
        _controller.Jump();
    }
    private Grab(){
        if (State != Entity_Data.DeathType.Alive) return;
        _controller.Grab();
    }
    private Suicide(){
        if (State != Entity_Data.DeathType.Alive) return;
        Die(Entity_Data.DeathType.None);

        // メンタルを追加で減らす処理を書く

        _controller.Suicide();
    }
}
