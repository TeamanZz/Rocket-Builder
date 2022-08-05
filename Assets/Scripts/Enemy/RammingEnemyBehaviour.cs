using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RammingEnemyBehaviour : MonoBehaviour
{
    public Vector3 playerTarget;
    [SerializeField] private float speed;

    private void Awake()
    {
        playerTarget = PlayerRocket.Instance.transform.position;
        Vector3 difference = playerTarget - transform.position;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(180, 0, 90 - rotationZ);
    }

    private void FixedUpdate()
    {
        transform.position += (playerTarget - transform.position).normalized * speed * Time.deltaTime;
    }
}
