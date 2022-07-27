using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlanetsMovement : MonoBehaviour
{
    public float minSpeed;
    public float maxSpeed;

    private void Update()
    {
        transform.Rotate(0, 30 * Time.deltaTime, 0);
    }
}