using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class Attack : MonoBehaviour
{
	[Header("Components")]

	[SerializeField]
	[Tooltip("The trigger used to detect hits")]
	private Collider2D _hitTrigger;

	public Collider2D HitTrigger
	{
		get
		{
			return this._hitTrigger;
		}
	}

	[Header("Timing Settings")]

	[SerializeField]
	[Tooltip("The number of seconds of delay before this can hit")]
	[Min(0)]
	private float _windUpTime;

	public float WindUpTime
	{
		get
		{
			return this._windUpTime;
		}
	}

	[SerializeField]
	[Tooltip("The number of seconds this can hit for")]
	[Min(0)]
	private float _holdTime;

	public float HoldTime
	{
		get
		{
			return this._holdTime;
		}
	}

	[SerializeField]
	[Tooltip("The number of seconds of delay after this has had the opportunity to hit")]
	[Min(0)]
	private float _windDownTime;

	public float WindDownTime
	{
		get
		{
			return this._windDownTime;
		}
	}

	[Header("Events")]

	[SerializeField]
	[Tooltip("The methods invoked when the state changes")]
	private UnityEvent<AttackState> _onStateChange;

	public UnityEvent<AttackState> OnStateChange
	{
		get
		{
			return this._onStateChange;
		}
	}

	[SerializeField]
	[Tooltip("The methods invoked when the hit trigger becomes active")]
	private UnityEvent _onStartSwing;

	public UnityEvent OnStartSwing
	{
		get
		{
			return this._onStartSwing;
		}
	}

	[SerializeField]
	[Tooltip("The methods invoked when a hit lands\nFirst parameter is the hit collider")]
	private UnityEvent<Collider2D> _onHit;

	public UnityEvent<Collider2D> OnHit
	{
		get
		{
			return this._onHit;
		}
	}

	[SerializeField]
	[Tooltip("The methods invoked when the hit trigger is deactivated")]
	private UnityEvent _onStopSwing;

	public UnityEvent OnStopSwing
	{
		get
		{
			return this._onStopSwing;
		}
	}

	[SerializeField]
	[Tooltip("The methods invoked when the attack is done winding down")]
	private UnityEvent _onAttackDone;

	public UnityEvent OnAttackDone
	{
		get
		{
			return this._onAttackDone;
		}
	}

	public enum AttackState
	{
		Inactive,
		WindUp,
		Hold,
		WindDown
	}

	private AttackState _currentState;
	/** The current state of the attack */
	public AttackState CurrentState 
	{
		get
		{
			return this._currentState;
		} 
		private set
		{
			this._currentState = value;
			this.OnStateChange.Invoke(value);
		}
	}

	/** Start the active portion of the attack */
	private void StartSwing()
	{
		this.HitTrigger.enabled = true;
		this.CurrentState = AttackState.Hold;
		this.Invoke("StopSwing", this.HoldTime);
		this.OnStartSwing.Invoke();
	}

	/** Stop the active portion of the attack */
	private void StopSwing()
	{
		this.HitTrigger.enabled = false;
		this.CurrentState = AttackState.WindDown;
		this.Invoke("Stop", this.WindDownTime);
		this.OnStopSwing.Invoke();
	}

	/** Disable this */
	private void Stop()
	{
		this.OnAttackDone.Invoke();
	}

	private void OnEnable()
	{
		this.HitTrigger.enabled = false;
		this.CurrentState = AttackState.WindUp;
		this.Invoke("StartSwing", this.WindUpTime);
	}

	private void OnDisable()
	{
		this.HitTrigger.enabled = false;
		this.CurrentState = AttackState.Inactive;
		this.CancelInvoke();
	}

	private void OnTriggerEnter2D(Collider2D coll)
	{
		this.OnHit.Invoke(coll);
	}
}