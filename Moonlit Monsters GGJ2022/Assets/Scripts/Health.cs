using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
	[Header("Settings")]

	[SerializeField]
	[Tooltip("The maximum health that the creature starts with")]
	[Min(0)]
	private float _maximum;

	public float Maximum
	{
		get
		{
			return this._maximum;
		}
	}

	private float _value;
	/** current amount of health the creature has */
	public float Value
	{
		get
		{
			return this._value;
		}
		set
		{
			this._value = Mathf.Min(this.Maximum, Mathf.Max(0, value));
			this._onChange.Invoke(this._value);
			if (this._value <= 0)
			{
				this._onDeath.Invoke();
			}
		}
	}

	[Header("Events")]

	[SerializeField]
	[Tooltip("The events invoked when health changes")]
	private UnityEvent<float> _onChange;

	[SerializeField]
	[Tooltip("The events invoked when health is reduced to zero")]
	private UnityEvent _onDeath;
}