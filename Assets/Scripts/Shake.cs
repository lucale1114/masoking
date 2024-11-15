using System.Collections;
using Cinemachine;
using Player;
using UnityEngine;

public class ShakeManager : MonoBehaviour
{
    [SerializeField] private float shakeAmplitude = 3f;
    [SerializeField] private float shakeFrequency = 3f;
    [SerializeField] private float shakeDuration = 0.2f;

    private HeatSystem _heatSystem;
    private CinemachineBasicMultiChannelPerlin _cinemachineBasicMultiChannelPerlin;

    private void Start()
    {
        _heatSystem = FindObjectOfType<HeatSystem>();

        var cinemachineVirtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        _cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera
            .GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        _heatSystem.TakenDamage += () => StartCoroutine(ShakeRoutine());
    }

    private IEnumerator ShakeRoutine()
    {
        _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = shakeAmplitude;
        _cinemachineBasicMultiChannelPerlin.m_FrequencyGain = shakeFrequency;
        yield return new WaitForSeconds(shakeDuration);
        _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
        _cinemachineBasicMultiChannelPerlin.m_FrequencyGain = 0;
    }
}