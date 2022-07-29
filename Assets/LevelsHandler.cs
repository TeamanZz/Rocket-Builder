using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsHandler : MonoBehaviour
{
    public static LevelsHandler Instance;

    private int currentLevelIndex;
    public Transform targetPlanet;
    public List<GameObject> startPlanetsContainer = new List<GameObject>();
    public List<GameObject> targetPlanetsContainer = new List<GameObject>();
    public List<GameObject> levelTriggersContainer = new List<GameObject>();
    public List<float> targetPlanetYPosition = new List<float>();
    public List<int> targetDistancesUI = new List<int>();

    private void Awake()
    {
        Instance = this;
    }

    public float GetTargetPlanetPosition()
    {
        return targetPlanetYPosition[currentLevelIndex];
    }
    public int GetTargetDistanceNuber()
    {
        return targetDistancesUI[currentLevelIndex];
    }

    public void SetNewLevel()
    {
        startPlanetsContainer[currentLevelIndex].SetActive(false);
        targetPlanetsContainer[currentLevelIndex].SetActive(false);
        levelTriggersContainer[currentLevelIndex].SetActive(false);

        currentLevelIndex++;

        startPlanetsContainer[currentLevelIndex].SetActive(true);
        targetPlanetsContainer[currentLevelIndex].SetActive(true);
        levelTriggersContainer[currentLevelIndex].SetActive(true);
    }
}