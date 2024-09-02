using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaManager : MonoBehaviour
{
    [HideInInspector]
    public string areaName;
    public LandManager[] landManagers;

    [SerializeField] private GameObject grassPrefab;
    [SerializeField] private int maxGrassQuantity = 50;
    [SerializeField] private int minGrassQuantity = 30;

    public Camera imageCamera;

    public bool isSelected {  get; private set; }

    private void Start()
    {
        spawnMiscellenous();
        for (int i = 0; i < landManagers.Length; i++)
        {
            landManagers[i].index = i;
            landManagers[i].areaName = areaName;
        }
    }

    public void toggleSelection(bool toggle)
    {
        isSelected = toggle;
    }

    public void spawnMiscellenous()
    {
        int randomGrassQuantity = Random.Range(minGrassQuantity, maxGrassQuantity);
        for (int i = 0;i < randomGrassQuantity; i++)
        {
            Vector3 randomPos = new Vector3(Random.Range(-5f, 5f), 0f, Random.Range(-5f, 5f));
            GameObject grass = Instantiate(grassPrefab, transform);
            grass.transform.localPosition = randomPos;
            grass.transform.localRotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
        }
    }

    public IEnumerator ItakeScreenshot(string fullPath)
    {
        yield return new WaitForEndOfFrame();

        RenderTexture rt = new RenderTexture(256, 256, 24);
        imageCamera.targetTexture = rt;

        Texture2D texture = new Texture2D(256, 256, TextureFormat.ARGB32, false);
        imageCamera.Render();

        RenderTexture.active = rt;

        texture.ReadPixels(new Rect(0, 0, 256, 256), 0, 0);
        imageCamera.targetTexture = null;
        RenderTexture.active = null;

        byte[] buffer = texture.EncodeToPNG();
        System.IO.File.WriteAllBytes(fullPath, buffer);
    }
}
