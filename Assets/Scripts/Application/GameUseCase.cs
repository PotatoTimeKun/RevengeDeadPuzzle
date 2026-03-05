using UnityEngine;
using Unity.Cinemachine;

public class GameUseCase : MonoBehaviour , ITickable
{
    private PlayerController _playerController;
    private StageDef _stageDef;
    private CinemachineCamera _cinemachineCamera;
    private CinemachineFollow _cinemachineFollow;
    [SerializeField] private GameObject _startPos;
    private GameObject _playerPrefab; 
    public CostumeRegistry CostumeRegistry;
    void Start(){
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
