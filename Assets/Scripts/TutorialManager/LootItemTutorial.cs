using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LootItemTutorial : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {

        BuildItem buildItem;
        if (other.TryGetComponent<BuildItem>(out buildItem))
        {
            Debug.Log("LootTut");
            TutorialManager.Instance.ShowLootItemTutorial();
        }
    }

    
}
