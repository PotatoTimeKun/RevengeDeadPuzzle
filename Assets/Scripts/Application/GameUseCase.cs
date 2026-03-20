using UnityEngine;
using Unity.Cinemachine;
using System.Collections.Generic;

public class GameUseCase : MonoBehaviour , ITickable
{
    public static GameUseCase Instance { get; private set; }
    private void Awake() {
        Instance = this;
        BeforeStage = Stage;
    }
    public static StageDef BeforeStage;
    public PlayerController PlayerController;
    public StageDef Stage;
    private CinemachineCamera _cinemachineCamera;
    private CinemachineFollow _cinemachineFollow;
    [SerializeField] private GameObject _startPos;
    public GameObject PlayerPrefab; 
    public CostumeRegistry CostumeRegistry;
    [HideInInspector] public MentalLogic Mental;
    [HideInInspector] public ScoreLogic Score;
    void Start(){
        Mental = new MentalLogic(Stage.MaxMental);
        Score = new ScoreLogic(Stage);
        StartGame();
        GameLoop.Instance.Register(this);
    }
    void OnDestroy(){
        GameLoop.Instance.Unregister(this);
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void SpawnPlayer(){
        GameObject playerObj = Instantiate(PlayerPrefab);
        PlayerController = playerObj.GetComponent<PlayerController>();
        PlayerController.Initialize(this);
        playerObj.transform.position = _startPos.transform.position;
        string costumeId = CostumeCollector.Instance.UnlockRandomId();
        playerObj.GetComponent<PlayerView>().SetCostume(costumeId);
    }

    public void StartGame(){
        Debug.Log("GameUseCase StartGame");
        InputHandler.Instance.SetInputState(InputState.Player);
        Time.timeScale = 1f;
        SpawnPlayer();
    }

    public void PauseGame(){
        InputHandler.Instance.SetInputState(InputState.Menu);
        // タイマー、物理エンジン等を停止
        Score.StopTimer();
        Time.timeScale = 0f;
    }

    public void ResumeGame(){
        InputHandler.Instance.SetInputState(InputState.Player);
        // タイマー、物理エンジン等を再開
        Score.ResumeTimer();
        Time.timeScale = 1f;
    }

    public void OnPlayerDead(Entity_Data.DeathType deathType){ // 
        SpawnPlayer();
    }

    private bool _goalFlag = false;
    private float _goalWaitTime = 2.0f;
    public void OnGoal(){
        // ゴール演出を待機
        if (_goalFlag) return;
        _goalFlag = true;
        Score.StopTimer();
        PlayerController.PlayerLogic.State = Entity_Data.PlayerState.Goal;
        List<bool> evaluations = Score.CheckEvaluation();
        StageSelecter.Instance.ClearStage(Stage.Id,evaluations[0],evaluations[1],evaluations[2]);
    }

    public void Tick(float deltaTime){
        // 死んだときに処理を実行
        if (PlayerController.PlayerLogic.State == Entity_Data.PlayerState.Dead) {
            OnPlayerDead(PlayerController.PlayerLogic.Type);
            return;
        }

        // ゴールしたときに処理を実行
        if (_goalFlag) {
            _goalWaitTime -= deltaTime;
            if (_goalWaitTime <= 0) {
                ResultView.OpenScene();
            }
        }
    }
}
