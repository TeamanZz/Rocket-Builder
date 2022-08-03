using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnEndDragEngine : MonoBehaviour, IEndDragHandler
{
    [SerializeField] private GameObject objectToUnActive;
    [SerializeField] private GameObject objectToActive;

    public void OnEndDrag(PointerEventData eventData)
    {
        if (PlayerPrefs.HasKey("TutorialDone"))
        {
            return;
        }
        else
        {
            objectToUnActive.SetActive(false);
            objectToActive.SetActive(true);
            TutorialManager.Instance.EnableFuelButton();
        }
    }
}
