using UnityEngine;

public class SaveDataReset : MonoBehaviour
{
    void Start()
    {
        SaveDataStore.Instance.ResetSaveData();
    }
}
