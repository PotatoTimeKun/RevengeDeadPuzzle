using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class ResultView : MonoBehaviour
{
    public static void OpenScene() {
        SceneManager.LoadScene("Result");
    }
    public void RetryButton(){
        SceneManager.LoadScene(GameUseCase.BeforeStage.Scene);
    }
    public void BackButton(){
        SceneManager.LoadScene("StageSelect");
    }
    public Text TimeText;
    public Text CountText;
    public Text TypeText;
    public GameObject TimeEvaluation;
    public GameObject CountEvaluation;
    public GameObject TypeEvaluation;
    public Text DeathHistoryText;
    public GameObject ClearCamera;
    public GameObject GameOverCamera;
    public GameObject ClearPanel;
    public GameObject GameOverPanel;
    public void Start() {
        if (ScoreLogic.BeforeScore.IsClear) {
            ClearCamera.SetActive(true);
            GameOverCamera.SetActive(false);
            ClearPanel.SetActive(true);
            GameOverPanel.SetActive(false);
        } else {
            ClearCamera.SetActive(false);
            GameOverCamera.SetActive(true);
            ClearPanel.SetActive(false);
            GameOverPanel.SetActive(true);
        }
        TimeText.text = GameUseCase.BeforeStage.TimerTargetToString();
        CountText.text = GameUseCase.BeforeStage.DeathCountTargetToString();
        TypeText.text = GameUseCase.BeforeStage.AcceptedDeathTypeTargetToString();
        TimeEvaluation.SetActive(ScoreLogic.BeforeScore.TimeTarget);
        CountEvaluation.SetActive(ScoreLogic.BeforeScore.CountTarget);
        TypeEvaluation.SetActive(ScoreLogic.BeforeScore.TypeTarget);
        DeathHistoryText.text = DeathTypeHistoryToString();
    }
    private Dictionary<Entity_Data.DeathType, string> DeathTypeNames = new();
    private void Awake() {
        DeathTypeNames.Add(Entity_Data.DeathType.Burned, "焼死");
        DeathTypeNames.Add(Entity_Data.DeathType.Crushed, "圧死");
        DeathTypeNames.Add(Entity_Data.DeathType.Frozen, "凍死");
        DeathTypeNames.Add(Entity_Data.DeathType.Dismembered, "切断");
        DeathTypeNames.Add(Entity_Data.DeathType.None, "他");
        InputHandler.Instance.SetInputState(InputState.Menu);
        InputHandler.Instance.Menu.Cancel += RetryButton;
        InputHandler.Instance.Menu.Submit += BackButton;
    }
    private void OnDestroy() {
        InputHandler.Instance.Menu.Cancel -= RetryButton;
        InputHandler.Instance.Menu.Submit -= BackButton;
    }
    private string DeathTypeHistoryToString() {
        string history = "";
        Dictionary<Entity_Data.DeathType, int> deathTypeCount = new();
        foreach (var deathType in ScoreLogic.BeforeScore.DeathTypeHistory) {
            if (!deathTypeCount.ContainsKey(deathType)) {
                deathTypeCount.Add(deathType, 0);
            }
            deathTypeCount[deathType]++;
        }
        foreach (var deathType in deathTypeCount) {
            history += DeathTypeNames[deathType.Key] + "... " + deathType.Value+ "回\n";
        }
        history = history.TrimEnd('\n');
        return history;
    }
}
