using UnityEngine;
using Domain;

namespace Presentation
{
    /// <summary>
    /// 規約：1クラス1責務（キャラクターの「衣装・見た目」の同期のみを担当）
    /// 座標の移動はPlayerControllerが担当するため、ここでは触りません
    /// </summary>
    public class PlayerView : MonoBehaviour
    {
        private PlayerLogic _logic;

        [Header("衣装モデルの設定")]
        [SerializeField] private GameObject costumeRoot; // 衣装の親オブジェクトなど

        /// <summary>
        /// 規約：依存関係はInitializeで受け取る
        /// </summary>
        public void Initialize(PlayerLogic logic)
        {
            _logic = logic;
        }

        private void Update()
        {
            // 安全策：Logicがセットされるまでは何もしない
            if (_logic == null) return;

            // 規約：Domain層の状態（State）を元に見た目を更新
            // PlayerLogicのStateがAliveでない（死亡状態）なら、衣装を非表示にする等の処理
            UpdateAppearance();
        }

        /// <summary>
        /// 規約：見た目の更新ロジック
        /// </summary>
        private void UpdateAppearance()
        {
            // Logicの現在の状態を参照（Entity_Data.PlayerState.Alive など）
            // 注意：提供されたLogicでは State の型が Entity_Data.PlayerState なのでそれに合わせます
            bool isAlive = (_logic.State == Entity_Data.PlayerState.Alive);

            if (costumeRoot != null)
            {
                costumeRoot.SetActive(isAlive);
            }
        }
    }
}