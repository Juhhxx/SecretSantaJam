using System;
using UnityEngine;

// Base game actor, any object with this can interact with the turn manager
public class Actor : MonoBehaviour
{
    [Min(0)]
    public int initiative;
    [Min(0)]
    public int teamID;

    private void OnEnable()
    {
        TurnManager.instance.EnqueueActor(this);
    }

    private void OnDisable()
    {
        RemoveActorFromTurns();
    }

    public void RemoveActorFromTurns()
    {
        try
        {
            TurnManager.instance.RemoveActor(this);
        }
        catch (Exception e)
        {
            
        }
    }
}