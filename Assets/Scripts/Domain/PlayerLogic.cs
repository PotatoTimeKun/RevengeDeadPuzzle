// UnityEngine禁止

public class PlayerLogic : ITickable
{
    public int HP;
    public Entity_Data.PlayerState State;
    public Entity_Data.DeathType Type;
    public Tick(float deltaTime){}
    public Die(Entity_Data.DeathType deathType){
        
    }
}
