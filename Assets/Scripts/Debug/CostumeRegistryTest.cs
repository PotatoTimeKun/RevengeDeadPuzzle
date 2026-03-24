using UnityEngine;

public class CostumeRegistryTest : MonoBehaviour
{
    void Start(){
        GameObject obj = Instantiate(CostumeCollector.Instance.CostumeRegistry.GetById("Default"));
    }
}
