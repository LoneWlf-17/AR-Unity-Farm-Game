using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ImageTrackingController : MonoBehaviour
{
    private ARTrackedImageManager arTrackedImageManager;
    public GameObject[] ARPrefabs;

    List<GameObject> ARObjects = new List<GameObject>();

    private void Awake()
    {
        arTrackedImageManager = GetComponent<ARTrackedImageManager>();
    }

    private void OnEnable()
    {
        arTrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    private void OnDisable()
    {
        arTrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs args)
    {
        foreach (var trackedImage in args.added)
        {
            foreach (var arPrefab in ARPrefabs)
            {
                if (arPrefab.name == trackedImage.referenceImage.name)
                {
                    var spawnedObj = Instantiate(arPrefab, trackedImage.transform);
                    ARObjects.Add(spawnedObj);
                }
            }
        }

        foreach (var trackedImage in args.updated)
        {
            foreach (var arObject in ARObjects)
            {
                if (arObject.name == trackedImage.referenceImage.name)
                {
                    arObject.SetActive(trackedImage.trackingState == TrackingState.Tracking);
                }
            }
        }
    }
}
