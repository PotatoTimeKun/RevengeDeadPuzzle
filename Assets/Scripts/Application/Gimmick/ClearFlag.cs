using UnityEngine;

public class ClearFlag : MonoBehaviour
{
    public ParticleSystem _particleSystem;
    public void OnTriggerEnter(Collider other) {
        if (other.GetComponentInParent<PlayerController>() == null) return;
        if (other.GetComponentInParent<PlayerController>().PlayerLogic.State != Entity_Data.PlayerState.Alive) return;
        _particleSystem.Play();
        GameUseCase.Instance.OnGoal();
    }
}
