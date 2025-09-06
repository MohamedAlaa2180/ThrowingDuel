using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInputHandler _playerInputsHandler;

    private CharacterController _characterController;

    private const float _gravity = -9.81f;
    private float _verticalVelocity;

    private float _stickVelocity = -2f;
    private Vector3 _movementInput;
    private Vector3 _velocity;
    private bool _isGrounded;

    [SerializeField] private PlayerMovementSettings _movementSettings;

    public void Init(PlayerInputHandler playerInputsHandler)
    {
        _playerInputsHandler = playerInputsHandler;
        if (_playerInputsHandler == null)
        {
            Debug.LogError("PlayerInputsHandler is not assigned in PlayerMovement.");
        }
        if (_movementSettings == null)
        {
            Debug.LogError("PlayerMovementSettings is not assigned in PlayerMovement.");
        }
    }

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    private void Update()
    {
        Move();
    }

    public void Move()
    {
        // Reuse existing Vector3 instead of creating new one
        Vector2 inputVector = _playerInputsHandler.MovementInput;
        _movementInput.x = inputVector.x;
        _movementInput.y = 0;
        _movementInput.z = inputVector.y;

        // Reuse velocity vector
        _velocity.x = _movementInput.x * _movementSettings.MovementSpeed;
        _velocity.z = _movementInput.z * _movementSettings.MovementSpeed;

        // Apply gravity
        if (_isGrounded)
        {
            _verticalVelocity = _stickVelocity;
        }
        else
        {
            _verticalVelocity += _gravity * Time.deltaTime;
        }

        _velocity.y = _verticalVelocity;

        _characterController.Move(Time.deltaTime * _velocity);

        // Cache ground state for next frame
        _isGrounded = _characterController.isGrounded;
    }
}