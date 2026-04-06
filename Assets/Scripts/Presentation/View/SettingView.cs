using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingView : MonoBehaviour
{
    public static SettingView Instance { get; private set; }
    private void Awake() {
        Instance = this;
    }

    public static void OpenScene() {
        SceneManager.LoadScene("Setting", LoadSceneMode.Additive);
    }

    public void CloseScene() {
        OnClose?.Invoke();
        AudioController.Instance.PlaySE(Audio_Data.SEType.Button);
        SceneManager.UnloadSceneAsync(gameObject.scene);
    }
    public static System.Action OnClose;

    void Start() {
        InputHandler.Instance.Menu.Cancel += CloseScene;
        InputHandler.Instance.SetInputState(InputState.Menu);
        Init();
    }

    void OnDestroy() {
        InputHandler.Instance.Menu.Cancel -= CloseScene;
    }

    public UISlider bgmSlider;
    public UISlider seSlider;
    public UISlider masterSlider;

    void Init() {
        bgmSlider.SetValue(SettingDataController.CurrentData.BgmVolume);
        seSlider.SetValue(SettingDataController.CurrentData.SeVolume);
        masterSlider.SetValue(SettingDataController.CurrentData.MasterVolume);
        bgmSlider.OnValueChanged += (value) => SettingDataController.Instance.SetBgmVolume(value);
        seSlider.OnValueChanged += (value) => SettingDataController.Instance.SetSeVolume(value);
        masterSlider.OnValueChanged += (value) => SettingDataController.Instance.SetMasterVolume(value);
    }
}
