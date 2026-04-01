// UnityEngine禁止
using System.Collections.Generic;
using System;

public class ScoreLogic : ITickable
{
    public static ScoreLogic BeforeScore; // 前のステージのスコア
    private float _currentTime;
    private StageDef _currentStage;
    private bool _isTimerEnabled;
    public float CurrentTime { get { return _currentTime; } }
    public int DeathCount { get; private set; }
    public List<Entity_Data.DeathType> DeathTypeHistory = new();
    public bool IsClear = true;

    public Action OnTimerChange;
    public Action OnDeathCountChange;
    public Action OnEvaluationChange;

    public ScoreLogic(StageDef stage)
    {
        BeforeScore = this;
        _currentStage = stage;
        _currentTime = 0f;
        _isTimerEnabled = true;
        DeathCount = 0;
        GameUseCase.Instance.OnGameClear += OnClear;
        GameUseCase.Instance.OnPause += StopTimer;
        GameUseCase.Instance.OnResume += ResumeTimer;
        GameUseCase.Instance.OnGameOver += OnGameOver;
        GameLoop.Instance.Register(this);
    }
    ~ScoreLogic(){
        GameUseCase.Instance.OnGameClear -= OnClear;
        GameUseCase.Instance.OnPause -= StopTimer;
        GameUseCase.Instance.OnResume -= ResumeTimer;
        GameUseCase.Instance.OnGameOver -= OnGameOver;
        GameLoop.Instance.Unregister(this);
    }

    public void Tick(float deltaTime)
    {
        if (!_isTimerEnabled) return;
        int oldTimerInt = (int)_currentTime;
        _currentTime += deltaTime;
        if (oldTimerInt != (int)_currentTime) {
            OnTimerChange?.Invoke();
            OnEvaluationChange?.Invoke();
        }
    }

    private void StopTimer()
    {
        _isTimerEnabled = false;
    }

    private void ResumeTimer()
    {
        _isTimerEnabled = true;
    }

    public void AddDeath(Entity_Data.DeathType deathType = Entity_Data.DeathType.None)
    {
        DeathCount++;
        DeathTypeHistory.Add(deathType);
        OnDeathCountChange?.Invoke();
        OnEvaluationChange?.Invoke();
    }

    public bool TimeTarget { get { return CurrentTime <= _currentStage.TimerSecondTarget; } }
    public bool CountTarget { get { return DeathCount <= _currentStage.DeathCountTarget; } }
    public bool TypeTarget { get { return DeathTypeHistory.TrueForAll(deathType => _currentStage.AcceptedDeathTypeTarget.Contains(deathType)); } }

    private void OnClear(){
        StopTimer();
        // 評価を保存
        StageSelecter.Instance.ClearStage(_currentStage.Id,TimeTarget,CountTarget,TypeTarget);
    }

    private void OnGameOver(){
        StopTimer();
    }
}
