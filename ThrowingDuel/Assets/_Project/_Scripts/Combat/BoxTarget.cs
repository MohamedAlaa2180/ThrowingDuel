using UnityEngine;

public class BoxTarget : MonoBehaviour
{
	[Header("Team")] 
	public TeamId team = TeamId.None;

	[Header("Optional: Visuals")]
	public Color teamAColor = new Color(0.2f, 0.6f, 1f);
	public Color teamBColor = new Color(1f, 0.4f, 0.2f);

	private void Reset()
	{
		// Try to ensure a collider exists for physics
		if (!TryGetComponent<Collider>(out _))
		{
			gameObject.AddComponent<BoxCollider>();
		}
	}

	private void OnValidate()
	{
		ApplyTeamTint();
	}

	private void ApplyTeamTint()
	{
		if (TryGetComponent<Renderer>(out var r))
		{
			var block = new MaterialPropertyBlock();
			r.GetPropertyBlock(block);
			switch (team)
			{
				case TeamId.TeamA:
					block.SetColor("_Color", teamAColor);
					break;
				case TeamId.TeamB:
					block.SetColor("_Color", teamBColor);
					break;
				default:
					break;
			}
			r.SetPropertyBlock(block);
		}
	}
} 