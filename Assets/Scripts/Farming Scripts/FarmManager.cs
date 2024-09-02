using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FarmManager : MonoBehaviour
{
    public string placedLandName;
    public string placedAreaName;
    public int index;

    private bool isPlanted;
    private bool isSelected;
    public bool isHarvestable { private set; get; }
    public CropUI_SO plantedCropType {  private set; get; }
    public float timeToGrow = 0f; 
    public float totalGrowTime = 0f;

    [SerializeField] private Color color;
    [SerializeField] private CropUI_SO[] cropUI_SO;
    [SerializeField] private Slider slider;
    [SerializeField] private GameObject fieldUI;

    private MeshRenderer Renderer;
    private List<GameObject> plantedCrops = new List<GameObject>();

    private void Start()
    {
        loadSavedGame();
        Renderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        if (isPlanted && !isHarvestable)
        {
            timeToGrow += Time.deltaTime;
            slider.value = timeToGrow / totalGrowTime;

            if (timeToGrow >= totalGrowTime)
            {
                isHarvestable = true;
                isPlanted = false;
                timeToGrow = 0f;
                fieldUI.SetActive(false);
                loadGrownCrops();
            }
        }
        fieldUI.transform.LookAt(GameManager.Instance.player.transform.position);
        fieldUI.transform.rotation *= Quaternion.Euler(0f, 180f, 0f);

        GameManager.Instance.assetManager.updateAssetData(this);
    }

    public void placeCrop(string cropName)
    {
        if (!isPlanted && !isHarvestable && isSelected)
        {
            foreach (CropUI_SO SO in cropUI_SO)
            {
                if (SO.cropName == cropName)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            plantedCrops.Add(Instantiate(SO.childCropPrefab, transform.GetChild(i).GetChild(j)));
                        }
                    }
                    totalGrowTime = SO.timeToGrow;
                    plantedCropType = SO;

                    fieldUI.gameObject.SetActive(true);
                }
            }
            foreach (GameObject crop in plantedCrops) 
                crop.transform.localRotation = Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f);
            isPlanted = true;
            isSelected = false;
        }
    }

    public void loadGrownCrops()
    {
        foreach (GameObject crop in plantedCrops)
        {
            Destroy(crop);
        }
        plantedCrops.Clear();

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                plantedCrops.Add(Instantiate(plantedCropType.prefab, transform.GetChild(i).GetChild(j)));
            }
        }
        foreach (GameObject crop in plantedCrops)
            crop.transform.localRotation = Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f);
    }

    public void harvestField()
    {
        bool isAddedToInventory = GameManager.Instance.inventoryManager.addToInventory(plantedCropType, 12);
        if (isHarvestable && isAddedToInventory)
        {
            foreach (GameObject crop in plantedCrops)
            {
                Destroy(crop);
            }
            plantedCrops.Clear();
            plantedCropType = null;
            
            isHarvestable = false;
            slider.value = 0f;

            AudioManager.instance.play("HarvestSound");
        }
    }

    public void cancelCrop()
    {
        foreach (GameObject crop in plantedCrops)
        {
            Destroy(crop);
        }
        plantedCrops.Clear();

        timeToGrow = 0f;
        isPlanted = false;
        isHarvestable = false;
        slider.value = 0f;
        fieldUI.SetActive(false);
    }

    public void toggleSelection(bool toggled)
    {
        if (toggled)
        {
            Renderer.material.color = color;
        }
        else
        {
            Renderer.material.color = Color.white;
        }

        isSelected = toggled;
    }

    private void loadSavedGame()
    {
        foreach (SceneData.AssetData assetData in GameManager.Instance.assetManager.assetData)
        {
            if (assetData.assetName == placedAreaName)
            {
                if (assetData.farmData[index].cropName != null)
                {
                    foreach (CropUI_SO crop in cropUI_SO)
                    {
                        if (crop.cropName == assetData.farmData[index].cropName)
                        {
                            isSelected = true;
                            placeCrop(crop.cropName);
                            if (assetData.farmData[index].isHarvestable)
                                timeToGrow = totalGrowTime;
                            else
                            {
                                timeToGrow = assetData.farmData[index].timeToGrow;
                                timeToGrow += (float)(DateTime.Now - GameManager.Instance.lastDateTime).TotalSeconds;
                            }
                        }
                    }
                }
            }
        }
    }


}
