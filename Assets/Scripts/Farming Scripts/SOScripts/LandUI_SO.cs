using UnityEngine;

[CreateAssetMenu(fileName = "LandItem", menuName = "Shop UI Item/Land UI Item", order = 1)]
public class LandUI_SO : ScriptableObject
{
    public string itemName;
    public GameObject itemPrefab;
    public Sprite itemImage;
    public int itemCost;
}
