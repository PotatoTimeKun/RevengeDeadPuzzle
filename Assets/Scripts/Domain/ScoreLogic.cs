// UnityEngine禁止
using System.Collections.Generic;

public class ScoreLogic : ITickable
{
    private float _currentTime;
    private StageDef _currentStage;
    public float CurrentTime { get { return _currentTime; } }
    public int DeathCount;
    public List<Entity_Data.DeathType> DeathTypeHistory = new();

    public ScoreLogic(StageDef stage)
    {
        _currentStage = stage;
        _currentTime = 0f;
        DeathCount = 0;
    }

    public void Tick(float deltaTime)
    {
        _currentTime += deltaTime;
    }

    public void AddDeathCount(Entity_Data.DeathType deathType = Entity_Data.DeathType.None)
    {
        DeathCount++;
        DeathTypeHistory.Add(deathType);
    }

    public List<bool> CheckEvaluation()
    {
        List<bool> evaluations = new();

        evaluations.Add(CurrentTime <= _currentStage.TimerSecondTarget);
        evaluations.Add(DeathCount <= _currentStage.DeathCountTarget);
        evaluations.Add(DeathTypeHistory.TrueForAll(deathType => _currentStage.AcceptedDeathTypeTarget.Contains(deathType)));

        return evaluations;
    }
}
