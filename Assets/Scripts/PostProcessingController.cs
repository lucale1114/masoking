using System.Collections;
using Player;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingController : MonoBehaviour
{
    [SerializeField] private float vignetteStartIntensity = 0.2f;
    [SerializeField] private float vignetteEndIntensity = 0.4f;
    [SerializeField] private float vignetteDuration = 0.2f;

    private Vignette _vignette;
    private HeatSystem _heatSystem;

    private void Start()
    {
        _heatSystem = FindObjectOfType<HeatSystem>();

        GetComponent<Volume>().profile.TryGet(out _vignette);
        _vignette.intensity.value = vignetteStartIntensity;

        _heatSystem.TakenDamage += () => StartCoroutine(ShakeRoutine());
    }

    private IEnumerator ShakeRoutine()
    {
        _vignette.intensity.value = vignetteEndIntensity;
        yield return new WaitForSeconds(vignetteDuration);
        _vignette.intensity.value = vignetteStartIntensity;
    }
}