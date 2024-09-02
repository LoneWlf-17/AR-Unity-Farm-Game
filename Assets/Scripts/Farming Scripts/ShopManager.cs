using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;
using System.Linq;

public class ShopManager : MonoBehaviour
{
    public LandUI_SO[] landUI_SO;
    public GameObject[] landUI;

    public CropUI_SO[] cropUI_SO;
    public GameObject[] cropUI;

    private void Start()
    {
        loadLandUI();
        loadCropUI();
    }

    public void loadLandUI()
    {
        for (int i = 0; i < landUI_SO.Length; i++)
        {
            LandShopTemplate landShop = landUI[i].GetComponent<LandShopTemplate>();
            landUI[i].SetActive(true);
            landShop.itemName.SetText(landUI_SO[i].itemName);
            landShop.itemImage.texture = landUI_SO[i].itemImage.texture;
            landShop.itemCost.SetText(landUI_SO[i].itemCost.ToString() + " Coins");
        }
    }
    
    public void purchaseLand(int index)
    {
        if (landUI_SO[index].itemCost <= GameManager.Instance.coins)
        {
            GameManager.Instance.buyLand(landUI_SO[index].itemName);
            GameManager.Instance.coins -= landUI_SO[index].itemCost;
            GameManager.Instance.loadCoinDisplay();
        }
    }

    public void loadCropUI()
    {
        for (int i = 0; i < cropUI_SO.Length - 1; i++)
        {
            for (int j = 0; j < cropUI_SO.Length - 1; j++)
            {
                if (cropUI_SO[j].order > cropUI_SO[j + 1].order)
                {
                    CropUI_SO temp_SO = cropUI_SO[j];
                    cropUI_SO[j] = cropUI_SO[j + 1];
                    cropUI_SO[j + 1] = temp_SO;
                }
            }
        }

        for (int i = 0; i < cropUI_SO.Length; i++)
        {
            CropShopTemplate cropShop = cropUI[i].GetComponent<CropShopTemplate>();
            cropUI[i].SetActive(true);
            cropShop.cropName.SetText(cropUI_SO[i].cropName);
            cropShop.cropImage.texture = cropUI_SO[i].UI_Image.texture;
            cropShop.timeToGrow.SetText(cropUI_SO[i].timeToGrow.ToString() + " s");
            cropShop.cropCost.SetText(cropUI_SO[i].cropCost.ToString() + " Coins");
        }
    }

    public void purchaseCrop(int index)
    {
        if (cropUI_SO[index].cropCost <= GameManager.Instance.coins)
        {
            GameManager.Instance.plantCrop(cropUI_SO[index].cropName);
            GameManager.Instance.coins -= cropUI_SO[index].cropCost * 12;
            GameManager.Instance.loadCoinDisplay();
        }
    }
}
