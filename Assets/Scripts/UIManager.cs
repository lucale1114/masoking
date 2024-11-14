using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private GameObject _heatBar;
    private HeatSystem _heatSystem;

    private void Start()
    {
        _heatBar = GameObject.Find("HeatBar");
        _heatSystem = FindObjectOfType<HeatSystem>();

        _heatBar.GetComponent<Slider>().maxValue = HeatSystem.MaximumHeat;

        _heatSystem.HeatChanged += heat => { _heatBar.GetComponent<Slider>().value = heat; };
    }
}