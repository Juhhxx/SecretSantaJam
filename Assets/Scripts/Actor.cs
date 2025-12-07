using System;
using UnityEngine;

// Base game actor, any object with this can interact with the turn manager
public class Actor : MonoBehaviour
{
    public event Action OnStartTurn;
    public event Action OnEndTurn;
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

    private void Awake()
    {
        gameObject.name = gameObject.name + " - Team " + teamID;
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

    public void FinishTurnActor()
    {
        TurnManager.instance.StartNextTurn();
    }

    public virtual void StartTurn()
    {
        OnStartTurn?.Invoke();
    }

    public virtual void EndTurn()
    {
        OnEndTurn?.Invoke();
    }
}