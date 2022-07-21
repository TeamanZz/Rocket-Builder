using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatelliteMovement : MonoBehaviour
{
    [SerializeField] private float satelliteSpeed;

    private void FixedUpdate()
    {
        transform.Translate(Vector3.down * Time.fixedDeltaTime * satelliteSpeed);
    }
}
