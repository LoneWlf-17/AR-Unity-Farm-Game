using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class SpawnableManager : MonoBehaviour
{
    public GameObject placeIndicatorPrefab;

    private ARRaycastManager raycastManager;

    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private Vector2 screenPoint = new Vector2(Screen.width / 2, Screen.height / 2);

    private void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
    }

    private void Update()
    {
        if (raycastManager.Raycast(screenPoint, hits, TrackableType.Planes))
        {
            if (GameManager.Instance.indicatorPlane == null)
            {
                GameManager.Instance.indicatorPlane = Instantiate(placeIndicatorPrefab);
            }
            GameManager.Instance.indicatorPlane.transform.position = hits[0].pose.position;
            GameManager.Instance.indicatorPlane.transform.rotation = hits[0].pose.rotation;
        }
    }

    private void OnEnable()
    {
        if (GameManager.Instance.indicatorPlane != null)
        {
            GameManager.Instance.indicatorPlane.SetActive(true);
        }
        GameManager.Instance.placeButton.SetActive(true);
    }

    private void OnDisable()
    {
        GameManager.Instance.indicatorPlane.SetActive(false);
        GameManager.Instance.placeButton.SetActive(false);
    }
}
