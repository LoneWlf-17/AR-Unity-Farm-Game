using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiscellenousObject : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag != "Area" && collider.tag != "BuySign")
            Destroy(gameObject);
    }
}
