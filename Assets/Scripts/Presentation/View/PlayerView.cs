using UnityEngine;

namespace Presentation
{
    /// <summary>
    /// 役割：プレイヤーの「衣装（見た目）」の管理に特化したクラス
    /// </summary>
    public class PlayerView : MonoBehaviour
    {
        private PlayerLogic _playerLogic;

        [Header("コスチューム設定")]
        [SerializeField] private Transform _costumeAnchor;
        
        private GameObject _currentCostumeInstance;

        public void Initialize(PlayerLogic logic)
        {
            _playerLogic = logic;
        }

        /// <summary>
        /// 外部から渡されたPrefabを元に、現在の衣装を差し替える
        /// </summary>
        public void SetCostume(GameObject costumePrefab)
        {
            if (_currentCostumeInstance != null)
            {
                Destroy(_currentCostumeInstance);
            }

            if (costumePrefab != null)
            {
                _currentCostumeInstance = Instantiate(costumePrefab, _costumeAnchor);
                
                _currentCostumeInstance.transform.localPosition = Vector3.zero;
                _currentCostumeInstance.transform.localRotation = Quaternion.identity;
            }
        }
    }
}