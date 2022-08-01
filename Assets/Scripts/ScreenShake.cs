using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using DG.Tweening;

public class ScreenShake : MonoBehaviour
{
    public float ShakeDuration = 0.3f;          // Time the Camera Shake effect will last
    public float ShakeAmplitude = 1.2f;         // Cinemachine Noise Profile Parameter
    public float ShakeFrequency = 2.0f;         // Cinemachine Noise Profile Parameter

    // Cinemachine Shake
    public CinemachineVirtualCamera VirtualCamera;
    private CinemachineBasicMultiChannelPerlin virtualCameraNoise;

    public CinemachineVirtualCamera cineCamera;
    private CinemachineTransposer transposer;

    public static ScreenShake Instance;

    private void Awake()
    {
        Instance = this;
        transposer = cineCamera.GetCinemachineComponent<CinemachineTransposer>();

        if (VirtualCamera != null)
            virtualCameraNoise = VirtualCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
    }

    public void ShakeScreenOnRocketStart()
    {
        DOTween.To(x => virtualCameraNoise.m_AmplitudeGain = x, virtualCameraNoise.m_AmplitudeGain,
            ShakeAmplitude, ShakeDuration).SetLoops(2, LoopType.Yoyo);

        DOTween.To(x => virtualCameraNoise.m_FrequencyGain = x, virtualCameraNoise.m_FrequencyGain,
        ShakeFrequency, ShakeDuration).SetLoops(2, LoopType.Yoyo);

        transposer.m_YDamping = 5;
        DOTween.To(x => transposer.m_YDamping = x, transposer.m_YDamping,
        1, 4f);

        IncreaseEnginesFuelOnStart();
    }

    public void IncreaseEnginesFuelOnStart()
    {
        var enginesList = BuildingGrid.Instance.GetEnginesList();
        foreach (var item in enginesList)
        {
            item.GetComponent<EngineViewController>().IncreaseTrailEffectOnStart();
        }
    }
}