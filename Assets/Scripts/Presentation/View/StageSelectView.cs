using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

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

    private static int _beforeMaxUnlockedCount = -1;
    public GameObject UnlockEffect;

    private void Start() {
        PutStageObject();
        // 入力設定
        InputHandler.Instance.SetInputState(InputState.Menu);
        InputHandler.Instance.Menu.Move += Move;
        InputHandler.Instance.Menu.Submit += PlayButton;
        InputHandler.Instance.Menu.Cancel += BackButton;
        GameLoop.Instance.Register(this);
        // カメラ初期位置設定
        Camera.main.transform.position = new Vector3(SelectedIndex * _objectDistance, Camera.main.transform.position.y, Camera.main.transform.position.z);
        if (_beforeMaxUnlockedCount == -1) { // 初回起動時
            _beforeMaxUnlockedCount = StageSelecter.Instance.UnlockedStageList.Count;
            SelectedIndex = _beforeMaxUnlockedCount - 1;
            if (SelectedIndex < 0) SelectedIndex = 0;
            Camera.main.transform.position = new Vector3(SelectedIndex * _objectDistance, Camera.main.transform.position.y, Camera.main.transform.position.z);
        } else if (_beforeMaxUnlockedCount < StageSelecter.Instance.UnlockedStageList.Count) { // 新ステージ解放時
            _beforeMaxUnlockedCount = StageSelecter.Instance.UnlockedStageList.Count;
            SelectedIndex = _beforeMaxUnlockedCount - 1;
            Instantiate(UnlockEffect, _stageObjectList[SelectedIndex].transform.position, Quaternion.identity);
        }
        if (AudioController.Instance.GetCurrentBGMType() != Audio_Data.BGMType.Title) {
            AudioController.Instance.StopBGM();
            AudioController.Instance.PlayBGM(Audio_Data.BGMType.Title);
        }
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
        // ステージオブジェクトの回転
        GameObject stageObject = _stageObjectList[SelectedIndex];
        stageObject.transform.Rotate(0, 30 * deltaTime, 0);
    }

    private float _objectDistance = 5f;
    private List<GameObject> _stageObjectList = new();
    private void PutStageObject(){ // ステージオブジェクトの配置
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
            stageObject.transform.Rotate(0, 180, 0);
            _stageObjectList.Add(stageObject);
        }
    }

    private void Move(Vector2 inputVector){
        if (inputVector.x > 0) {
            SelectedIndex++;
        } else if (inputVector.x < 0) {
            SelectedIndex--;
        }
        AudioController.Instance.PlaySE(Audio_Data.SEType.Button);
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
            AudioController.Instance.PlaySE(Audio_Data.SEType.Button);
        }
    }

    private void BackButton(){
        SceneManager.LoadScene("Title");
        AudioController.Instance.PlaySE(Audio_Data.SEType.Button);
    }
}
