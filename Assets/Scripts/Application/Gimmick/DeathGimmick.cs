using UnityEngine;

public class DeathGimmick : MonoBehaviour
{
    [SerializeField]
    [Tooltip("プレイヤーが触れた際の死因")]
    private Entity_Data.DeathType deathType = Entity_Data.DeathType.None;

    private void OnTriggerEnter(Collider other)
    {
        ProcessHit(other.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        ProcessHit(collision.gameObject);
    }

    private void ProcessHit(GameObject hitObject)
    {
        PlayerController player = hitObject.GetComponent<PlayerController>();
        if (player == null)
        {
            player = hitObject.GetComponentInParent<PlayerController>();
        }

        if (player != null && player.PlayerLogic != null)
        {
            if (player.PlayerLogic.State == Entity_Data.PlayerState.Alive)
            {
                player.PlayerLogic.Die(deathType);
            }
        }
    }
}
