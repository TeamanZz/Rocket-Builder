using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameScreen : MonoBehaviour
{
    public GameObject startCapsule;
    void Awake()
    {
        startCapsule.SetActive(false);
    }

    public void EnableCapsule()
    {
        startCapsule.SetActive(true);
    }
}