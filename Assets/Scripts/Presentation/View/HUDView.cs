using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HUDView : MonoBehaviour
{
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
    public void UpdateMental(float value)
    {
        MentalSlider.value = value;
    }

    public void UpdateDeadCount(int count)
    {
        DeadCountText.text = $"DEAD : {count.ToString()}";
    }

    public void UpdateEvaluation(bool timeEval, bool countEval, bool typeEval)
    {
        TimeEvaluation.SetActive(timeEval);
        CountEvaluation.SetActive(countEval);
        TypeEvaluation.SetActive(typeEval);
    }

    public void UpdateEvaluationText()
    {
        int minute = (int)(GameUseCase.Instance.Stage.TimerSecondTarget / 60);
        int second = (int)(GameUseCase.Instance.Stage.TimerSecondTarget % 60);
        TimeEvaluationText.text = $"{minute:00}分{second:00}秒以内";
        CountEvaluationText.text = $"死亡{GameUseCase.Instance.Stage.DeathCountTarget.ToString()}回以内";
        TypeEvaluationText.text = GameUseCase.Instance.Stage.DeathTypeTargetExplanation;
    }

    public void UpdateTimer(int minute, int second)
    {
        TimerText.text = $"{minute:00}:{second:00}";
    }

    public void UpdateStageName(string name)
    {
        StageNameText.text = name;
    }

    public void UpdateGuide()
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

    void Start()
    {
        UpdateStageName(GameUseCase.Instance.Stage.DisplayName);
        UpdateEvaluationText();
    }

    void Update()
    {
        float mental = GameUseCase.Instance.Mental.CurrentValue / GameUseCase.Instance.Mental.MaxValue;
        UpdateMental(mental);
        UpdateDeadCount(GameUseCase.Instance.Score.DeathCount);
        List<bool> evaluations = GameUseCase.Instance.Score.CheckEvaluation();
        UpdateEvaluation(evaluations[0], evaluations[1], evaluations[2]);
        int minute = (int)(GameUseCase.Instance.Score.CurrentTime / 60);
        int second = (int)(GameUseCase.Instance.Score.CurrentTime % 60);
        UpdateTimer(minute, second);
        UpdateGuide();
    }
}
