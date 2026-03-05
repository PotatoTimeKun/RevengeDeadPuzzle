// UnityEngine禁止
using System.Collections.Generic;

public class ScoreLogic : ITickable
{
    private float _currentTime;
    private StageDef _currentStage;
    private bool _isTimerEnabled;
    public float CurrentTime { get { return _currentTime; } }
    public int DeathCount;
    public List<Entity_Data.DeathType> DeathTypeHistory = new();

    public ScoreLogic(StageDef stage)
    {
        _currentStage = stage;
        _currentTime = 0f;
        _isTimerEnabled = true;
        DeathCount = 0;
        GameLoop.Instance.Register(this);
    }

    public void Tick(float deltaTime)
    {
        if (!_isTimerEnabled) return;
        _currentTime += deltaTime;
    }

    public void StopTimer()
    {
        _isTimerEnabled = false;
    }

    public void ResumeTimer()
    {
        _isTimerEnabled = true;
    }

    public void AddDeath(Entity_Data.DeathType deathType = Entity_Data.DeathType.None)
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
