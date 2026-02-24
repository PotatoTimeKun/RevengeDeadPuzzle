using UnityEngine;

public class InputTest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //InputHandler.Instance.SetInputState(InputState.Player);
        InputHandler.Instance.Player.Move += PlayerMove;
        InputHandler.Instance.Player.Jump += PlayerJump;
        InputHandler.Instance.Player.Drag += PlayerDrag;
        InputHandler.Instance.Player.Suicide += PlayerSuicide;
        InputHandler.Instance.Player.Menu += PlayerMenu;
        InputHandler.Instance.SetInputState(InputState.Menu);
        InputHandler.Instance.Menu.Move += MenuMove;
        InputHandler.Instance.Menu.Submit += MenuSubmit;
        InputHandler.Instance.Menu.Cancel += MenuCancel;
    }

    private void PlayerMove(Vector2 v){
        Debug.Log("PlayerMove");
        Debug.Log(v);
    }
    private void PlayerJump(){
        Debug.Log("Jump");
    }
    private void PlayerDrag(){
        Debug.Log("Drag");
    }
    private void PlayerSuicide(){
        Debug.Log("Suicide");
    }
    private void PlayerMenu(){
        Debug.Log("Menu");
    }
    private void MenuMove(Vector2 v){
        Debug.Log("MenuMove");
        Debug.Log(v);
    }
    private void MenuSubmit(){
        Debug.Log("Submit");
    }
    private void MenuCancel(){
        Debug.Log("Cancel");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
