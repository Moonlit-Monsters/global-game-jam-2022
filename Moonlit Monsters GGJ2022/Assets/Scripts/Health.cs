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
			this.OnChange.Invoke(this._value);
		}
	}

	[Header("Events")]

	[SerializeField]
	[Tooltip("The events invoked when health changes")]
	public UnityEvent<float> OnChange;

	private void Awake()
	{
		this.Value = this.Maximum;
	}
}