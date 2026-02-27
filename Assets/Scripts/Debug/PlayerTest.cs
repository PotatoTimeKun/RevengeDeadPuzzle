using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InputHandler.Instance.SetInputState(InputState.Player);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
