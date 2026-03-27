using UnityEngine;

public class SwitchButton : UISelectableItem
{
    public override void OnSubmit() {
        
    }

    [SerializeField]
    public GameObject DefaultObject;
    [SerializeField]
    public GameObject SwitchedObject;
    
    private bool _isSwitched = false;
    public override void OnHorizontalInput(float direction) {
        if(direction > 0) {
            _isSwitched = true;
        } else if(direction < 0) {
            _isSwitched = false;
        }
        DefaultObject.SetActive(!_isSwitched);
        SwitchedObject.SetActive(_isSwitched);
        OnValueChanged?.Invoke(_isSwitched);
    }

    public bool IsSwitched() {
        return _isSwitched;
    }

    public void SetSwitched(bool isSwitched) {
        _isSwitched = isSwitched;
        DefaultObject.SetActive(!_isSwitched);
        SwitchedObject.SetActive(_isSwitched);
        OnValueChanged?.Invoke(_isSwitched);
    }

    public System.Action<bool> OnValueChanged;
}
