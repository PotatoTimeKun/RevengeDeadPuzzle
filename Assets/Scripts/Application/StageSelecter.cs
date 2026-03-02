using UnityEngine;
using System.Collections.Generic;

public class StageSelecter
{
    private StageSelecter(){}
    private static StageSelecter _instance;
    public static StageSelecter Instance{
        get{
            if (_instance == null) _instance = new StageSelecter();
            return _instance;
        }
    }
    public List<string> UnlockedStageList = new List<string>();
    public void UnlockStage(string id){}
}
