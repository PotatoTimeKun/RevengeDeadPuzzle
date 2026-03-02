using UnityEngine;

public class GameUseCase : MonoBehaviour , ITickable
{
    private PlayerController _playerController;
    private StageDef _stageDef;
    public void StartGame(){}
    public void OnPlayerDead(Entity_Data.DeathType deathType){}
    public void OnGoal(){}
    public void Tick(float deltaTIme){}
}
