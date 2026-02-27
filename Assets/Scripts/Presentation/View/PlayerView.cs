using UnityEngine;

namespace RevengeDeadPuzzle.Presentation
{
    public class PlayerView : MonoBehaviour
    {
               private PlayerLogic _logic;

        [SerializeField] private Transform _costumeAnchor;

        public void Initialize(PlayerLogic logic)
        {
            _logic = logic;
        }

                public void SetCostume(string costumeId)
        {
            Debug.Log($"[View] Visual Updated: {costumeId}");
        }

        private void LateUpdate()
        {
            if (_logic == null) return;

            SyncVisualWithState();
        }

        private void SyncVisualWithState()
        {
            bool isVisible = _logic.State == Entity_Data.PlayerState.Alive;
            
            if (_costumeAnchor.gameObject.activeSelf != isVisible)
            {
                _costumeAnchor.gameObject.SetActive(isVisible);
            }
        }
    }
}