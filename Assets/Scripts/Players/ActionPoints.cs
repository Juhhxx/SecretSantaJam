using UnityEngine;

[CreateAssetMenu(menuName = "Game/Action Points")]
public class ActionPoints : ScriptableObject
{
    public float MovementPoints = 10; // max movement distance
    public int CombatPoints = 1;
}