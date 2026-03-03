using UnityEngine;

public class CostumeRegistryTest : MonoBehaviour
{
    public CostumeRegistry registry;
    void Start(){
        GameObject obj = Instantiate(registry.GetById("Default"));
    }
}
