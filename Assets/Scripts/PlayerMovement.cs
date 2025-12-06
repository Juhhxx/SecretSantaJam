using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _velocity;
    [SerializeField] private int _playerNum;
    private Rigidbody _rigidbody;
    private Vector2 _moveDelta = Vector2.zero;
    private Vector3 _baseVelocity = Vector2.zero;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void FixedUpdate()
    {
        DoMovement();

        _rigidbody.linearVelocity = _baseVelocity;
    }

    private void DoMovement()
    {
        _moveDelta.x = Input.GetAxis("Horizontal" + _playerNum);
        _moveDelta.y = Input.GetAxis("Vertical" + _playerNum);

        _baseVelocity = Vector3.Normalize(_moveDelta) * _velocity;
    }
}