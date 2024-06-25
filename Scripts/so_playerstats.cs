using UnityEngine;

[CreateAssetMenu(fileName = "SO_PlayerStats", menuName = "SO_PlayerStats")]
public class so_playerstats : ScriptableObject
{
    public string[] decisions;
    public int Smarts;
    public int Brawn;
    public int Agility;
    public int Charisma;
    public int Popularity;
    public int Vibereading;
    public int ST7;
    public int ST8;

    public void ResetStats()
    {
        // Reset all the stats to their default values
        decisions = new string[0];
        Smarts = 0;
        Brawn = 0;
        Agility = 0;
        Charisma = 0;
        Popularity = 0;
        Vibereading = 0;
        ST7 = 0;
        ST8 = 0;
    }
}
