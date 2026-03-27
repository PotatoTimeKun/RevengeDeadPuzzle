using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HUDView : MonoBehaviour, ITickable
{
    public static HUDView Instance { get; private set; }
    private void Awake() {
        Instance = this;
        MenuPanel.SetActive(false);
        InputHandler.Instance.SetInputState(InputState.Player);
    }
    private int _lastFrame = -1; // 連続呼び出しを防ぐ
    public void CloseMenuPanel(){
        if(isSettingViewOpen || !MenuPanel.activeSelf || Time.frameCount == _lastFrame) return;
        _lastFrame = Time.frameCount;
        MenuPanel.SetActive(false);
        InputHandler.Instance.SetInputState(InputState.Player);
        GameUseCase.Instance.ResumeGame();
    }
    public void OpenMenuPanel(){
        if(isSettingViewOpen || MenuPanel.activeSelf || Time.frameCount == _lastFrame) return;
        _lastFrame = Time.frameCount;
        MenuPanel.SetActive(true);
        InputHandler.Instance.SetInputState(InputState.Menu);
        GameUseCase.Instance.PauseGame();
    }
    private bool isSettingViewOpen = false;
    public void OpenSettingView() {
        if(isSettingViewOpen) return;
        SettingView.OpenScene();
        isSettingViewOpen = true;
        SettingView.OnClose += CloseSettingView;
        GetComponentInChildren<UIListNavigator>().IsActive = false;
    }
    private void CloseSettingView() {
        isSettingViewOpen = false;
        SettingView.OnClose -= CloseSettingView;
        GetComponentInChildren<UIListNavigator>().IsActive = true;
    }
    public void OpenStageSelectView() {
        if(isSettingViewOpen) return;
        StageSelectView.OpenScene();
    }
    void OnDestroy(){
        InputHandler.Instance.Player.Menu -= OpenMenuPanel;
        InputHandler.Instance.Menu.Cancel -= CloseMenuPanel;
        GameLoop.Instance.Unregister(this);
    }
    public GameObject MenuPanel;
    public Slider MentalSlider;
    public Text DeadCountText;
    public Text TimerText;
    public Text StageNameText;
    public GameObject TimeEvaluation;
    public GameObject CountEvaluation;
    public GameObject TypeEvaluation;
    public Text TimeEvaluationText;
    public Text CountEvaluationText;
    public Text TypeEvaluationText;
    public GameObject KeybordGuide;
    public GameObject GamepadGuide;
    public Animator ClearTextAnimator;
    private void UpdateMental(float value)
    {
        MentalSlider.value = value;
    }

    private void UpdateDeadCount(int count)
    {
        DeadCountText.text = $"DEAD : {count.ToString()}";
    }

    private void UpdateEvaluation(bool timeEval, bool countEval, bool typeEval)
    {
        TimeEvaluation.SetActive(timeEval);
        CountEvaluation.SetActive(countEval);
        TypeEvaluation.SetActive(typeEval);
    }

    private void UpdateEvaluationText()
    {
        int minute = (int)(GameUseCase.Instance.Stage.TimerSecondTarget / 60);
        int second = (int)(GameUseCase.Instance.Stage.TimerSecondTarget % 60);
        TimeEvaluationText.text = $"{minute:00}分{second:00}秒以内";
        CountEvaluationText.text = $"死亡{GameUseCase.Instance.Stage.DeathCountTarget.ToString()}回以内";
        TypeEvaluationText.text = GameUseCase.Instance.Stage.DeathTypeTargetExplanation;
    }

    private void UpdateTimer(int minute, int second)
    {
        TimerText.text = $"{minute:00}:{second:00}";
    }

    private void UpdateStageName(string name)
    {
        StageNameText.text = name;
    }

    private void UpdateGuide()
    {
        if (InputHandler.Instance.IsGamepad)
        {
            KeybordGuide.SetActive(false);
            GamepadGuide.SetActive(true);
        }
        else
        {
            KeybordGuide.SetActive(true);
            GamepadGuide.SetActive(false);
        }
    }

    private void ShowClearEffect(){
        ClearTextAnimator.SetBool("Goal", true);
    }

    void Start()
    {
        MenuPanel.SetActive(false);
        InputHandler.Instance.Player.Menu += OpenMenuPanel;
        InputHandler.Instance.Menu.Cancel += CloseMenuPanel;
        UpdateStageName(GameUseCase.Instance.Stage.DisplayName);
        UpdateEvaluationText();
        GameLoop.Instance.Register(this);
    }

    public void Tick(float deltaTime){
        // ゲームの状態を監視してHUDに反映
        float mental = GameUseCase.Instance.Mental.CurrentValue / GameUseCase.Instance.Mental.MaxValue;
        UpdateMental(mental);
        UpdateDeadCount(GameUseCase.Instance.Score.DeathCount);
        List<bool> evaluations = GameUseCase.Instance.Score.CheckEvaluation();
        UpdateEvaluation(evaluations[0], evaluations[1], evaluations[2]);
        int minute = (int)(GameUseCase.Instance.Score.CurrentTime / 60);
        int second = (int)(GameUseCase.Instance.Score.CurrentTime % 60);
        UpdateTimer(minute, second);
        UpdateGuide();
        if(GameUseCase.Instance.PlayerController.PlayerLogic.State == Entity_Data.PlayerState.Goal){
            ShowClearEffect();
        }
    }
}
