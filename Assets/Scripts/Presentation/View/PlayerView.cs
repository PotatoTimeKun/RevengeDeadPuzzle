using UnityEngine;

namespace RevengeDeadPuzzle.Presentation
{
    public class PlayerView : MonoBehaviour
    {
        private PlayerLogic _logic;
        private bool _isInitialized = false;

        [SerializeField] private Transform _costumeAnchor; // 衣装Prefabを生成する親
        private GameObject _currentCostumeInstance;

        public void Initialize(PlayerLogic logic)
        {
            _logic = logic;
            _isInitialized = true;
        }

        /// <summary>
        /// 死亡・復活時にApplication層から呼ばれ、ランダムな衣装を反映する
        /// </summary>
        public void SetCostume(GameObject costumePrefab)
        {
            // 古い衣装を破棄
            if (_currentCostumeInstance != null)
            {
                Destroy(_currentCostumeInstance);
            }

            // 新しい衣装を生成してアンカーに配置
            if (costumePrefab != null)
            {
                _currentCostumeInstance = Instantiate(costumePrefab, _costumeAnchor);
                _currentCostumeInstance.transform.localPosition = Vector3.zero;
                _currentCostumeInstance.transform.localRotation = Quaternion.identity;
            }
        }

        private void LateUpdate()
        {
            if (!_isInitialized || _logic == null) return;

            // LogicのStateに合わせて表示・非表示を切り替え
            // 死亡中はモデルを隠し、復活(Alive)したら表示する
            bool shouldShow = _logic.State == Entity_Data.PlayerState.Alive;
            if (_costumeAnchor.gameObject.activeSelf != shouldShow)
            {
                _costumeAnchor.gameObject.SetActive(shouldShow);
            }
        }
    }
}