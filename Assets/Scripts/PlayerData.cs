using System;

[System.Serializable]
public class PlayerData
{
    public string name;
    public string storeName;
    public bool isAdmin;
    public int shiftsCompleted;
    public int itemsScanned;
    public int mistakesMade;
    public float profitsEarned;
    public int highScore;

    public PlayerData(string name, string storeName, bool isAdmin, int shiftsCompleted, int itemsScanned, int mistakesMade, float profitsEarned, int highScore)
    {
        this.name = name;
        this.storeName = storeName;
        this.isAdmin = isAdmin;
        this.shiftsCompleted = shiftsCompleted;
        this.itemsScanned = itemsScanned;
        this.mistakesMade = mistakesMade;
        this.profitsEarned = profitsEarned;
        this.highScore = highScore;
    }
}