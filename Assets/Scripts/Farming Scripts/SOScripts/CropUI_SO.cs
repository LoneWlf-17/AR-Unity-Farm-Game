using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "cropItem", menuName = "Shop UI Item/Crop UI Item", order = 2)]
public class CropUI_SO : ScriptableObject
{
    public string cropName;
    public GameObject prefab;
    public GameObject childCropPrefab;
    public Sprite UI_Image;
    public float timeToGrow;
    public int cropCost;
    public float weight;
    public int order;
    public int baseMarketPrice;
}
