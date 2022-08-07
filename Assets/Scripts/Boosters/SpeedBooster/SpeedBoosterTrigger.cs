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
    [SerializeField] private float speedBoostDuration;

    public float defaultFOV;
    public float increasedFOV;

    private bool wasTriggered;

    private Tween fovTweenUp;
    private Tween fovTweenDown;
    private Tween speedTween;

    private Coroutine boostCoroutine;

    public void StopBoostCoroutine()
    {
        StopCoroutine(boostCoroutine);
    }

    private void Awake()
    {
        mainCamera = FindObjectOfType<CinemachineVirtualCamera>();
        Debug.Log($"default fov = {defaultFOV} + {increasedFOV}");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (wasTriggered)
            return;

        BuildItem buildItem;
        if (other.TryGetComponent<BuildItem>(out buildItem))
        {
            wasTriggered = true;

            boostCoroutine = StartCoroutine(SpeedBooster());

            if (PlayerRocket.Instance.lastBoosterTrigger != null)
            {
                PlayerRocket.Instance.lastBoosterTrigger.ResetSpeedAndFOVValuesOnDuplicates();
                PlayerRocket.Instance.lastBoosterTrigger.StopBoostCoroutine();
                PlayerRocket.Instance.lastBoosterTrigger = null;
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
    }

    public void SlowlyChangeCameraFieldUp()
    {
        fovTweenUp = DOTween.To(x => mainCamera.m_Lens.FieldOfView = x, mainCamera.m_Lens.FieldOfView,
            increasedFOV, speedBoostDuration / 2);
    }

    public void SlowlyChangeCameraFieldBack()
    {
        fovTweenDown = DOTween.To(x => mainCamera.m_Lens.FieldOfView = x, mainCamera.m_Lens.FieldOfView,
           defaultFOV, speedBoostDuration);
    }

    public void SlowlyDecreaseSpeed()
    {
        speedTween = DOTween.To(x => SpaceShipMovement.Instance.constantVelocity = x, SpaceShipMovement.Instance.constantVelocity,
            SpaceShipMovement.Instance.rocketTrueSpeed, 0.3f);
    }

    private void ResetSpeedAndFOVValuesOnDuplicates()
    {
        DOTween.Kill(fovTweenUp);
        DOTween.Kill(fovTweenDown);
        DOTween.Kill(speedTween);
        fovTweenUp.Kill();
        fovTweenDown.Kill();
        speedTween.Kill();
        // mainCamera.m_Lens.FieldOfView = mainCameraFieldOfViewValue;
        // SpaceShipMovement.Instance.constantVelocity = SpaceShipMovement.Instance.rocketTrueSpeed;
    }
}