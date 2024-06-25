using UnityEngine;

[CreateAssetMenu(fileName = "SO_PlayerStats", menuName = "SO_PlayerStats")]
public class so_playerstats : ScriptableObject
{
    public string[] decisions;
    public int ST1;
    public int ST2;
    public int ST3;
    public int ST4;
    public int ST5;
    public int ST6;
    public int ST7;
    public int ST8;

    public void ResetStats()
    {
        // Reset all the stats to their default values
        decisions = new string[0];
        ST1 = 0;
        ST2 = 0;
        ST3 = 0;
        ST4 = 0;
        ST5 = 0;
        ST6 = 0;
        ST7 = 0;
        ST8 = 0;
    }
}
