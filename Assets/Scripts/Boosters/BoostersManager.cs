using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostersManager : MonoBehaviour
{
    public static BoostersManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public List<GameObject> boostersList = new List<GameObject>();

    public void ClearBoosterList()
    {
        for (int i = 0; i < boostersList.Count; i++)
        {
            Destroy(boostersList[i]);
        }
        boostersList.Clear();
    }

    public void AddBoosterToList(GameObject booster)
    {
        boostersList.Add(booster);
    }
}
