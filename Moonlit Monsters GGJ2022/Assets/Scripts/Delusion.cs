using UnityEngine;
using UnityEngine.Events;

public class Delusion : MonoBehaviour
{
	[Header("Settings")]

	[SerializeField]
	[Tooltip("The maximum delusion that can be accumulated")]
	[Min(0)]
	private float _maximum = 100;

	public float Maximum
	{
		get
		{
			return this._maximum;
		}
	}

	[SerializeField]
	[Tooltip("The default rate at which delusion decays per second")]
	[Min(0)]
	private float _baseDecayRate = 10;

	private float _decayRate;
	/** The rate at which delusion decays in units per second */
	public float DecayRate
	{
		get
		{
			return this._decayRate;
		}
		set
		{
			this._decayRate = Mathf.Max(0, value);
		}
	}

	/** Reset the decay rate back to the base rate */
	public void ResetDecay()
	{
		this.DecayRate = this._baseDecayRate;
	}

	private float _value;
	/** The current amount of accumulated delusion */
	public float Value
	{
		get
		{
			return this._value;
		}
		set
		{
			this._value = Mathf.Min(this._maximum, Mathf.Max(0, value));
			this.OnChange.Invoke(this._value);
		}
	}

	[Header("Events")]

	[SerializeField]
	[Tooltip("The events invoked when the amount of delusion is modified")]
	public UnityEvent<float> OnChange;

	private void Update()
	{
		this.Value -= this.DecayRate * Time.deltaTime;
	}
}