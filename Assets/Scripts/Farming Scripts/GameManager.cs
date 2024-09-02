using System;
using System.Collections.Generic;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject player;
    public SpawnableManager spawnableManager;

    public GameObject areaPrefab;
    public GameObject signBoardPrefab;

    public GameObject placeButton;
    public GameObject[] infoPanels;
    public GameObject notEnoughCoinsDBox;
    public GameObject nameAlreadyUsedBox;

    public AssetManager assetManager;
    public InventoryManager inventoryManager;
    public TMP_Text[] coinDisplays;

    public int coins = 100;
    [HideInInspector] public DateTime lastDateTime;

    [HideInInspector] public List<AreaManager> placedAreas = new List<AreaManager>();
    [HideInInspector] public List<GameObject> purchacedLands = new List<GameObject>();
    [HideInInspector] public GameObject indicatorPlane;

    [HideInInspector] public int landNumber = 4;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(this);
        }
    }

    public void buyLand(string landName)
    {
        foreach (AreaManager area in placedAreas)
        {
            for (int i = 0; i < area.landManagers.Length; i++)
            {
                GameObject land = area.landManagers[i].placeLand(landName);
                if (land != null)
                {
                    Debug.Log(i);
                    purchacedLands.Add(land);
                }
            }
        }
        togglePanel(-1);
    }

    public void plantCrop(string cropName)
    {
        foreach (GameObject purchacedLand in purchacedLands)
        {
            if (purchacedLand != null)
            if (purchacedLand.tag == "Field")
            {
                FarmManager farmManager = purchacedLand.GetComponent<FarmManager>();
                farmManager.placeCrop(cropName);
                farmManager.toggleSelection(false);
            }
        }
        togglePanel(-1);
    }

    public void loadCoinDisplay()
    {
        foreach (TMP_Text coinDisplay in coinDisplays)
        {
            coinDisplay.SetText("Coins: " + Instance.coins);
        }
    }

    public void togglePanel(int index)
    {
        for (int i = 0; i < infoPanels.Length; i++)
        {
            if (index == i)
            {
                infoPanels[i].SetActive(true);
                LeanTween.moveY(infoPanels[i], 0f, 0.3f);
            }
            else
            {
                GameObject infoPanel = infoPanels[i];
                LeanTween.moveY(infoPanel, -500f, 0.3f).setOnComplete(() =>
                {
                    infoPanel.SetActive(false);
                });
            }
        }
    }

    public void notEnoughCash()
    {
        notEnoughCoinsDBox.SetActive(true);
    }

    public void OnApplicationPause(bool isPaused)
    {
        if (isPaused)
            SaveSystem.SaveScene(assetManager, inventoryManager, coins, DateTime.Now);
    }

    public void OnApplicationFocus(bool isFocused)
    {
        if (!isFocused)
            SaveSystem.SaveScene(assetManager, inventoryManager, coins, DateTime.Now);
    }

    public void OnApplicationQuit()
    {
        SaveSystem.SaveScene(assetManager, inventoryManager, coins, DateTime.Now);
    }
}
