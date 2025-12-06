using System;
using UnityEngine;

// Base game actor, any object with this can interact with the turn manager
public class Actor : MonoBehaviour
{
    public event Action OnStartTurn;
    public event Action OnEndTurn;
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

    public void StartTurn()
    {
        
    }

    public void EndTurn()
    {
        
    }
}