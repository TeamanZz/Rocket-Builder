using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            gameObject.transform.Translate(Vector3.forward * Time.deltaTime * 20);
        }
    }
}
