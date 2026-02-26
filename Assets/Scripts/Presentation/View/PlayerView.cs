using UnityEngine;
using Domain;

namespace Presentation
{
    public class PlayerView : MonoBehaviour
    {
        private PlayerLogic _logic;

        public void Initialize(PlayerLogic logic)
        {
            _logic = logic;
        }

        private void Update()
        {
            // 動きの計算はせず、Logicから届いた結果を「代入」するだけ
            if (_logic == null) return;

            transform.position = new Vector3(_logic.X, _logic.Y, _logic.Z);
        }
    }
}