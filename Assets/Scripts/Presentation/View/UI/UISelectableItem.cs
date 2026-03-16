using UnityEngine;

namespace Application.View.UI
{
    public abstract class UISelectableItem : MonoBehaviour
    {
        [SerializeField, Tooltip("GameObject to show when this item is selected.")]
        protected GameObject SelectedObject;

        [SerializeField, Tooltip("GameObject to show when this item is NOT selected.")]
        protected GameObject UnselectedObject;

        protected virtual void Awake()
        {
            // Set initial state
            SetSelected(false);
        }

        /// <summary>
        /// Updates the visual state of the item.
        /// </summary>
        public virtual void SetSelected(bool isSelected)
        {
            if (SelectedObject != null)
            {
                SelectedObject.SetActive(isSelected);
            }

            if (UnselectedObject != null)
            {
                UnselectedObject.SetActive(!isSelected);
            }
        }

        /// <summary>
        /// Called when the item is selected and the submit action is triggered.
        /// </summary>
        public abstract void OnSubmit();

        /// <summary>
        /// Called when the item is selected and horizontal input is received.
        /// </summary>
        /// <param name="direction">The horizontal input direction (-1 to 1).</param>
        public virtual void OnHorizontalInput(float direction)
        {
            // Base implementation does nothing. Override in subclasses like UISlider.
        }
    }
}
