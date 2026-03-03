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

    public void SaveAll()
    {

    }

    public void LoadUnlockedCostumes()
    {

    }

    public void LoadStageProgress()
    {

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
    public float Sensitivity;
    public bool RecoveryIsCat;
}