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
   private bool wasTriggered;

   private float oldSpeed;

   private void OnTriggerEnter(Collider other)
   {
      if (wasTriggered)
         return;

      BuildItem buildItem;
      if (other.TryGetComponent<BuildItem>(out buildItem))
      {
         wasTriggered = true;
         StartCoroutine(SpeedBooster());
      }
   }

   private IEnumerator SpeedBooster()
   {
      Debug.Log("trigger");
      oldSpeed = SpaceShipMovement.Instance.constantVelocity;
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
      DOTween.To(x => mainCamera.m_Lens.FieldOfView = x, mainCamera.m_Lens.FieldOfView,
         mainCamera.m_Lens.FieldOfView + cameraFieldOfViewBoost, speedBoostDuration / 2);
   }
   
   public void SlowlyChangeCameraFieldBack()
   {
      DOTween.To(x => mainCamera.m_Lens.FieldOfView = x, mainCamera.m_Lens.FieldOfView,
         mainCamera.m_Lens.FieldOfView - cameraFieldOfViewBoost, speedBoostDuration * 2);
   }

   public void SlowlyDecreaseSpeed()
   {
      DOTween.To(x => SpaceShipMovement.Instance.constantVelocity = x, SpaceShipMovement.Instance.constantVelocity,
         oldSpeed, 0.5f);
   }
}
