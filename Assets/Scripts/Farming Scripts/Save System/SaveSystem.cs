using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System;

public static class SaveSystem
{
    public static void SaveScene(AssetManager assetManager, InventoryManager inventoryManager, int coins, DateTime dateTime)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/SceneData.wlf";
        FileStream stream = new FileStream(path, FileMode.Create);

        SceneData sceneData = new SceneData(assetManager, inventoryManager, coins, dateTime);

        binaryFormatter.Serialize(stream, sceneData);
        stream.Close();
    }

    public static SceneData LoadScene()
    {
        string path = Application.persistentDataPath + "/SceneData.wlf";
        if (File.Exists(path))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SceneData sceneData = binaryFormatter.Deserialize(stream) as SceneData;
            stream.Close();

            return sceneData;
        }
        else
        {
            Debug.Log("File not found in " + path);
            return null;
        }
    }
}
