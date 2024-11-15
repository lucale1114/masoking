using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private Image _heatBar;
    private HeatSystem _heatSystem;

    private void Start()
    {
        _heatBar = GameObject.Find("HeatBar").GetComponent<Image>();
        _heatSystem = FindObjectOfType<HeatSystem>();

        _heatSystem.HeatChanged += heat => _heatBar.fillAmount = heat;
    }
}