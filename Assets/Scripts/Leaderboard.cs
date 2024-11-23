using System;

[System.Serializable]
public class Leaderboard
{
    public string name;
    public int highScore;
    public float profitsEarned;
    public int shiftsCompleted;

    public Leaderboard(string name, int highScore, float profitsEarned, int shiftsCompleted)
    {
        this.name = name;
        this.shiftsCompleted = shiftsCompleted;
        this.profitsEarned = profitsEarned;
        this.highScore = highScore;
    }
}