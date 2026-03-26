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

    public static StageDef BeforeStage; // 前のステージをクラス変数に保存

    public PlayerController PlayerController;
    public StageDef Stage;
    public GameObject PlayerPrefab;
    [SerializeField] private GameObject _startPos;

    [HideInInspector] public MentalLogic Mental;
    [HideInInspector] public ScoreLogic Score;
    [Header("UI設定")]
    [SerializeField] private GameObject _gameOverPrefab; 
    [SerializeField] private Transform _uiParent; 
    private bool _isGameOver = false;
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
        // プレイヤー生成
        GameObject playerObj = Instantiate(PlayerPrefab);
        PlayerController = playerObj.GetComponent<PlayerController>();
        // 位置設定
        playerObj.transform.position = _startPos.transform.position;
        // コスチューム解放
        string costumeId = CostumeCollector.Instance.UnlockRandomId();
        PlayerController.PlayerLogic.CostumeId = costumeId;
    }

    public void StartGame(){
        Time.timeScale = 1f;
        SpawnPlayer();
    }

    public void PauseGame(){
        // タイマー、物理エンジン等を停止
        Score.StopTimer();
        Time.timeScale = 0f;
    }

    public void ResumeGame(){
        // タイマー、物理エンジン等を再開
        Score.ResumeTimer();
        Time.timeScale = 1f;
    }

    public void OnPlayerDead(Entity_Data.DeathType deathType){ // 
       if (Mental != null && Mental.CurrentValue > 0)
        {
            SpawnPlayer();
        }
        else
        {
            Debug.Log("Mental is 0. Skip SpawnPlayer.");
        }
}
    

    private bool _goalFlag = false;
    private float _goalWaitTime = 2.0f;
    public void OnGoal(){
        // ゴール演出を待機
        if (_goalFlag) return;
        _goalFlag = true;
        Score.StopTimer();
        PlayerController.PlayerLogic.State = Entity_Data.PlayerState.Goal;
        // 評価を保存
        List<bool> evaluations = Score.CheckEvaluation();
        StageSelecter.Instance.ClearStage(Stage.Id,evaluations[0],evaluations[1],evaluations[2]);
    }

    public void Tick(float deltaTime){
        
        // 死んだときに処理を実行
        if (!_isGameOver && Mental != null && Mental.CurrentValue <= 0)
        {
            PerformGameOver();
            return;
        }

        // 死んだときにリスポーン）
        if (PlayerController != null && PlayerController.PlayerLogic.State == Entity_Data.PlayerState.Dead) {
            OnPlayerDead(PlayerController.PlayerLogic.Type);
            return;
        }

        // ゴールしたときに処理を実行
        if (_goalFlag) {
            _goalWaitTime -= deltaTime;
            if (_goalWaitTime <= 0) {
                ResultView.OpenScene(); // リザルト画面へ
            }
        }
    }
    private void PerformGameOver()
{
    if (_isGameOver) return;
    _isGameOver = true;

    GameObject obj = Instantiate(_gameOverPrefab, _uiParent);

    InputHandler.Instance.SetInputState(InputState.Menu);

    Score.StopTimer();

    if (PlayerController != null) {
        PlayerController.enabled = false; 
    }
}
    public void RestartGame()
    {
        Time.timeScale = 1.0f;
        string currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        UnityEngine.SceneManagement.SceneManager.LoadScene(currentSceneName);

    }
}
