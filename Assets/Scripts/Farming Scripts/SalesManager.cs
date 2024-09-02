using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class SalesManager : MonoBehaviour
{
    [SerializeField] private TMP_Text dialog;
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_InputField quantityInput;
    [SerializeField] private InventoryManager inventoryManager;

    private void Start()
    {
        slider.onValueChanged.AddListener(onSliderChanged);
        quantityInput.onValueChanged.AddListener(onInputChanged);
    }

    public void setupSales()
    {
        for (int i = 0; i < inventoryManager.inventoryItems.Count; i++)
        {
            if (inventoryManager.inventoryItems[i].isItemToSell)
            {
                dialog.SetText("Do you want to sell this item " + inventoryManager.inventoryItems[i].itemName + "?");
                slider.maxValue = inventoryManager.inventoryItems[i].itemCount;
            }
        }
    }

    public void onClickSellButton()
    {
        for (int i = 0; i < inventoryManager.inventoryItems.Count; i++)
        {
            if (inventoryManager.inventoryItems[i].isItemToSell)
            {
                SceneData.InventoryItems temp = inventoryManager.inventoryItems[i];
                temp.itemCount -= (int)slider.value;
                temp.isItemToSell = false;
                inventoryManager.inventoryItems[i] = temp;
                GameManager.Instance.coins += (int)slider.value * inventoryManager.inventoryItems[i].baseMarketValue;
            }
        }
        inventoryManager.updateInventoryUI();
        GameManager.Instance.loadCoinDisplay();
        gameObject.SetActive(false);
    }

    private void onSliderChanged(float number)
    {
        if (quantityInput.text != number.ToString())
            quantityInput.text = number.ToString();
    }

    private void onInputChanged(string text)
    {
        if (slider.value.ToString() != text)
        {
            if (float.TryParse(text, out float number))
            {
                slider.value = number;
            }
        }
    }
}
