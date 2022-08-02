using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EnemyProjectile : ProjectileBase
{
    public bool wasCollided;
    public GameObject hitParticles;
    public float timeUntilDestroy;
    public float startHealth;
    private float currentHealth;
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

    public void DescreaseHealth(float value)
    {
        currentHealth -= value;

        if (currentHealth <= 0)
        {
            Instantiate(hitParticles, transform.position, Quaternion.identity, CommonContainer.Instance.transform);
            Destroy(gameObject);
        }

        float randValue = Random.Range(-0.1f, 0.1f);
        transform.DOPunchScale(new Vector3(randValue, randValue, randValue), 0.1f).SetEase(Ease.InBack);
    }
}