using UnityEngine;
using UnityEngine.Events;

public class PowerUp : MonoBehaviour
{
	[Header("Delusion Settings")]

	[SerializeField]
	[Tooltip("The amount to increase delusion by")]
	[Min(0)]
	private float _value = 50;

	public float Value
	{
		get
		{
			return this._value;
		}
	}

	[Header("Events")]

	[SerializeField]
	[Tooltip("The events invoked when this is collected")]
	public UnityEvent OnCollect;

	/** Collect this power up with the given collector
	\param collector The delusion tracker to modify
	*/
	public void Collect(Delusion collector)
	{
		if (collector != null)
		{
			collector.Value -= this.Value;
		}
		this.OnCollect.Invoke();
		Object.Destroy(this.gameObject);
	}

	private void OnCollisionEnter2D(Collision2D coll)
	{
		this.Collect(coll.gameObject.GetComponent<Delusion>());
	}
}