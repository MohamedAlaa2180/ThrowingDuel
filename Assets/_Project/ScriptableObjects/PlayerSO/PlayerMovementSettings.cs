using UnityEngine;

[CreateAssetMenu(fileName = "PlayerMovementSettings", menuName = "Sos/Player/MovementSettings")]

public class PlayerMovementSettings : ScriptableObject
{
    [SerializeField] private float _movementSpeed;

    public float MovementSpeed => _movementSpeed;
}