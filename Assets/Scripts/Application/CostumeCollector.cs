using UnityEngine;
using System.Collections.Generic;

public class CostumeCollector
{
    private CostumeCollector(){}
    private static CostumeCollector _instance;
    public static CostumeCollector Instance{
        get{
            if (_instance == null) _instance = new CostumeCollector();
            return _instance;
        }
    }
    public List<string> UnlockedIdList = new List<string>();
    public string UnlockRandomId(){return "";}
}
