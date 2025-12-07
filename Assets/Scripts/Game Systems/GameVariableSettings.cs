using System;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Game/Game Variable Settings")]
public class GameVariableSettings : ScriptableObject
{
    public float CameraTurnFollowTime = 0.8f;
    public float TurnSkipHoldTime = 0.7f;

    public Action<float> movementUsageUpdate;
    public Action<float> currentSkipHoldTime;
    public TurnChange turnActorChange;
    public Action<Actor> combatCallout;
    public Action<Actor, Texture2D> portraitGenerated;
    public Action gameStart;

    public void RaiseCombatCallout(Actor a)
    {
        combatCallout?.Invoke(a);
    }

    public void RaiseGameStart()
    {
        gameStart?.Invoke();
    }

    public void RaisePortraitGenerated(Actor actor, Texture2D portrait)
    {
        portraitGenerated?.Invoke(actor, portrait);
    }
    
    public void RaiseMovementUpdate(float accumulatedMovement)
    {
        movementUsageUpdate?.Invoke(accumulatedMovement);
    }
    
    public void RaiseHoldTime(float time)
    {
        currentSkipHoldTime?.Invoke(time);
    }

    public void RaiseTurnChange(Actor a)
    {
        turnActorChange?.Invoke(a);
    }
}