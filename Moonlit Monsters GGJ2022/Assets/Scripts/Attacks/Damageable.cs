using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
	[Header("Components")]

	[SerializeField]
	[Tooltip("The health component to reduce when damaged")]
	private Health _health;

	public Health Health
	{
		get
		{
			return this._health;
		}
	}

	[Header("Events")]

	[SerializeField]
	[Tooltip("The methods invoked when this is damaged\nFirst parameter is the amount of damage taken")]
	private UnityEvent<float> _onDamage;

	public UnityEvent<float> OnDamage
	{
		get
		{
			return this._onDamage;
		}
	}

	/** Take the given amount of damage
	\param amount The amount to reduce the health by
	*/
	public void Damage(float amount)
	{
		this.Health.Value -= amount;
	}
}