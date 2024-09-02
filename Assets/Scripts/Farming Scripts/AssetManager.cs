using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using static SceneData;

public class AssetManager : MonoBehaviour
{
    [HideInInspector]
    public List<AssetData> assetData = new List<AssetData>();
    private AssetData selectedAssetData;
    private bool isLoadable;

    [SerializeField] private GameObject[] assetUIs;
    [SerializeField] private GameObject addButton;

    [SerializeField] private GameObject buyAssetDialogBox;
    [SerializeField] private TMP_Text dialogText;
    [SerializeField] private TMP_InputField inputField;

    [SerializeField] private LandUI_SO areaInfo;

    [SerializeField] private GameObject demolishButton;

    private string enteredName;

    private void Start()
    {
        SceneData sceneData = SaveSystem.LoadScene();
        if (sceneData != null)
        {
            GameManager.Instance.coins = sceneData.coins;
            GameManager.Instance.lastDateTime = sceneData.SavedDateTime;
            GameManager.Instance.loadCoinDisplay();
            assetData = sceneData.assetData;
            StartCoroutine(IloadAssetUIs());
        }
    }


    #region Button Scripts

    public void onClickAddAssetButton()
    {
        if (areaInfo.itemCost * Mathf.Pow(10, assetData.Count) <= GameManager.Instance.coins)
        {
            if (assetData.Count < assetUIs.Length)
            {
                buyAssetDialogBox.gameObject.SetActive(true);
            }
        }
        else
        {
            GameManager.Instance.notEnoughCash();
            GameManager.Instance.togglePanel(-1);
        }
    }

    public void onClickBuyAssetButton()
    {
        foreach (AssetData temp in assetData)
        {
            if (temp.assetName == enteredName)
            {
                GameManager.Instance.nameAlreadyUsedBox.SetActive(true);
                buyAssetDialogBox.SetActive(false);
                return;
            }
        }
        enteredName = inputField.text;
        GameManager.Instance.spawnableManager.enabled = true;
    }

    public void onClickLoadAssetButton(int index)
    {
        AreaManager area = null;
        foreach (AreaManager areaManager in GameManager.Instance.placedAreas)
        {
            if (assetData[index].assetName == areaManager.areaName)
            {
                area = areaManager;
            }
        }
        if (area != null)
        {
            GameManager.Instance.placedAreas.Remove(area);
            Destroy(area.gameObject);
        }

        selectedAssetData = assetData[index];
        GameManager.Instance.spawnableManager.enabled = true;
        isLoadable = true;
        GameManager.Instance.togglePanel(-1);
    }

    #endregion


    public void placeArea()
    {
        if (isLoadable)
        {
            GameObject indicatorPlane = GameManager.Instance.indicatorPlane;
            if (indicatorPlane != null)
            {
                AreaManager placedArea = Instantiate(areaInfo.itemPrefab, indicatorPlane.transform.position, indicatorPlane.transform.rotation).GetComponent<AreaManager>();
                placedArea.areaName = selectedAssetData.assetName;
                StartCoroutine(placedArea.ItakeScreenshot(Application.persistentDataPath + "/" + selectedAssetData.assetName + ".png"));

                GameManager.Instance.placedAreas.Add(placedArea);

                GameManager.Instance.spawnableManager.enabled = false;
                StartCoroutine(IloadAssetUIs());
            }
            isLoadable = false;
        }
        else
        {
            GameObject indicatorPlane = GameManager.Instance.indicatorPlane;
            if (indicatorPlane != null)
            {
                AreaManager placedArea = Instantiate(areaInfo.itemPrefab, indicatorPlane.transform.position, indicatorPlane.transform.rotation).GetComponent<AreaManager>();
                placedArea.areaName = enteredName;
                StartCoroutine(placedArea.ItakeScreenshot(Application.persistentDataPath + "/" + enteredName + ".png"));

                GameManager.Instance.placedAreas.Add(placedArea);
                GameManager.Instance.coins -= areaInfo.itemCost * (int)Mathf.Pow(10, assetData.Count);

                GameManager.Instance.spawnableManager.enabled = false;

                assetData.Add(new AssetData(enteredName));

                StartCoroutine(IloadAssetUIs());
            }
        }
    }

    public IEnumerator IloadAssetUIs()
    {
        yield return new WaitForEndOfFrame();

        for (int i = 0; i < assetData.Count; i++)
        {
            AssetShopTemplate assetShop = assetUIs[i].GetComponent<AssetShopTemplate>();
            assetUIs[i].SetActive(true);

            try
            {
                Texture2D texture = new(0, 0, TextureFormat.ARGB32, false);
                byte[] buffer = System.IO.File.ReadAllBytes(Application.persistentDataPath + "/" + assetData[i].assetName + ".png");
                texture.LoadImage(buffer);
                assetShop.assetImage.texture = texture;
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
            }

            assetShop.assetName.SetText(assetData[i].assetName);
        }

        if (assetData.Count == assetUIs.Length)
        {
            addButton.SetActive(false);
        }
    }


    #region Asset Data Updation

    public void updateAssetData(LandManager landManager)
    {
        foreach (AssetData assetData in assetData)
        {
            if (landManager.areaName == assetData.assetName)
            {
                assetData.landNames[landManager.index] = landManager.placedLandName;
            }
        }
    }

    public void updateAssetData(FarmManager farmManager)
    {
        foreach (AssetData assetData in assetData)
        {
            if (farmManager.placedAreaName == assetData.assetName)
            {
                int i = farmManager.index;
                if (farmManager.plantedCropType != null)
                {
                    assetData.farmData[i].cropName = farmManager.plantedCropType.cropName;
                    assetData.farmData[i].timeToGrow = farmManager.timeToGrow;
                    assetData.farmData[i].isHarvestable = farmManager.isHarvestable;
                }
                else
                    assetData.farmData[i].cropName = null;
            }
        }
    }

    public void updateAssetData(TreeManager treeManager)
    {
        foreach (AssetData assetData in assetData)
        {
            if (treeManager.placedAreaName == assetData.assetName)
            {
                int i = treeManager.index;
                assetData.farmData[i].cropName = treeManager.fruitInfo.cropName;
                assetData.farmData[i].timeToGrow = treeManager.timeToGrow;
                assetData.farmData[i].isHarvestable = treeManager.isHarvestable;
            }
        }
    }

    #endregion
}
