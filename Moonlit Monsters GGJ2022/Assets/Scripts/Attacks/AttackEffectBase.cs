using UnityEngine;

[RequireComponent(typeof(Attack))]
public abstract class AttackEffectBase : MonoBehaviour
{
	/** The attack this is triggered by */
	public Attack Attack {get; private set;}

	/** THe method called when the attack hits
	\param target The hit collider
	*/
	protected abstract void OnHit(Collider2D target);

	protected void Awake()
	{
		this.Attack = this.GetComponent<Attack>();
		this.Attack?.OnHit?.AddListener(this.OnHit);
	}
}