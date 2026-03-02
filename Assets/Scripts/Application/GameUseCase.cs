using UnityEngine;
using Unity.Cinemachine;

public class GameUseCase : MonoBehaviour , ITickable
{
    private PlayerController _playerController;
    private StageDef _stageDef;
    [SerializeField] private GameObject _startPos;
    [SerializeField] private CinemachineCamera _camera;
    public GameObject DEBUGPlayerPrefab; // 仮置き、実際にはCostumeRedistoryから持ってこれるようにする
    void Start(){
        StartGame();
        GameLoop.Instance.Register(this);
    }
    void OnDestroy(){
        GameLoop.Instance.Unregister(this);
    }
    public void StartGame(){
        GameObject PlayerObj = Instantiate(DEBUGPlayerPrefab);
        _playerController = PlayerObj.GetComponent<PlayerController>();
        _playerController.vcam = _camera;
        PlayerObj.transform.position = _startPos.transform.position;
    }
    public void OnPlayerDead(Entity_Data.DeathType deathType){
        GameObject PlayerObj = Instantiate(DEBUGPlayerPrefab);
        _playerController = PlayerObj.GetComponent<PlayerController>();
        _playerController.vcam = _camera;
        PlayerObj.transform.position = _startPos.transform.position;
    }
    public void OnGoal(){}
    public void Tick(float deltaTime){
        // 死んだときに処理を実行
        if (_playerController.PlayerLogic.State != Entity_Data.PlayerState.Dead) return;
        OnPlayerDead(_playerController.PlayerLogic.Type);
    }
}
