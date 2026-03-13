using System.Collections.Generic;
using UnityEngine;

namespace Application.View.UI
{
    public class UIListNavigator : MonoBehaviour
    {
        [SerializeField, Tooltip("List of items to navigate through.")]
        public List<UISelectableItem> Items = new();

        [SerializeField, Tooltip("Require vertical input to exceed this magnitude to change selection.")]
        public float VerticalDeadzone = 0.5f;

        [SerializeField, Tooltip("Require horizontal input to exceed this magnitude to trigger horizontal action.")]
        public float HorizontalDeadzone = 0.5f;
        
        [SerializeField, Tooltip("Delay in seconds between vertical selection changes when holding input.")]
        public float VerticalRepeatDelay = 0.2f;

        private int _currentIndex = 0;
        private float _lastVerticalInputTime;

        private void OnEnable()
        {
            if (Items.Count > 0)
            {
                // Ensure initial selection state is visually correct
                UpdateSelectionVisuals();
            }

            // Subscribe to input events
            if (InputHandler.Instance != null)
            {
                InputHandler.Instance.Menu.Move += HandleMove;
                InputHandler.Instance.Menu.Submit += HandleSubmit;
            }
        }

        private void OnDisable()
        {
            if (InputHandler.Instance != null)
            {
                InputHandler.Instance.Menu.Move -= HandleMove;
                InputHandler.Instance.Menu.Submit -= HandleSubmit;
            }
        }

        private void HandleMove(Vector2 direction)
        {
            if (Items.Count == 0 || _currentIndex < 0 || _currentIndex >= Items.Count) return;

            // Determine primary axis of input
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                // Horizontal input is dominant
                if (Mathf.Abs(direction.x) > HorizontalDeadzone)
                {
                    Items[_currentIndex].OnHorizontalInput(direction.x);
                }
            }
            else
            {
                // Vertical input is dominant (or equal, default to vertical for list navigation)
                if (Mathf.Abs(direction.y) > VerticalDeadzone)
                {
                    // Basic rate limiting for continuous vertical hold
                    if (Time.unscaledTime - _lastVerticalInputTime >= VerticalRepeatDelay)
                    {
                        if (direction.y > 0)
                        {
                            SelectPrevious();
                        }
                        else if (direction.y < 0)
                        {
                            SelectNext();
                        }
                        _lastVerticalInputTime = Time.unscaledTime;
                    }
                }
                else
                {
                    // Reset input time when input is released or below deadzone to allow immediate next press
                    _lastVerticalInputTime = 0f;
                }
            }
        }

        private void HandleSubmit()
        {
            if (Items.Count > 0 && _currentIndex >= 0 && _currentIndex < Items.Count)
            {
                Items[_currentIndex].OnSubmit();
            }
        }

        private void SelectNext()
        {
            int oldIndex = _currentIndex;
            _currentIndex = (_currentIndex + 1) % Items.Count;
            if (oldIndex != _currentIndex)
            {
                UpdateSelectionVisuals();
            }
        }

        private void SelectPrevious()
        {
            int oldIndex = _currentIndex;
            _currentIndex--;
            if (_currentIndex < 0)
            {
                _currentIndex = Items.Count - 1;
            }
            
            if (oldIndex != _currentIndex)
            {
                UpdateSelectionVisuals();
            }
        }

        private void UpdateSelectionVisuals()
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i] != null)
                {
                    Items[i].SetSelected(i == _currentIndex);
                }
            }
        }
    }
}
