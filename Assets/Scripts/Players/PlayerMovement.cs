using System;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _velocity;
    [SerializeField] private int _playerNum;
    private Rigidbody2D _rigidbody;
    private Vector2 _moveDelta = Vector2.zero;
    private Vector3 _baseVelocity = Vector2.zero;

    private bool _lostMovement;
    [Header("Internal Variables")]
    [ShowNonSerializedField]
    private float _maxMoveDistance = 10f;
    [ShowNonSerializedField]
    private float _accumulatedMovement;

    public bool CanMove
    {
        get { return _accumulatedMovement <= _maxMoveDistance; }
    }

    private void Start()
    {
        ResetUsedDistance();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void FixedUpdate()
    {
        if (CanMove)
        {
            DoMovement();

            _accumulatedMovement += _baseVelocity.magnitude * Time.fixedDeltaTime;
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

    public void SetMoveDistance(float maxDistance)
    {
        _maxMoveDistance = maxDistance;
    }

    public void ResetUsedDistance()
    {
        _accumulatedMovement = 0;
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