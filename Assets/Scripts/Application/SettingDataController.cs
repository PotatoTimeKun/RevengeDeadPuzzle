using UnityEngine;

public class SettingDataController : MonoBehaviour
{
    private static SettingDataController _instance;
    public static SettingDataController Instance {
        get {
            if (_instance == null) _instance = new SettingDataController();
            return _instance;
        }
    }
    private void Awake() {
        if (_instance == null) _instance = this;
        else {
            Destroy(gameObject);
            return;
        }
    }
    private static SettingData _currentData;
    public static SettingData CurrentData {
        get {
            if (_currentData == null) _currentData = SaveDataStore.Instance.LoadSetting();
            return _currentData;
        }
    }
    public void SetBgmVolume(float volume){
        _currentData.BgmVolume = volume;
        SaveDataStore.Instance.SaveAll();
    }
    public void SetSeVolume(float volume){
        _currentData.SeVolume = volume;
        SaveDataStore.Instance.SaveAll();
    }
    public void SetMasterVolume(float volume){
        _currentData.MasterVolume = volume;
        SaveDataStore.Instance.SaveAll();
    }
    public void SetRecoveryIsCat(bool isCat){
        _currentData.RecoveryIsCat = isCat;
        SaveDataStore.Instance.SaveAll();
    }
}
