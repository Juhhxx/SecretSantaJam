using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

/// <summary>
/// Change callback, needs a team ID (usually 0 - 1) and an actor that will play that turn
/// team ids can go to 1+ if we want objects that are player independent,
/// gift box that expires for example
/// </summary>
public delegate void TurnChange(Actor actor);

public delegate void ActorQueueChange(Actor actor);

public class TurnManager : MonoBehaviour
{
    [SerializeField] private GameVariableSettings _gameEvents;

    enum TurnPhase
    {
        Idle = -1,
        Start = 0,
        Turn = 1,
        End = 2
    }

    private static TurnManager _instance;

    public static TurnManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindAnyObjectByType<TurnManager>();
            }

            return _instance;
        }
    }

    public event Action TurnEndCallback;
    public event ActorQueueChange QueueChangeCallback;

    [ShowNonSerializedField] private TurnPhase _turnPhase;

    // not using a queue so we can move up and down the list depending on abilities
    [SerializeField] private List<Actor> _turnQueue = new List<Actor>();

    [ShowNonSerializedField] private Actor _currentActor;
    public Actor CurrentActor => _currentActor;
    [ShowNonSerializedField] private Actor _nextInQueue;

    public int TurnCount { get; private set; }

    private void Awake()
    {
        _turnPhase = TurnPhase.Idle;
        // not really a singleton, beware of multiple instances
        // self contained instance that overrides per most recent scene load
        _instance = this;
    }

    [Button("Start Next Turn")]
    public void StartNextTurn()
    {
        if (_turnPhase != TurnPhase.End)
        {
            // Auto finish turn
            FinishTurn();
        }

        _turnPhase = TurnPhase.Start;

        TurnCount++;

        if (_turnQueue.Count > 0)
        {
            _currentActor = _turnQueue[0];
            _turnQueue.RemoveAt(0);

            if (_turnQueue.Count > 0)
                _nextInQueue = _turnQueue[0];
        }

        _gameEvents.RaiseTurnChange(_currentActor);

        _currentActor?.StartTurn();
    }

    [Button("Finish Turn")]
    public void FinishTurn()
    {
        if (_currentActor == null) return;

        _currentActor.EndTurn();
        _turnQueue.Add(_currentActor); // move current actor to the bottom of the queue
        if (TurnEndCallback != null)
            TurnEndCallback();

        _turnPhase = TurnPhase.End;
    }

    public void EnqueueActor(Actor actor)
    {
        if (_turnQueue == null)
            _turnQueue = new List<Actor>();

        if (!_turnQueue.Contains(actor))
        {
            if (actor.initiative < 0)
            {
                _turnQueue.Add(actor);
            }
            else
            {
                int index = SortedActorIndex(actor);
                _turnQueue.Insert(index, actor);
            }

            if (QueueChangeCallback != null)
                QueueChangeCallback(actor);
        }
    }

    /// <summary>
    /// Used on actor death, remove from the loop etc....
    /// </summary>
    /// <param name="actor"></param>
    public void RemoveActor(Actor actor)
    {
        if (_turnQueue.Contains(actor))
        {
            _turnQueue.Remove(actor);

            if (QueueChangeCallback != null)
                QueueChangeCallback(actor);
        }
    }

    public void OrderByInitiative()
    {
        _turnQueue = _turnQueue.OrderBy(x => x.initiative).ToList();
    }

    private int SortedActorIndex(Actor newActor)
    {
        int bestIndex = 0;
        for (int i = 0; i < _turnQueue.Count; i++)
        {
            if (_turnQueue[i].initiative > newActor.initiative)
            {
                bestIndex = i + 1;
            }
            else
            {
                bestIndex = i;
                break;
            }
        }

        return bestIndex;
    }
}