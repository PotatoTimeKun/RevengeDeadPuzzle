using UnityEngine;

public class Audio_Data : MonoBehaviour
{
    public enum BGMType
    {
        None,
        Title,
        Game,
        Result,
        GameOver
    }

    public enum SEType
    {
        Jump,
        Grab,
        Release,
        Button,
        DeathByFire,
        DeathByCrush,
        DeathByDismemberment,
        DeathByFreeze,
        DeathByDefault,
        Fire
    }
}
