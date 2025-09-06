using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private InputActions playerInputs;
    private Vector2 movementInput;

    public static event Action OnMovementInput;

    public static event Action OnThrowInput;

    public Vector2 MovementInput => movementInput;

    private void Awake()
    {
        playerInputs = new InputActions();
        playerInputs.Enable();
    }

    private void OnEnable()
    {
        playerInputs.PlayerActionMap.Throw.performed += SetThrowEvent;
        playerInputs.PlayerActionMap.Movement.performed += SetMovementInput;
        playerInputs.PlayerActionMap.Movement.canceled += SetMovementInput;
    }

    private void OnDisable()
    {
        playerInputs.PlayerActionMap.Throw.performed -= SetThrowEvent;
        playerInputs.PlayerActionMap.Movement.performed -= SetMovementInput;
        playerInputs.PlayerActionMap.Movement.canceled -= SetMovementInput;
    }

    private void SetMovementInput(InputAction.CallbackContext context) => movementInput = context.ReadValue<Vector2>();

    private void SetThrowEvent(InputAction.CallbackContext context) => OnThrowInput?.Invoke();
}