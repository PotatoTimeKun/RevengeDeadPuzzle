using UnityEngine;
using Unity.Cinemachine;

public class GameUseCase : MonoBehaviour , ITickable
{
    public static GameUseCase Instance { get; private set; }
    private void Awake() {
        Instance = this;
    }
    private PlayerController _playerController;
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
    }

    private void SpawnPlayer(){
        GameObject playerObj = Instantiate(PlayerPrefab);
        _playerController = playerObj.GetComponent<PlayerController>();
        _playerController.Initialize(this);
        playerObj.transform.position = _startPos.transform.position;
        string costumeId = CostumeCollector.Instance.UnlockRandomId();
        playerObj.GetComponent<PlayerView>().SetCostume(costumeId);
    }

    public void StartGame(){
        InputHandler.Instance.SetInputState(InputState.Player);
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

    public void OnGoal(){}

    public void Tick(float deltaTime){
        // 死んだときに処理を実行
        if (_playerController.PlayerLogic.State != Entity_Data.PlayerState.Dead) return;
        OnPlayerDead(_playerController.PlayerLogic.Type);
    }
}
