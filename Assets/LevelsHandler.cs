using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsHandler : MonoBehaviour
{
    public static LevelsHandler Instance;

    public Transform targetPlanet;

    private int currentLevelIndex;

    public List<GameObject> startPlanetsContainer = new List<GameObject>();
    public List<GameObject> targetPlanetsContainer = new List<GameObject>();
    public List<GameObject> levelTriggersContainer = new List<GameObject>();

    public List<float> targetPlanetYPosition = new List<float>();

    public List<int> targetDistancesUI = new List<int>();
    public List<float> flyTime = new List<float>();


    private void Awake()
    {
        Instance = this;
    }

    public void SetNewLevel()
    {

    }
}