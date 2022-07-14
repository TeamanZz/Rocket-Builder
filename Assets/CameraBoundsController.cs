using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraBoundsController : MonoBehaviour
{
    public float boundValue;
    public CinemachineVirtualCamera cineCamera;

    private float cineXValue;
    private CinemachineComposer transposer;

    private void Awake()
    {
        transposer = cineCamera.GetCinemachineComponent<CinemachineComposer>();
    }

    private void Update()
    {
        cineXValue = (transform.position.x - -5) / (5 + 5);
        transposer.m_ScreenX = Mathf.Clamp(Mathf.Lerp(transposer.m_ScreenX, cineXValue, Time.deltaTime), 0.45f, 0.55f);
    }
}