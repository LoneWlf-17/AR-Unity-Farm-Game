using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SceneData
{
    [Serializable]
    public struct FarmData
    {
        public string cropName;
        public float timeToGrow;
        public bool isHarvestable;
    }

    [Serializable]
    public struct AssetData
    {
        public string assetName;
        public string[] landNames;
        public FarmData[] farmData;

        public AssetData(string assetName)
        {
            this.assetName = assetName;
            landNames = new string[4];
            farmData = new FarmData[4];
        }
    }

    [Serializable]
    public struct InventoryItems
    {
        public string itemName;
        public int itemCount;
        public float itemWeight;
        public int baseMarketValue;
        public bool isItemToSell;
        public InventoryItems(string itemName, int itemCount, float itemWeight, int baseMarketValue)
        {
            this.itemName = itemName;
            this.itemCount = itemCount;
            this.itemWeight = itemWeight;
            this.baseMarketValue = baseMarketValue;
            isItemToSell = false;
        }
    };

    public List<AssetData> assetData = new List<AssetData>();
    public List<InventoryItems> inventoryItems = new List<InventoryItems>();
    public int coins;
    public DateTime SavedDateTime;

    public SceneData(AssetManager assetManager, InventoryManager inventoryManager, int coins, DateTime savedDateTime)
    {
        this.assetData = assetManager.assetData;
        this.inventoryItems = inventoryManager.inventoryItems;
        this.coins = coins;
        SavedDateTime = savedDateTime;
    }
}
