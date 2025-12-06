using System;
using UnityEngine;

[RequireComponent(typeof(Actor))]
public class Actions : MonoBehaviour
{
    private Actor _actor;
    private PlayerMovement _movement;
    [SerializeField] private ActionPoints _actionPoints;
    public ActionPoints ActionPoints => _actionPoints;
    
    private void Awake()
    {
        if (_actionPoints == null)
            _actionPoints = ScriptableObject.CreateInstance<ActionPoints>();

        _actor = GetComponent<Actor>();
        _movement = GetComponent<PlayerMovement>();
        _movement?.SetMoveDistance(_actionPoints.MovementPoints);
    }

    private void OnEnable()
    {
        _actor.OnStartTurn += StartTurnActions;
    }

    private void OnDisable()
    {
        _actor.OnStartTurn -= StartTurnActions;
    }

    private void StartTurnActions()
    {
        _movement.ResetUsedDistance();
    }
}