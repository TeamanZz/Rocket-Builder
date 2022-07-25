using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonContainer : MonoBehaviour
{
    public static CommonContainer Instance;

    private void Awake()
    {
        Instance = this;
    }
}