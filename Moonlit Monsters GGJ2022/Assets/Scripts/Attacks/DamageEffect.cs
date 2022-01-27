using UnityEngine;

public class DamageEffect : AttackEffectBase
{
	[SerializeField]
	[Tooltip("The amount of damage to deal")]
	[Min(0)]
	private float _amount;

	public float Amount
	{
		get
		{
			return this._amount;
		}
	}

	/** Deal damage to the health attached to the given collider
	\param target The collider to deal damage to
	*/
	protected override void OnHit(Collider2D target)
	{
		target.GetComponent<Damageable>()?.Damage(this.Amount);
	}
}