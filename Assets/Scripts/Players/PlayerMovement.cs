using System;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _velocity;
    [SerializeField, Min(1)] private int _playerNum = 1;
    private Rigidbody2D _rigidbody;
    private Vector2 _moveDelta = Vector2.zero;
    private Vector3 _baseVelocity = Vector2.zero;

    private Actions actions;
    private bool _lostMovement;

    public Vector2 Velocity => _baseVelocity;

    public bool CanMove
    {
        get { return actions.MovementPointsUsed <= actions.MaxMovementPoints 
                     && (actions.Actor != null && actions.Actor.IsTurn); }
    }

    private void Awake()
    {
        actions = GetComponent<Actions>();
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        ResetUsedDistance();
    }

    public void FixedUpdate()
    {
        if (CanMove)
        {
            DoMovement();

            float movementDelta = _baseVelocity.magnitude * Time.fixedDeltaTime;
            actions.TakeMovementPoints(movementDelta);

            _rigidbody.linearVelocity = _baseVelocity;

            if (!CanMove)
            {
                _lostMovement = true;
            }
        }
        else if (_lostMovement)
        {
            _rigidbody.linearVelocity = Vector2.zero;
            _lostMovement = false;
        }
    }

    public void ResetUsedDistance()
    {
        _lostMovement = false;
    }

    public void Update()
    {
        //  Get input on update or it will be janky
        _moveDelta.x = Input.GetAxis("Horizontal" + _playerNum);
        _moveDelta.y = Input.GetAxis("Vertical" + _playerNum);
    }

    private void DoMovement()
    {
        _baseVelocity = Vector3.Normalize(_moveDelta) * _velocity;
    }
}