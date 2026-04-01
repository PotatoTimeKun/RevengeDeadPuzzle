using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HUDView : MonoBehaviour
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
        
        if (GameUseCase.Instance != null && GameUseCase.Instance.Mental != null) {
            GameUseCase.Instance.Mental.OnMentalChange -= UpdateMental;
        }
        if (GameUseCase.Instance != null && GameUseCase.Instance.Score != null) {
            GameUseCase.Instance.Score.OnDeathCountChange -= UpdateDeadCount;
            GameUseCase.Instance.Score.OnTimerChange -= UpdateTimer;
            GameUseCase.Instance.Score.OnEvaluationChange -= UpdateEvaluation;
        }
        if (GameUseCase.Instance != null) {
            GameUseCase.Instance.OnGameClear -= ShowClearEffect;
        }
        if (InputHandler.Instance != null) {
            InputHandler.Instance.OnInputMethodChange -= UpdateGuide;
        }
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
    
    private void UpdateMental()
    {
        float value = GameUseCase.Instance.Mental.CurrentValue / GameUseCase.Instance.Mental.MaxValue;
        MentalSlider.value = value;
    }

    private void UpdateDeadCount()
    {
        int count = GameUseCase.Instance.Score.DeathCount;
        DeadCountText.text = $"DEAD : {count.ToString()}";
    }

    private void UpdateEvaluation()
    {
        bool timeEval = GameUseCase.Instance.Score.TimeTarget;
        bool countEval = GameUseCase.Instance.Score.CountTarget;
        bool typeEval = GameUseCase.Instance.Score.TypeTarget;
        TimeEvaluation.SetActive(timeEval);
        CountEvaluation.SetActive(countEval);
        TypeEvaluation.SetActive(typeEval);
    }

    private void UpdateEvaluationText()
    {
        TimeEvaluationText.text = GameUseCase.Instance.Stage.TimerTargetToString();
        CountEvaluationText.text = GameUseCase.Instance.Stage.DeathCountTargetToString();
        TypeEvaluationText.text = GameUseCase.Instance.Stage.DeathTypeTargetExplanation;
    }

    private void UpdateTimer()
    {
        int minute = (int)(GameUseCase.Instance.Score.CurrentTime / 60);
        int second = (int)(GameUseCase.Instance.Score.CurrentTime % 60);
        TimerText.text = $"{minute:00}:{second:00}";
    }

    private void UpdateStageName(string name)
    {
        StageNameText.text = name;
    }

    private void UpdateGuide(bool isGamepad)
    {
        if (isGamepad)
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

        UpdateMental();
        UpdateDeadCount();
        UpdateTimer();
        UpdateEvaluation();
        UpdateGuide(InputHandler.Instance.IsGamepad);

        GameUseCase.Instance.Mental.OnMentalChange += UpdateMental;
        GameUseCase.Instance.Score.OnDeathCountChange += UpdateDeadCount;
        GameUseCase.Instance.Score.OnTimerChange += UpdateTimer;
        GameUseCase.Instance.Score.OnEvaluationChange += UpdateEvaluation;
        InputHandler.Instance.OnInputMethodChange += UpdateGuide;
        GameUseCase.Instance.OnGameClear += ShowClearEffect;
    }
}
