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
        SpawnPlayer();
    }

    public void PauseGame(){
        Score.StopTimer();
    }

    public void ResumeGame(){
        Score.ResumeTimer();
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
