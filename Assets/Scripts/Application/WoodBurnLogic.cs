using MyGame.Domain;
using MyGame.Presentation;

namespace MyGame.Application
{
    public class WoodBurnLogic : ITickable
    {
        private readonly WoodState _state;
        private readonly WoodView _view;
        private bool _isEffectPlayed;

        public WoodBurnLogic(WoodState state, WoodView view)
        {
            _state = state;
            _view = view;
            // Viewから通知が来たら、状態を「燃焼中」にする
            _view.OnFireTouched += () => _state.StartBurning();
        }

        public void Tick(float deltaTime)
        {
            if (!_state.IsBurning || _state.IsDead) return;

            // 最初の一度だけエフェクトを出す
            if (!_isEffectPlayed)
            {
                _view.PlayFireEffect();
                _isEffectPlayed = true;
            }

            // カウントダウン
            _state.ReduceHealth(deltaTime);

            // 死亡判定
            if (_state.IsDead)
            {
                _view.Remove();
            }
        }
    }
}