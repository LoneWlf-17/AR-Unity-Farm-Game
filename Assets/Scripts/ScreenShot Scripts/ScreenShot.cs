using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ScreenShot : MonoBehaviour
{
    public Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        TakeScreenShot(Application.dataPath + "/TreeImage.png");
    }

    public void TakeScreenShot(string fullPath)
    {
        RenderTexture rt = new RenderTexture(256, 256, 24);

        cam.targetTexture = rt;

        Texture2D screenShot = new Texture2D(256,256, TextureFormat.ARGB32, false);
        cam.Render();

        RenderTexture.active = rt;

        screenShot.ReadPixels(new Rect(0, 0, 256, 256), 0, 0);
        cam.targetTexture = null;
        RenderTexture.active = null;

        byte[] buffer = screenShot.EncodeToPNG();
        System.IO.File.WriteAllBytes(fullPath, buffer);
    }
}
