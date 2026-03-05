using UnityEngine;
using Unity.Cinemachine;

public class GameUseCase : MonoBehaviour , ITickable
{
    public static GameUseCase Instance { get; private set; }
    private void Awake() {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    private PlayerController _playerController;
    public StageDef Stage;
    private CinemachineCamera _cinemachineCamera;
    private CinemachineFollow _cinemachineFollow;
    [SerializeField] private GameObject _startPos;
    private GameObject _playerPrefab; 
    public CostumeRegistry CostumeRegistry;
    [HideInInspector] public MentalLogic Mental;
    [HideInInspector] public ScoreLogic Score;
    void Start(){
        Mental = new MentalLogic(Stage.MaxMental);
        Score = new ScoreLogic(Stage);
        _playerPrefab = CostumeRegistry.GetById("Default");
        StartGame();
        GameLoop.Instance.Register(this);
    }
    void OnDestroy(){
        GameLoop.Instance.Unregister(this);
    }
    public void StartGame(){
        GameObject PlayerObj = Instantiate(_playerPrefab);
        _playerController = PlayerObj.GetComponent<PlayerController>();
        PlayerObj.transform.position = _startPos.transform.position;
    }
    public void PauseGame(){
        Score.StopTimer();
    }
    public void ResumeGame(){
        Score.ResumeTimer();
    }
    public void OnPlayerDead(Entity_Data.DeathType deathType){ // 
        GameObject PlayerObj = Instantiate(_playerPrefab);
        _playerController = PlayerObj.GetComponent<PlayerController>();
        PlayerObj.transform.position = _startPos.transform.position;
    }
    public void OnGoal(){}
    public void Tick(float deltaTime){
        // 死んだときに処理を実行
        if (_playerController.PlayerLogic.State != Entity_Data.PlayerState.Dead) return;
        OnPlayerDead(_playerController.PlayerLogic.Type);
    }
}
