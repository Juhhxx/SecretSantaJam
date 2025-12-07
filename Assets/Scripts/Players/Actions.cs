using System;
using UnityEngine;

[RequireComponent(typeof(Actor))]
public class Actions : MonoBehaviour
{
    private Actor _actor;
    [SerializeField] private ActionPoints _actionPoints;
    [SerializeField] private GameVariableSettings _gameEvents;

    private float _movementPointsUsed;
    private int _combatPointsUsed;
    public float MovementPointsUsed => _movementPointsUsed;
    public float CombatPointsUsed => _combatPointsUsed;
    public float MaxMovementPoints => _actionPoints.MovementPoints;
    public float MaxCombatPoints => _actionPoints.CombatPoints;

    public bool OutOfPoints =>
        _movementPointsUsed >= MaxMovementPoints && _combatPointsUsed >= _actionPoints.CombatPoints;
    public Actor Actor => _actor;
    private void Awake()
    {
        if (_gameEvents == null)
        {
            _gameEvents = Resources.Load<GameVariableSettings>("GameEvents");
        }
        
        if (_actionPoints == null)
            _actionPoints = ScriptableObject.CreateInstance<ActionPoints>();

        _actor = GetComponent<Actor>();
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
        _movementPointsUsed = 0;
        _gameEvents?.RaiseMovementUpdate(_movementPointsUsed);
    }

    public void TakeMovementPoints(float amount)
    {
        _movementPointsUsed += amount;
        _gameEvents?.RaiseMovementUpdate(_movementPointsUsed);
    }

    public void TakeCombatPoints(int amount)
    {
        _combatPointsUsed += amount;
    }

    public void Update()
    {
        if (_actor.IsTurn)
        {
            if (OutOfPoints)
            {
                _actor.FinishTurnActor();
            }
        }
    }
}