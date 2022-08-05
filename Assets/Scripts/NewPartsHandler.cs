using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewPartsHandler : MonoBehaviour
{
    public static NewPartsHandler Instance;

    public GameObject rocketPartPopup;
    public List<PartsUIItem> boughtedItems = new List<PartsUIItem>();

    public List<ItemDropChance> itemsDropChances = new List<ItemDropChance>();

    private void Awake()
    {
        Instance = this;
    }

    [ContextMenu("UnlockNewDuplicate")]
    public void UnlockNewDuplicate()
    {
        rocketPartPopup.SetActive(true);

        bool itemFounded = false;
        BuildItem.ItemType newItemType = BuildItem.ItemType.Fuel;

        while (itemFounded == false)
        {
            int randomNum = Random.Range(1, 100);

            for (int i = 0; i < itemsDropChances.Count; i++)
            {
                if (itemsDropChances[i].leftEdge <= randomNum &&
                itemsDropChances[i].rightEdge >= randomNum &&
                boughtedItems.Find(x => x.mainImage.GetComponent<BuildItemUI>().buildItemPrefab.itemType == itemsDropChances[i].itemType))
                {
                    newItemType = itemsDropChances[i].itemType;
                    itemFounded = true;
                    break;
                }
            }
        }

        var allowedItems = boughtedItems.FindAll(x => x.mainImage.GetComponent<BuildItemUI>().buildItemPrefab.itemType == newItemType);
        var duplicatedItemNum = Random.Range(0, allowedItems.Count);
        allowedItems[duplicatedItemNum].IncreaseCount();

        rocketPartPopup.GetComponent<RocketPartPopup>().popupText.text = $"You found a {allowedItems[duplicatedItemNum].partName} part!";
        rocketPartPopup.GetComponent<RocketPartPopup>().rocketPartIcon.sprite = allowedItems[duplicatedItemNum].mainImage.sprite;
    }

    public void RemoveAllDuplicates()
    {
        for (var i = 0; i < boughtedItems.Count; i++)
        {
            if (boughtedItems[i].mainImage.GetComponent<BuildItemUI>().buildItemPrefab.itemType == BuildItem.ItemType.Capsule)
                boughtedItems[i].countValue = 0;
            else
            {
                boughtedItems[i].countValue = 1;
            }
            boughtedItems[i].HandleView();
        }
    }

    [System.Serializable]
    public class ItemDropChance
    {
        public int leftEdge;
        public int rightEdge;
        public BuildItem.ItemType itemType;
    }
}

