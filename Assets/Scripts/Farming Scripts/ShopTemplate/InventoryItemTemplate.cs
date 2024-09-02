using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemTemplate : MonoBehaviour
{
    [HideInInspector] public string itemName;
    public RawImage itemImage;
    public TMP_Text itemCount;
}
