using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class PlayerThrower : MonoBehaviour
{
	[Header("Setup")]
	public Transform throwOrigin;
	public GameObject projectilePrefab;
	public TeamId team = TeamId.TeamA;

	[Header("Throwing")]
	[Min(0f)] public float throwForce = 15f;
	[Min(0f)] public float upwardForce = 2f;
	public ForceMode forceMode = ForceMode.Impulse;

#if ENABLE_INPUT_SYSTEM
	[Header("Input System")] 
	public InputActionReference throwAction; // Bind this to your "Fire/Throw" action in the editor
#endif

	private void OnEnable()
	{
#if ENABLE_INPUT_SYSTEM
		if (throwAction != null && throwAction.action != null)
		{
			throwAction.action.performed += OnThrowPerformed;
			throwAction.action.Enable();
		}
#endif
	}

	private void OnDisable()
	{
#if ENABLE_INPUT_SYSTEM
		if (throwAction != null && throwAction.action != null)
		{
			throwAction.action.performed -= OnThrowPerformed;
			throwAction.action.Disable();
		}
#endif
	}

#if ENABLE_INPUT_SYSTEM
	private void OnThrowPerformed(InputAction.CallbackContext ctx)
	{
		Throw();
	}
#endif

	public void Throw()
	{
		if (projectilePrefab == null)
		{
			Debug.LogWarning("PlayerThrower: No projectilePrefab assigned.");
			return;
		}

		Transform origin = throwOrigin != null ? throwOrigin : transform;
		var instance = Instantiate(projectilePrefab, origin.position, origin.rotation);

		// Ensure projectile component and set team
		var projectile = instance.GetComponent<Projectile>();
		if (projectile == null)
		{
			projectile = instance.AddComponent<Projectile>();
		}
		projectile.ownerTeam = team;

		// Ensure rigidbody exists
		if (!instance.TryGetComponent<Rigidbody>(out var rb))
		{
			rb = instance.AddComponent<Rigidbody>();
		}

		// Calculate initial throw direction
		Vector3 direction = origin.forward;
		direction.y += Mathf.Max(0f, upwardForce) * 0.05f; // small upward bias
		rb.AddForce(direction.normalized * throwForce, forceMode);
	}
} 