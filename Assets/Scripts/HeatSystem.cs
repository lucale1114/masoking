using System;
using System.Collections;
using UnityEngine;

public class HeatSystem : MonoBehaviour
{
    [SerializeField] private int maximumHeat = 100;
    [SerializeField] private int startHeat = 50;
    [SerializeField] private int heatDecayPerSecond = 1;

    public event Action<float> HeatChanged;

    private int _currentHeat;

    private void Awake()
    {
        _currentHeat = startHeat;
        StartCoroutine(HeatDecayRoutine());
    }

    public void ChangeHeat(int amount)
    {
        _currentHeat += amount;
        HeatChanged?.Invoke(GetCurrentHeatNormalized());
    }

    private float GetCurrentHeatNormalized()
    {
        return _currentHeat / (float) maximumHeat;
    }

    private IEnumerator HeatDecayRoutine()
    {
        ChangeHeat(0);
        while (true) {
            yield return new WaitForSeconds(1f);
            ChangeHeat(-heatDecayPerSecond);
        }
    }
}