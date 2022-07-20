using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRotation : MonoBehaviour
{
    [SerializeField] private Vector3 rotationAngle;

    private void FixedUpdate()
    {
        transform.Rotate(rotationAngle);
    }
}
