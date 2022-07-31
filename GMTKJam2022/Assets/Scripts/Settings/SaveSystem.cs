using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

/// <summary>A class for saving and loading a player's data locally.</summary>
public class SaveSystem
{
    /// <summary>Stores the given save data locally.</summary>
    /// <param name="data">The data to be stored</param>
    public static void Save(SaveData data)
    {
        string path;
        // Saves to different paths depending on whether the user is playing in the editor or on a build.
        if (Application.isEditor)
            path = Application.persistentDataPath + "/Gamble.game";
        else
            path = "idbfs/Gamble.game";

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    /// <summary>Loads the save data from local storage.</summary>
    /// <returns>The loaded save data</returns>
    public static SaveData Load()
    {
        string path;
        // Loads from different paths depending on whether the user is playing in the editor or on a build.
        if (Application.isEditor)
            path = Application.persistentDataPath + "/Gamble.game";
        else
            path = "idbfs/Gamble.game";

        try
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();
            return data;
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
            return new SaveData();
        }
    }
}
