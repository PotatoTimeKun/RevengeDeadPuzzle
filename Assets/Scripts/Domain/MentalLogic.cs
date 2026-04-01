// UnityEngine禁止
using System;

public class MentalLogic
{
    private float _currentValue;
    private float _maxValue;
    public float MaxValue {
        get { return _maxValue; }
    }
    public float CurrentValue {
        get { return _currentValue; }
        set {
            _currentValue = value;
            if (_currentValue < 0) _currentValue = 0;
            OnMentalChange?.Invoke();
        }
    }
    public Action OnMentalChange;

    public MentalLogic(float maxAmount)
    {
        _maxValue = maxAmount;
        _currentValue = maxAmount;
    }

    public void Decrease(float amount)
    {
        CurrentValue -= amount;
    }
}