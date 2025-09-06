using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
	[Header("Ownership")]
	public TeamId ownerTeam = TeamId.None;

	[Header("Behaviour")]
	[Min(0f)] public float lifeTimeSeconds = 10f;
	public bool destroyOnAnyCollision = false;

	private bool _hasHit;
	private float _lifeTimer;

	private void Awake()
	{
		_lifeTimer = lifeTimeSeconds;
	}

	private void Update()
	{
		if (lifeTimeSeconds <= 0f)
		{
			return;
		}

		_lifeTimer -= Time.deltaTime;
		if (_lifeTimer <= 0f)
		{
			Destroy(gameObject);
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		TryHandleHit(collision.collider);
	}

	private void OnTriggerEnter(Collider other)
	{
		TryHandleHit(other);
	}

	private void TryHandleHit(Collider other)
	{
		if (_hasHit)
		{
			return;
		}

		// If the other collider belongs to a box target
		if (other.TryGetComponent<BoxTarget>(out var box))
		{
			if (box.team != TeamId.None && box.team != ownerTeam)
			{
				_hasHit = true;
				Destroy(box.gameObject);
				Destroy(gameObject);
				return;
			}
		}

		// Optional: destroy on any collision (e.g., ground or walls)
		if (destroyOnAnyCollision)
		{
			_hasHit = true;
			Destroy(gameObject);
		}
	}
} 