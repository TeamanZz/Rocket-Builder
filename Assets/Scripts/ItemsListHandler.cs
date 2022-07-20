using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemsListHandler : MonoBehaviour
{
    public List<Sprite> buttonSprites = new List<Sprite>();
    public List<Image> buttonsImages = new List<Image>();

    public List<GameObject> partsContainers = new List<GameObject>();

    public void SelectButton(int buttonIndex)
    {
        buttonsImages[buttonIndex].sprite = buttonSprites[1];
        partsContainers[buttonIndex].SetActive(true);
        DeselectOtherButtons(buttonIndex);
    }

    private void DeselectOtherButtons(int buttonIndex)
    {
        for (var i = 0; i < buttonsImages.Count; i++)
        {
            if (buttonsImages[i] == buttonsImages[buttonIndex])
                continue;
            partsContainers[i].SetActive(false);
            buttonsImages[i].sprite = buttonSprites[0];

        }
    }
}