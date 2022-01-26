using UnityEngine;

public class StunEffect : AttackEffectBase
{
	[SerializeField]
	[Tooltip("The number of seconds the target should be stunned for")]
	[Min(0)]
	private float _duration;

	public float Duration
	{
		get
		{
			return this._duration;
		}
	}

	protected override void OnHit(Collider2D target)
	{
		target.GetComponent<Stunnable>()?.Stun(this.Duration);
	}
}