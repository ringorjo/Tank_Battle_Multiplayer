using System;
using Unity.Netcode;
using UnityEngine;

public class TankMovement : NetworkBehaviour
{
    [Header("References")]
    [SerializeField]
    private InputReader _inputActionAsset;
    [SerializeField]
    private Transform _bodyTank;
    [SerializeReference]
    private Rigidbody2D _rb;
    [Header("Settings")]
    [SerializeField]
    private float _movementSpeed;
    [SerializeField]
    private float _boostSpeed;
    [SerializeField]
    private float _rotSpeed;
    private float _boost = 1;
    private Vector2 _currentDirection;


    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
            return;

        _inputActionAsset.OnMoveEvent += OnMoveTank;
        _inputActionAsset.OnSpeedEvent += OnApplyBoost;
    }

    private void OnApplyBoost(bool ispressed)
    {
        Debug.Log("IsPressed");
        _boost = !ispressed ? 1 : _boostSpeed;
    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner)
            return;
        _inputActionAsset.OnMoveEvent -= OnMoveTank;
    }

    private void Update()
    {
        if (!IsOwner)
            return;

        float ZRot = _currentDirection.x * -_rotSpeed * Time.deltaTime;

        _bodyTank.Rotate(new Vector3(0, 0, ZRot));

    }

    private void FixedUpdate()
    {
        if (!IsOwner)
            return;
        float speed = _movementSpeed * _boost;
        _rb.linearVelocity = _bodyTank.up * _currentDirection.y * speed;

    }

    private void OnMoveTank(Vector2 vector)
    {
        _currentDirection = vector;

    }
}
