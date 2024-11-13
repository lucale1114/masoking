using System;
using UnityEngine;

public class HeatSystem : MonoBehaviour
{
    public const int MaximumHeat = 100;

    public event Action<float> HeatChanged;

    private int _currentHeat;

    public void IncreaseHeat(int amount)
    {
        _currentHeat += amount;
        HeatChanged?.Invoke(GetCurrentHeatNormalized());
    }

    private float GetCurrentHeatNormalized()
    {
        return _currentHeat / (float)MaximumHeat;
    }
}