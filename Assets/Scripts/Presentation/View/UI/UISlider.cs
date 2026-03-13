using UnityEngine;
using UnityEngine.UI;

namespace Application.View.UI
{
    public class UISlider : UISelectableItem
    {
        [SerializeField, Tooltip("The UI Slider component to control.")]
        public Slider TargetSlider;

        [SerializeField, Tooltip("Amount to change the slider value per input step.")]
        public float StepAmount = 0.1f;

        [SerializeField, Tooltip("Delay before repeating continuous input.")]
        public float RepeatDelay = 0.2f;

        private float _lastInputTime;

        public override void OnSubmit()
        {
            // Sliders typically don't have a submit action, but could expand.
        }

        public override void OnHorizontalInput(float direction)
        {
            if (TargetSlider == null) return;

            // Simple rate limiting for continuous digital/analog input
            if (Time.unscaledTime - _lastInputTime < RepeatDelay) return;

            if (direction > 0)
            {
                TargetSlider.value = Mathf.Clamp(TargetSlider.value + StepAmount, TargetSlider.minValue, TargetSlider.maxValue);
                _lastInputTime = Time.unscaledTime;
            }
            else if (direction < 0)
            {
                TargetSlider.value = Mathf.Clamp(TargetSlider.value - StepAmount, TargetSlider.minValue, TargetSlider.maxValue);
                _lastInputTime = Time.unscaledTime;
            }
        }
    }
}
