using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

public class SpeedBoosterTrigger : MonoBehaviour
{
    [SerializeField] private float speedBooster;
    [SerializeField] private ParticleSystem enterTriggerEffect;
    [SerializeField] private CinemachineVirtualCamera mainCamera;
    [SerializeField] private float cameraFieldOfViewBoost;
    [SerializeField] private float speedBoostDuration;

    private float mainCameraFieldOfViewValue;
    private float mainCameraFieldOFViewIncreasedValues;

    private bool wasTriggered;

    private Tween fovTweenUp;
    private Tween fovTweenDown;
    private Tween speedTween;

    private Coroutine boostCoroutine;

    private void Awake()
    {
        mainCamera = FindObjectOfType<CinemachineVirtualCamera>();
        mainCameraFieldOfViewValue = mainCamera.m_Lens.FieldOfView;
        mainCameraFieldOFViewIncreasedValues = mainCamera.m_Lens.FieldOfView + cameraFieldOfViewBoost;
        Debug.Log($"default fov = {mainCameraFieldOfViewValue} + {mainCameraFieldOFViewIncreasedValues}");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (wasTriggered)
            return;

        BuildItem buildItem;
        if (other.TryGetComponent<BuildItem>(out buildItem))
        {
            wasTriggered = true;
            //ResetSpeedAndFOVValuesOnDuplicates();
            boostCoroutine = StartCoroutine(SpeedBooster());
            if (PlayerRocket.Instance.lastBoosterTrigger != null)
            {
                
            }
            PlayerRocket.Instance.lastBoosterTrigger = this;
        }
    }

    private IEnumerator SpeedBooster()
    {
        SlowlyChangeCameraFieldUp();
        SpaceShipMovement.Instance.constantVelocity = speedBooster;
        enterTriggerEffect.Play();
        yield return new WaitForSeconds(speedBoostDuration);
        SlowlyDecreaseSpeed();
        SlowlyChangeCameraFieldBack();
        wasTriggered = false;
    }

    public void SlowlyChangeCameraFieldUp()
    {
        fovTweenUp = DOTween.To(x => mainCamera.m_Lens.FieldOfView = x, mainCamera.m_Lens.FieldOfView,
            mainCameraFieldOFViewIncreasedValues, speedBoostDuration / 2);
    }

    public void SlowlyChangeCameraFieldBack()
    {
        fovTweenDown = DOTween.To(x => mainCamera.m_Lens.FieldOfView = x, mainCamera.m_Lens.FieldOfView,
           mainCameraFieldOfViewValue, speedBoostDuration);
    }

    public void SlowlyDecreaseSpeed()
    {
        speedTween = DOTween.To(x => SpaceShipMovement.Instance.constantVelocity = x, SpaceShipMovement.Instance.constantVelocity,
            SpaceShipMovement.Instance.rocketTrueSpeed, 0.3f);
    }

    private void ResetSpeedAndFOVValuesOnDuplicates()
    {
        fovTweenUp.Kill();
        fovTweenDown.Kill();
        speedTween.Kill();
        mainCamera.m_Lens.FieldOfView = mainCameraFieldOfViewValue;
        SpaceShipMovement.Instance.constantVelocity = SpaceShipMovement.Instance.rocketTrueSpeed;
    }
}
