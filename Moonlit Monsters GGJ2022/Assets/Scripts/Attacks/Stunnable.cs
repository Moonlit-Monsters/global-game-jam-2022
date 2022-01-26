using UnityEngine;
using UnityEngine.Events;

public class Stunnable : MonoBehaviour
{
	[SerializeField]
	[Tooltip("How quickly the stun duration should be reduced every second")]
	private float _recoveryRate = 1f;

	public float RecoveryRate
	{
		get
		{
			return this._recoveryRate;
		}
	}

	[SerializeField]
	[Tooltip("The methods invoked when this is stunned")]
	private UnityEvent _onStun;

	public UnityEvent OnStun
	{
		get
		{
			return this._onStun;
		}
	}

	/** How many seconds this is stunned for */
	public float Duration {get; private set;}

	/** Stun this for the given duration if it is greater than the current stun duration
	\param duration The number of seconds to stun this for
	*/
	public void Stun(float duration)
	{
		if (duration > this.Duration)
		{
			this.Duration = duration;
			this.OnStun.Invoke();
		}
	}

	private void Update()
	{
		this.Duration = Mathf.Max(0, this.Duration - this.RecoveryRate * Time.deltaTime);
	}
}