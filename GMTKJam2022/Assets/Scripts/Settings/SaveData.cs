using System.Collections.Generic;
[System.Serializable]

/// <summary>A class to store a player's save data.</summary>
public class SaveData
{
    /// <summary>The number of levels completed by the player.</summary>
    public float highScore;

    /// <summary>Creates a brand new, empty save file.</summary>
    public SaveData()
    {
        highScore = 0;
    }

    /// <summary>Creates a save file from the given save information</summary>
    /// <param name="savedScore">The saved high score for this player</param>
    public SaveData(float savedScore)
    {
        highScore = savedScore;
    }
}

