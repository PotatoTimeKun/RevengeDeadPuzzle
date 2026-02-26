// UnityEngine禁止

public class Entity_Data
{
    public enum DeathType { // 死因
        None,
        Burned,
        Frozen,
        Crushed,
        Dismembered
    }
    public enum PlayerState { // プレイヤーの状態
        Alive,
        DeathAnimationWait,
        Dead
    }
}
