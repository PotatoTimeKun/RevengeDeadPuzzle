using UnityEngine;
using Domain; // Logic担当が作っているクラスを参照

namespace Presentation
{
    
    /// <summary>
    /// 規約：1クラス1責務（見た目の更新のみを担当）
    /// </summary>
    public class PlayerView : MonoBehaviour
    {
        // 規約：private fieldは _camelCase
        private PlayerLogic _logic;

        /// <summary>
        /// 規約：依存関係はInitializeで受け取る
        /// UseCase担当がこのメソッドを呼び出します
        /// </summary>
        public void Initialize(PlayerLogic logic)
        {
            _logic = logic;
        }

        /// <summary>
        /// 規約：MonoBehaviourは薄く、Logicの結果を同期するだけ
        /// </summary>
        private void Update()
        {
            // 脳（Logic）がセットされるまでは何もしない（安全策）
            if (_logic == null) return;

            // 規約：Logicの結果を元に transform.position を更新
            // Logic担当が決めたプロパティ名（X, Y, Z）を反映します
            transform.position = new Vector3(_logic.X, _logic.Y, _logic.Z);
            
            // 余裕があれば：向き（回転）などもLogicから同期
            // transform.rotation = Quaternion.Euler(0, _logic.RotationY, 0);
        }
    }
}
