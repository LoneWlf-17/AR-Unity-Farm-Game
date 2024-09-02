using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandManager : MonoBehaviour
{
    [SerializeField] private LandUI_SO[] landUI_SO;

    public bool isPurchased;
    public bool isSelected;

    private GameObject signBoard;

    public GameObject placedLand;

    [HideInInspector] public FarmManager placedFarmManager;
    [HideInInspector] public string placedLandName;
    [HideInInspector] public int index;
    [HideInInspector] public string areaName;

    // Start is called before the first frame update
    void Start()
    {
        loadLand();

        if (!isPurchased)
        {
            signBoard = Instantiate(GameManager.Instance.signBoardPrefab, transform);
        }
    }

    public GameObject placeLand(string landName)
    {
        if(!isPurchased && isSelected)
        {
            Destroy(signBoard);
            for (int i = 0; i < landUI_SO.Length; i++)
            {
                if (landUI_SO[i].itemName == landName)
                {
                    placedLand = Instantiate(landUI_SO[i].itemPrefab, transform);
                    placedLandName = landName;

                    if (placedLand.tag == "Field")
                    {
                        FarmManager land = placedLand.GetComponent<FarmManager>();
                        
                        land.placedLandName = landName;
                        land.placedAreaName = areaName;
                        land.index = index;
                    }
                    else if (placedLand.tag == "Tree")
                    {
                        TreeManager tree = placedLand.GetComponent<TreeManager>();
                        tree.placedLandName = landName;
                        tree.placedAreaName = areaName;
                        tree.index = index;
                    }
                    GameManager.Instance.assetManager.updateAssetData(this);

                    isPurchased = true;
                    return placedLand;
                }
            }
        }
        return null;
    }

    private void loadLand()
    {
        foreach (SceneData.AssetData assetData in GameManager.Instance.assetManager.assetData)
        {
            if (assetData.assetName == areaName)
            {
                if (assetData.landNames[index] != null)
                {
                    isSelected = true;
                    GameManager.Instance.purchacedLands.Add(placeLand(assetData.landNames[index]));
                }
            }
        }
    }

    public void toggleSelection(bool toggled)
    {
        transform.GetChild(0).gameObject.SetActive(toggled);
        isSelected = toggled;
    }
}
