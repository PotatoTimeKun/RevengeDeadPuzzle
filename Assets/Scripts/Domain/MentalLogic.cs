// UnityEngine禁止

public class MentalLogic
{
    private float _currentValue;
    public float MaxValue;
    public float CurrentValue {
        get { return _currentValue; }
    }

    public MentalLogic(float maxAmount)
    {
        MaxValue = maxAmount;
        _currentValue = maxAmount;
    }

    public void Decrease(float amount)
    {
        _currentValue -= amount;
        if (_currentValue < 0) _currentValue = 0;
    }

    public void Recover()
    {
        _currentValue = MaxValue;
    }
}