using System;
using UnityEngine;

/// <summary>
/// Change callback, needs a team ID (usually 0 - 1) and an actor that will play that turn
/// team ids can go to 1+ if we want objects that are player independent,
/// gift box that expires for example
/// </summary>
public delegate void TurnChange(int teamID, GameObject actor);
public class TurnManager : MonoBehaviour
{
    public static TurnManager instance;
    public event TurnChange TurnChangeCallback;

    private void Awake()
    {
        // not really a singleton, beware of multiple instances
        // self contained instance that overrides per most recent scene load
        instance = this;
    }
}
