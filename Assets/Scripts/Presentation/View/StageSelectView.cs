using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageSelectView : MonoBehaviour, ITickable
{
    public static StageSelectView Instance { get; private set; }
    private void Awake() {
        Instance = this;
    }

    public static void OpenScene() {
        SceneManager.LoadScene("StageSelect");
    }

    public GameObject ClearedPrefab;
    public GameObject NotClearedPrefab;
    public GameObject LockedPrefab;
    public GameObject TimerIconActive;
    public GameObject CountIconActive;
    public GameObject TypeIconActive;
    public Text TimerText;
    public Text CountText;
    public Text TypeText;
    public Text StageNameText;
    public Text PlayButtonText;
    public GameObject RightArrow;
    public GameObject LeftArrow;

    private static int _selectedIndex = 0;
    private static int SelectedIndex{
        get{
            return _selectedIndex;
        }
        set{
            if (value < 0) value = 0;
            if (value >= StageSelecter.Instance.StageRegistry.AllStages.Count) value = StageSelecter.Instance.StageRegistry.AllStages.Count - 1;
            _selectedIndex = value;
        }
    }

    private void Start() {
        PutStageObject();
        InputHandler.Instance.SetInputState(InputState.Menu);
        InputHandler.Instance.Menu.Move += Move;
        InputHandler.Instance.Menu.Submit += PlayButton;
        InputHandler.Instance.Menu.Cancel += BackButton;
        GameLoop.Instance.Register(this);
        UpdateUI();
    }

    private void OnDestroy() {
        InputHandler.Instance.Menu.Move -= Move;
        InputHandler.Instance.Menu.Submit -= PlayButton;
        InputHandler.Instance.Menu.Cancel -= BackButton;
        GameLoop.Instance.Unregister(this);
    }

    public void Tick(float deltaTime){
        MoveCamera(_selectedIndex, deltaTime);
    }

    private float _objectDistance = 5f;
    private void PutStageObject(){
        var stageList = StageSelecter.Instance.StageRegistry.AllStages;
        for (int i = 0; i < stageList.Count; i++) {
            var stage = stageList[i];
            GameObject stageObject;
            var scoreData = StageSelecter.Instance.ScoreDataList.Find(x => x.StageId == stage.Id);
            if (StageSelecter.Instance.UnlockedStageList.Contains(stage.Id) && scoreData != null && scoreData.IsClear) {
                stageObject = Instantiate(ClearedPrefab, transform);
            } else if (StageSelecter.Instance.UnlockedStageList.Contains(stage.Id)) {
                stageObject = Instantiate(NotClearedPrefab, transform);
            } else {
                stageObject = Instantiate(LockedPrefab, transform);
            }
            stageObject.transform.position = new Vector3(i * _objectDistance, 0, 0);
        }
    }

    private void Move(Vector2 inputVector){
        if (inputVector.x > 0) {
            SelectedIndex++;
        } else if (inputVector.x < 0) {
            SelectedIndex--;
        }
        UpdateUI();
    }

    private float _moveSpeed = 5f;
    private void MoveCamera(int index, float deltaTime){
        Vector3 currentPosition = Camera.main.transform.position;
        Vector3 targetPosition = new Vector3(index * _objectDistance, currentPosition.y, currentPosition.z);
        Camera.main.transform.position = Vector3.Lerp(currentPosition, targetPosition, _moveSpeed * deltaTime);
    }

    private void UpdateUI(){
        var stage = StageSelecter.Instance.StageRegistry.AllStages[SelectedIndex];
        StageNameText.text = stage.DisplayName;
        TimerText.text = stage.TimerTargetToString();
        CountText.text = stage.DeathCountTargetToString();
        TypeText.text = stage.AcceptedDeathTypeTargetToString();
        TimerIconActive.SetActive(false);
        CountIconActive.SetActive(false);
        TypeIconActive.SetActive(false);
        var scoreData = StageSelecter.Instance.ScoreDataList.Find(x => x.StageId == stage.Id);
        if (StageSelecter.Instance.UnlockedStageList.Contains(stage.Id) && scoreData != null && scoreData.IsClear) {
            TimerIconActive.SetActive(scoreData.TimeTarget);
            CountIconActive.SetActive(scoreData.CountTarget);
            TypeIconActive.SetActive(scoreData.TypeTarget);
        }
        RightArrow.SetActive(SelectedIndex < StageSelecter.Instance.StageRegistry.AllStages.Count - 1);
        LeftArrow.SetActive(SelectedIndex > 0);
        PlayButtonText.text = StageSelecter.Instance.UnlockedStageList.Contains(stage.Id) ? "プレイ" : "未解放";
    }

    private void PlayButton(){
        if (StageSelecter.Instance.UnlockedStageList.Contains(StageSelecter.Instance.StageRegistry.AllStages[SelectedIndex].Id)) {
            SceneManager.LoadScene(StageSelecter.Instance.StageRegistry.AllStages[SelectedIndex].Scene);
        }
    }

    private void BackButton(){
        SceneManager.LoadScene("Title");
    }
}
