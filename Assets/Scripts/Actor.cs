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

    public bool IsTurn
    {
        get
        {
            return TurnManager.instance.CurrentActor == this;
        }
    }
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
        OnStartTurn?.Invoke();
    }

    public void EndTurn()
    {
        OnEndTurn?.Invoke();
    }
}