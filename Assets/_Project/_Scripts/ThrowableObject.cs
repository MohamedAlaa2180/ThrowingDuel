using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class ThrowableObject : MonoBehaviour
{
    private Rigidbody _rigidbody;
    [SerializeField] private float LaunchForce = 10f;
    [SerializeField] private float ArcAngleDegrees = 15f;

    [SerializeField] private Vector3 _direction;
    [SerializeField] private Transform _originPosition;

    protected virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        PlayerInputHandler.OnThrowInput += TestLaunch;
    }

    private void OnDisable()
    {
        PlayerInputHandler.OnThrowInput -= TestLaunch;
    }

    public void Launch(Vector3 direction)
    {
        if (direction.sqrMagnitude < 0.0001f)
        {
            return;
        }
        _rigidbody.isKinematic = false;

        // Build an upward-tilted launch direction for a throwing arc
        Vector3 planarDirection = Vector3.ProjectOnPlane(direction, Vector3.up);
        if (planarDirection.sqrMagnitude < 0.0001f)
        {
            planarDirection = direction.normalized;
        }
        else
        {
            planarDirection.Normalize();
        }

        Vector3 tiltAxis = Vector3.Cross(planarDirection, Vector3.up);
        if (tiltAxis.sqrMagnitude < 0.0001f)
        {
            tiltAxis = Vector3.right;
        }

        Vector3 launchDirection = Quaternion.AngleAxis(ArcAngleDegrees, tiltAxis) * planarDirection;

        // Set constant launch speed (no AddForce)
        _rigidbody.linearVelocity = launchDirection.normalized * LaunchForce;
    }

    [ContextMenu("Test Launch")]
    public void TestLaunch()
    {
        Launch(_direction);
    }

    private void OnCollisionEnter(Collision collision)
    {
        var collider = collision.collider;

        if (collider != null && collider.TryGetComponent<IDestroyable>(out var destroyable))
        {
            destroyable.Destroy();
            Reset();
        }
    }

    private void Reset()
    {
        _rigidbody.isKinematic = true;
        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        transform.position = _originPosition.position;
    }
}