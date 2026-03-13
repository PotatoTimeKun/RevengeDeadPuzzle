using UnityEngine;
using UnityEngine.Events;

namespace Application.View.UI
{
    public class UIButton : UISelectableItem
    {
        [SerializeField, Tooltip("Event invoked when the button is submitted.")]
        public UnityEvent OnSubmitEvent;

        public override void OnSubmit()
        {
            OnSubmitEvent?.Invoke();
        }
    }
}
