using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreeManager : MonoBehaviour
{
    public string placedLandName;
    public string placedAreaName;
    public int index;

    [SerializeField] private GameObject[] fruitPositions;
    public CropUI_SO fruitInfo;
    [SerializeField] private Slider slider;

    private List<GameObject> spawnedFruits = new List<GameObject>();

    public bool isHarvestable { get; private set; }

    public float timeToGrow = 0f;
    private float totalGrowTime = 0f;

    private void Start()
    {
        totalGrowTime = fruitInfo.timeToGrow;
        loadSavedGame();
    }

    private void Update()
    {
        if (!isHarvestable)
        {
            slider.gameObject.SetActive(true);
            timeToGrow += Time.deltaTime;
            slider.value = timeToGrow / totalGrowTime;

            if (timeToGrow >= totalGrowTime)
            {
                isHarvestable = true;
                timeToGrow = 0f;

                slider.gameObject.SetActive(false);
                loadFruits();
            }
            slider.transform.LookAt(GameManager.Instance.player.transform);
        }

        GameManager.Instance.assetManager.updateAssetData(this);
    }

    private void loadFruits()
    {
        foreach (GameObject fruitPosition in fruitPositions)
        {
            spawnedFruits.Add(Instantiate(fruitInfo.prefab, fruitPosition.transform));
        }
    }

    public void harvestFruits()
    {
        bool isAddedToInventory = GameManager.Instance.inventoryManager.addToInventory(fruitInfo, 20);
        if (isAddedToInventory)
        {
            for (int i = 0; i < spawnedFruits.Count; i++)
            {
                Destroy(spawnedFruits[i]);
            }
            spawnedFruits.Clear();

            isHarvestable = false;

            AudioManager.instance.play("HarvestSound");
        }
    }

    private void loadSavedGame()
    {
        foreach (SceneData.AssetData assetData in GameManager.Instance.assetManager.assetData)
        {
            if (assetData.assetName == placedAreaName)
            {
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
