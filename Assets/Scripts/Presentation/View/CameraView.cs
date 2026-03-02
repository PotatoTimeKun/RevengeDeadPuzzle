using Unity.Cinemachine;
using UnityEngine;

public class CameraView : MonoBehaviour, ITickable
{
    private PlayerController _controller;

    public void Initialize(PlayerController controller)
    {
        if (controller == null)
        {
            Debug.LogWarning("PlayerControllerÇ™ë∂ç›ÇµÇ‹ÇπÇÒÅI");
            return;
        }
        _controller = controller;
        Transform transform = controller.gameObject.transform;
        controller.vcam.Follow = transform;
        controller.vcam.LookAt = transform;
    }
    public void To3rdPerson()
    {
        if (_controller == null)
        {
            Debug.LogWarning("PlayerControllerÇ™ë∂ç›ÇµÇ‹ÇπÇÒÅI");
            return;
        }


    }

    public void To1stPerson()
    {
        if (_controller == null)
        {
            Debug.LogWarning("PlayerControllerÇ™ë∂ç›ÇµÇ‹ÇπÇÒÅI");
            return;
        }


    }

    public void PlayDeathEffect(Entity_Data.DeathType type)
    {

    }

    private void Start()
    {
        GameLoop.Instance.Register(this);
    }

    public void Tick(float deltaTime)
    {

    }
}
