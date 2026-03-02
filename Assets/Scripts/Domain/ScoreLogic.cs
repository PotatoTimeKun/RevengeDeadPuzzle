// UnityEngine禁止
using System.Collections.Generic;

public class ScoreLogic
{
    private float _currentTime;
    public float CurrentTime{get {return _currentTime;}}
    public int DeathCount;
    public List<Entity_Data.DeathType> DeathTypeHistory = new List<Entity_Data.DeathType>();
    public List<bool> CheckEvaluation(){return new List<bool>();}
}
