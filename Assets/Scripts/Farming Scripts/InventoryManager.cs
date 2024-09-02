using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static SceneData;

public class InventoryManager : MonoBehaviour
{

    [SerializeField] private TMP_Text tmpSpace;
    [SerializeField] private GameObject[] inventoryUI;
    [SerializeField] private CropUI_SO[] cropUI_SOs;
    public GameObject inventoryPanel;
    public GameObject sellDialogBox;
    public GameObject inventoryFullDialog;

    private float availableSpace = 0f;
    private float totalSpace = 120f;
    private bool isPanelActive;

    [HideInInspector] public List<InventoryItems> inventoryItems = new List<InventoryItems>();

    private void Start()
    {
        SceneData sceneData = SaveSystem.LoadScene();
        if (sceneData != null)
        {
            inventoryItems = sceneData.inventoryItems;
            updateInventoryUI();
        }
    }

    public bool addToInventory(CropUI_SO crop, int count)
    {
        if (crop.weight * count + availableSpace > totalSpace)
        {
            inventoryFullDialog.SetActive(true);
            return false;
        }
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (crop.cropName == inventoryItems[i].itemName)
            {
                InventoryItems temp = inventoryItems[i];
                temp.itemCount += count;
                inventoryItems[i] = temp;
                updateInventoryUI();
                return true;
            }
        }
        inventoryItems.Add(new InventoryItems(crop.cropName, count, crop.weight, crop.baseMarketPrice));
        updateInventoryUI();
        return true;
    }

    public void updateInventoryUI()
    {
        updateInventory();
        availableSpace = 0f;
        int index = 0;
        foreach (InventoryItems item in inventoryItems)
        {
            float bagWeight = 0;
            int itemCount = 0;
            int totalItems = item.itemCount;
            while (totalItems > 0)
            {
                availableSpace += item.itemWeight;
                bagWeight += item.itemWeight;
                itemCount++;
                totalItems--;
                InventoryItemTemplate inventoryUI_Item = inventoryUI[index].GetComponent<InventoryItemTemplate>();
                inventoryUI[index].SetActive(true);
                inventoryUI_Item.itemCount.SetText(itemCount.ToString());

                foreach (CropUI_SO crop in cropUI_SOs)
                    if (crop.cropName == item.itemName)
                        inventoryUI_Item.itemImage.texture = crop.UI_Image.texture;

                inventoryUI_Item.itemName = item.itemName;

                if (bagWeight >= 10)
                {
                    index++;
                    itemCount = 0;
                    bagWeight = 0;
                }
            }
            index++;
        }

        for (int i = index; i < inventoryUI.Length; i++) 
            inventoryUI[i].SetActive(false);

        tmpSpace.SetText("Space: "+ availableSpace.ToString() + "/" + totalSpace.ToString() + "kg");
    }

    public void toggleInventory()
    {
        if (!isPanelActive)
        {
            inventoryPanel.SetActive(true);
            LeanTween.moveY(inventoryPanel, 0f, 0.1f);
            isPanelActive = true;
        }
        else
        {
            inventoryPanel.SetActive(false);
            LeanTween.moveY(inventoryPanel, -500f, 0.1f);
            isPanelActive = false;
        }
    }

    public void promptSelling(int index)
    {
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (inventoryItems[i].itemName == inventoryUI[index].GetComponent<InventoryItemTemplate>().itemName)
            {
                InventoryItems temp = inventoryItems[i];
                temp.isItemToSell = true;
                inventoryItems[i] = temp;
                break;
            }
        }

        sellDialogBox.GetComponent<SalesManager>().setupSales();
        sellDialogBox.SetActive(true);
        toggleInventory();
    }

    private void updateInventory()
    {
        for (int i = 0;i < inventoryItems.Count;i++)
        {
            if (inventoryItems[i].itemCount == 0)
            {
                inventoryItems.RemoveAt(i);
            }
        }
    }
}
