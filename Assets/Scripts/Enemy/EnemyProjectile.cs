using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : ProjectileBase
{
    public bool wasCollided;
    public GameObject hitParticles;
    public float timeUntilDestroy;

    private void Start()
    {
        Destroy(gameObject, timeUntilDestroy);
    }

    private void OnTriggerEnter(Collider other)
    {
        BuildItem buildItem;
        if (other.TryGetComponent<BuildItem>(out buildItem))
        {
            Vector3 particlesPosition = new Vector3(buildItem.transform.position.x + Random.Range(-0.5f, 0.5f), buildItem.transform.position.y + Random.Range(-1f, 0f), buildItem.transform.position.z - 1);
            Instantiate(hitParticles, particlesPosition, Quaternion.identity, CommonContainer.Instance.transform);
            PlayerRocket.Instance.DescreaseHealth(damage);
            Destroy(gameObject);
        }
    }
}