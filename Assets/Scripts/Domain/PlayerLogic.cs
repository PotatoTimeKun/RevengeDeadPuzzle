// UnityEngine禁止

[System.Serializable]
public class PlayerLogic : ITickable
{
    public int HP;
    public float moveSpeed;
    public float jumpPower;
    public Entity_Data.PlayerState State;
    public Entity_Data.DeathType Type;
    public void Tick(float deltaTime){}
    public void Die(Entity_Data.DeathType deathType){}
}
