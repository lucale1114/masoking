using System.Collections;
using Player;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Misc
{
    public class PostProcessingController : MonoBehaviour
    {
        [SerializeField] private float vignetteStartIntensity = 0.2f;
        [SerializeField] private float vignetteEndIntensity = 0.4f;
        [SerializeField] private float vignetteDuration = 0.2f;

        [SerializeField] private float saturationLowValue = -50f;
        [SerializeField] private float saturationHighValue = 50f;

        private Vignette _vignette;
        private ColorAdjustments _colorAdjustments;
        private HeatSystem _heatSystem;

        private void Start()
        {
            _heatSystem = FindObjectOfType<HeatSystem>();

            GetComponent<Volume>().profile.TryGet(out _vignette);
            GetComponent<Volume>().profile.TryGet(out _colorAdjustments);

            _vignette.intensity.value = vignetteStartIntensity;

            _heatSystem.HeatChanged += UpdateSaturation;
            _heatSystem.TakenDamage += Shake;
        }

        private void UpdateSaturation(float heat)
        {
            _colorAdjustments.saturation.value = Mathf.Lerp(saturationLowValue, saturationHighValue, heat);
        }

        private void Shake(float _)
        {
            StartCoroutine(ShakeRoutine());
        }

        private IEnumerator ShakeRoutine()
        {
            _vignette.intensity.value = vignetteEndIntensity;
            yield return new WaitForSeconds(vignetteDuration);
            _vignette.intensity.value = vignetteStartIntensity;
        }
    }
}