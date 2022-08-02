using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class StartBossFight : MonoBehaviour
{
    [SerializeField] private GameObject bossObject;
    private float oldFuelMultiplier;
    private float oldVelocityMultiplier;
    private float oldSideSpeed;

    private void OnTriggerEnter(Collider other)
    {
        BuildItem buildItem;
        if (other.TryGetComponent<BuildItem>(out buildItem))
        {
            StartCoroutine(BossSpawn());
        }
    }

    private IEnumerator BossSpawn()
    {
        EnemyManager.Instance.enemiesContainer.Clear();
        bossObject.SetActive(true);
        bossObject.GetComponent<BossHealth>().SetHPValue();

        yield return new WaitForSeconds(2f);
        oldFuelMultiplier = PlayerRocket.Instance.fuelDecreaseMultiplier;
        oldVelocityMultiplier = SpaceShipMovement.Instance.constantVelocity;
        oldSideSpeed = SpaceShipMovement.Instance.sideSpeed;
        SlowlyChangeSpaceShipSpeedToZero();
        PlayerRocket.Instance.fuelDecreaseMultiplier = 0;
        SpaceShipMovement.Instance.sideSpeed = 12;
    }

    public void ReturnSpaceShipValuesOnBossDeath()
    {
        Debug.Log("boss defeated");
        SlowlyChangeSpaceShipSpeedToDefault();
        PlayerRocket.Instance.fuelDecreaseMultiplier = oldFuelMultiplier;
        SpaceShipMovement.Instance.sideSpeed = oldSideSpeed;
        Destroy(gameObject);
    }

    private void SlowlyChangeSpaceShipSpeedToZero()
    {
        DOTween.To(x => SpaceShipMovement.Instance.constantVelocity = x, SpaceShipMovement.Instance.constantVelocity,
            0, 2f);
    }
    public void SlowlyChangeSpaceShipSpeedToDefault()
    {
        DOTween.To(x => SpaceShipMovement.Instance.constantVelocity = x, SpaceShipMovement.Instance.constantVelocity,
            oldVelocityMultiplier, 1f);
    }

}
