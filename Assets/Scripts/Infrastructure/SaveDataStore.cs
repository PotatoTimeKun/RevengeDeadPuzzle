using System.Collections.Generic;

public class SaveDataStore
{
    private SaveDataStore(){}
    private static SaveDataStore _instance;
    public static SaveDataStore Instance {
        get {
            if (_instance == null) _instance = new SaveDataStore();
            return _instance;
        }
    }

    private SaveData _currentData;
    public SaveData CurrentData 
    {
        get
        {
            if (_currentData == null) _currentData = LoadFromCache();
            return _currentData;
        }
    }

    private const string SaveKey = "SaveData";

    private void SaveToCache(SaveData data)
    {
        string json = UnityEngine.JsonUtility.ToJson(data);
        UnityEngine.PlayerPrefs.SetString(SaveKey, json);
        UnityEngine.PlayerPrefs.Save();
    }

    private SaveData LoadFromCache()
    {
        if (UnityEngine.PlayerPrefs.HasKey(SaveKey))
        {
            string json = UnityEngine.PlayerPrefs.GetString(SaveKey);
            return UnityEngine.JsonUtility.FromJson<SaveData>(json);
        }

        return new SaveData
        {
            StageProgress = new StageProgressData { UnlockedIdList = new System.Collections.Generic.List<string>() },
            Costume = new CostumeData { UnlockedIdList = new System.Collections.Generic.List<string>() },
            Setting = new SettingData { MasterVolume = 1.0f, BgmVolume = 1.0f, SeVolume = 1.0f, RecoveryIsCat = true }
        };
    }

    public void SaveAll()
    {
        SaveToCache(CurrentData);
    }

    public CostumeData LoadUnlockedCostumes()
    {
        return CurrentData.Costume;
    }

    public StageProgressData LoadStageProgress()
    {
        return CurrentData.StageProgress;
    }

    public SettingData LoadSetting()
    {
        return CurrentData.Setting;
    }
}

[System.Serializable]
public class SaveData
{
    public StageProgressData StageProgress;
    public CostumeData Costume;
    public SettingData Setting;
}

[System.Serializable]
public class StageProgressData{
    public List<string> UnlockedIdList;
}

[System.Serializable]
public class CostumeData{
    public List<string> UnlockedIdList;
}

[System.Serializable]
public class SettingData{
    public float BgmVolume;
    public float SeVolume;
    public float MasterVolume;
    public bool RecoveryIsCat;
}