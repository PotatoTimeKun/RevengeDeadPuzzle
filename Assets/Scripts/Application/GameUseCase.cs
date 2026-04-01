using UnityEngine;
using Unity.Cinemachine;
using System.Collections.Generic;
using System;

public class GameUseCase : MonoBehaviour , ITickable
{
    public static GameUseCase Instance { get; private set; }
    private void Awake() {
        Instance = this;
        BeforeStage = Stage;
        Mental = new MentalLogic(Stage.MaxMental);
        Score = new ScoreLogic(Stage);
    }

    public static StageDef BeforeStage; // 前のステージをクラス変数に保存

    [HideInInspector]public PlayerController PlayerController;
    public StageDef Stage;
    public GameObject PlayerPrefab;
    [SerializeField] private GameObject _startPos;

    [HideInInspector] public MentalLogic Mental;
    [HideInInspector] public ScoreLogic Score;
    private bool _isGameOver = false;
    public bool IsGameOver { 
        get { return _isGameOver; } 
        private set { _isGameOver = value; OnGameOver?.Invoke(); } 
    }
    private bool _isGameClear = false;
    public bool IsGameClear { 
        get { return _isGameClear; } 
        private set { _isGameClear = value; OnGameClear?.Invoke(); } 
    }
    public Action OnGameOver;
    public Action OnGameClear;
    void Start(){
        StartGame();
        GameLoop.Instance.Register(this);
        Mental.OnMentalChange += CheckGameOver;
    }
    void OnDestroy(){
        GameLoop.Instance.Unregister(this);
        PlayerController.PlayerLogic.OnDead -= OnPlayerDead;
        Mental.OnMentalChange -= CheckGameOver;
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void SpawnPlayer(){
        // プレイヤー生成
        GameObject playerObj = Instantiate(PlayerPrefab);
        PlayerController = playerObj.GetComponent<PlayerController>();
        PlayerController.PlayerLogic.OnDead += OnPlayerDead;
        // 位置設定
        playerObj.transform.position = _startPos.transform.position;
    }

    private void StartGame(){
        Time.timeScale = 1f;
        SpawnPlayer();
    }

    public Action OnPause;
    public void PauseGame(){
        // タイマー、物理エンジン等を停止
        Time.timeScale = 0f;
        OnPause?.Invoke();
    }

    public Action OnResume;
    public void ResumeGame(){
        // タイマー、物理エンジン等を再開
        Time.timeScale = 1f;
        OnResume?.Invoke();
    }

    private void OnPlayerDead(){
        PlayerController.PlayerLogic.OnDead -= OnPlayerDead;
        if (IsGameOver) ResultView.OpenScene();
        else SpawnPlayer();
    }

    private float _goalWaitTime = 2.0f;
    public void OnGoal(){
        // ゴール演出を待機
        if (IsGameClear) return;
        IsGameClear = true;
        PlayerController.PlayerLogic.State = Entity_Data.PlayerState.Goal;
    }

    public void Tick(float deltaTime){
        // ゴールしたときに処理を実行
        if (IsGameClear) {
            _goalWaitTime -= deltaTime;
            if (_goalWaitTime <= 0) {
                ResultView.OpenScene(); // リザルト画面へ
            }
        }
    }

    private void CheckGameOver()
    {
        if (IsGameOver || Mental.CurrentValue > 0) return;
        IsGameOver = true;
    }
}
