using UnityEngine;

[CreateAssetMenu(fileName = "SO_AbilityCheck", menuName = "SO_AbilityCheck")]
public class so_abilitycheck : ScriptableObject
{
    public int DC;
    public string ability;
    public bool success;
}
