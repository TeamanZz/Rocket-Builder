using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuckWithEnemy : MonoBehaviour
{
    [SerializeField] private GameObject deathParticle;
    [SerializeField] private float explodeDamage;

    private void OnTriggerEnter(Collider other)
    {
        BuildItem buildItem;
        if (other.TryGetComponent<BuildItem>(out buildItem))
        {
            Instantiate(deathParticle, transform.position, Quaternion.identity);
            PlayerRocket.Instance.DescreaseHealth(explodeDamage);
            Destroy(transform.parent.gameObject);
        }
    }
}