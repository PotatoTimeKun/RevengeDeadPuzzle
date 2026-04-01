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
            var cacheSaveData = UnityEngine.JsonUtility.FromJson<SaveData>(json);
            return cacheSaveData;
        }

        var saveData = new SaveData
        {
            StageProgress = new StageProgressData { UnlockedIdList = new System.Collections.Generic.List<string>() , ScoreDataList = new System.Collections.Generic.List<ScoreData>()},
            Costume = new CostumeData { UnlockedIdList = new System.Collections.Generic.List<string>() },
            Setting = new SettingData { MasterVolume = 1.0f, BgmVolume = 1.0f, SeVolume = 1.0f }
        };
        return saveData;
    }

    public void SaveAll()
    {
        CurrentData.StageProgress.ScoreDataList = StageSelecter.Instance.ScoreDataList;
        CurrentData.StageProgress.UnlockedIdList = StageSelecter.Instance.UnlockedStageList;
        CurrentData.Costume.UnlockedIdList = CostumeCollector.Instance.UnlockedIdList;
        CurrentData.Setting = SettingDataController.CurrentData;
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
    public List<ScoreData> ScoreDataList;
}

[System.Serializable]
public class ScoreData{
    public string StageId;
    public bool IsClear;
    public bool TimeTarget;
    public bool CountTarget;
    public bool TypeTarget;
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
}