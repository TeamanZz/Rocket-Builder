using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SatelliteMovement : MonoBehaviour
{
    [SerializeField] private float satelliteSpeed;
    [SerializeField] private int minSpeedMultiplier = 1, maxSpeedMultiplier = 3;
    private int speedMultiplier;

    private void Start()
    {
        speedMultiplier = Random.Range(minSpeedMultiplier, maxSpeedMultiplier);
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector3.down * Time.fixedDeltaTime * (satelliteSpeed * speedMultiplier));
    }
}
