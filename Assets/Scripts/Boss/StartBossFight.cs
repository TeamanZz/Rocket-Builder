using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBossFight : MonoBehaviour
{
    [SerializeField] private GameObject bossObject;
    [SerializeField] private BossHealth bossHealth;
    private float oldFuelMultiplier;
    private float oldVelocityMultiplier;
    private float oldSideSpeed;

    private void OnTriggerEnter(Collider other)
    {
        BuildItem buildItem;
        if (other.TryGetComponent<BuildItem>(out buildItem))
        {
            Debug.Log("BossTrigger");
            bossObject.SetActive(true);
            oldFuelMultiplier = PlayerRocket.Instance.fuelDecreaseMultiplier;
            oldVelocityMultiplier = SpaceShipMovement.Instance.constantVelocity;
            oldSideSpeed = SpaceShipMovement.Instance.sideSpeed;
            SpaceShipMovement.Instance.constantVelocity = 0;
            PlayerRocket.Instance.fuelDecreaseMultiplier = 0;
            SpaceShipMovement.Instance.sideSpeed = 12;
        }
    }

    public void ReturnSpaceShipValuesOnBossDeath()
    {
        Debug.Log("boss defeated");
        SpaceShipMovement.Instance.constantVelocity = oldVelocityMultiplier;
        PlayerRocket.Instance.fuelDecreaseMultiplier = oldFuelMultiplier;
        SpaceShipMovement.Instance.sideSpeed = oldSideSpeed;
        Destroy(gameObject);
    }
}
