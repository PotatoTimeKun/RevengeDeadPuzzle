using UnityEngine;

namespace Presentation
{
    /// <summary>
    /// 役割：プレイヤーの見た目（衣装）を管理する
    /// 規約：座標の同期は行わず、衣装の動的生成・破棄のみを担当する
    /// </summary>
    public class PlayerView : MonoBehaviour
    {
        private PlayerLogic _playerLogic;

        [Header("コスチューム設定")]
        [SerializeField] private Transform _costumeAnchor; // 衣装モデルを配置する親オブジェクト
        
        private GameObject _currentCostumeInstance;

        /// <summary>
        /// 規約：初期化時にDomain層の参照を受け取る
        /// </summary>
        public void Initialize(PlayerLogic logic)
        {
            _playerLogic = logic;
        }

        /// <summary>
        /// 新しい衣装（ランダムに選ばれたPrefab）を設定する
        /// 死ぬたびにApplication層から呼ばれることを想定
        /// </summary>
        /// <param name="costumePrefab">新しく表示する衣装のプレハブ</param>
        public void SetCostume(GameObject costumePrefab)
        {
            // 1. 前回の衣装（死ぬ前の姿）を削除
            if (_currentCostumeInstance != null)
            {
                Destroy(_currentCostumeInstance);
            }

            // 2. 新しい衣装を生成して装着
            if (costumePrefab != null)
            {
                // アンカーの子として生成
                _currentCostumeInstance = Instantiate(costumePrefab, _costumeAnchor);
                
                // 座標と回転をリセットして親に固定
                _currentCostumeInstance.transform.localPosition = Vector3.zero;
                _currentCostumeInstance.transform.localRotation = Quaternion.identity;
            }
        }
    }
}