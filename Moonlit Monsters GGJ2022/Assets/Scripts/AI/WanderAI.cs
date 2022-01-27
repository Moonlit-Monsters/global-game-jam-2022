using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class WanderAI : SlaveAIBase
{
	[Header("Components")]

	[SerializeField]
	[Tooltip("The rigidbody to control")]
	private Rigidbody2D _rb;

	public Rigidbody2D Rb
	{
		get
		{
			return this._rb;
		}
	}

	[Header("Wander Settings")]

	[SerializeField]
	[Tooltip("How quickly the center point drifts back to the start point")]
	[Min(0)]
	private float _centerDriftSpeed;

	public float CenterDriftSpeed
	{
		get
		{
			return this._centerDriftSpeed;
		}
	}

	[SerializeField]
	[Tooltip("The minimum distance this can be from its wander point before wandering somwhere else")]
	[Min(0)]
	private float _distanceTolerance = .01f;

	public float DistanceTolerance
	{
		get
		{
			return this._distanceTolerance;
		}
	}

	[SerializeField]
	[Tooltip("The minimum radius this can wander from its center point")]
	[Min(0)]
	private float _minimumRadius;

	public float MinimumRadius
	{
		get
		{
			return this._minimumRadius;
		}
	}

	[SerializeField]
	[Tooltip("The maximum radius this can wander from its center point")]
	[Min(0)]
	private float _maximumRadius;

	public float MaximumRadius
	{
		get
		{
			return this._maximumRadius;
		}
	}

	[SerializeField]
	[Tooltip("The minimum time this waits between movements in seconds")]
	[Min(0)]
	private float _minimumTime;

	public float MinimumTime
	{
		get
		{
			return this._minimumTime;
		}
	}

	[SerializeField]
	[Tooltip("The maximum time this waits between movements in seconds")]
	[Min(0)]
	private float _maximumTime;

	public float MaximumTime
	{
		get
		{
			return this._maximumTime;
		}
	}

	[SerializeField]
	[Tooltip("The minimum speed at which this wanders")]
	[Min(0)]
	private float _minimumSpeed;

	public float MinimumSpeed
	{
		get
		{
			return this._minimumSpeed;
		}
	}

	[SerializeField]
	[Tooltip("The maximum speed at which this wanders")]
	[Min(0)]
	private float _maximumSpeed;

	public float MaximumSpeed
	{
		get
		{
			return this._maximumSpeed;
		}
	}

	[Header("Events")]

	[SerializeField]
	[Tooltip("The methods invoked when this wanders")]
	private UnityEvent _onWander;

	public UnityEvent OnWander
	{
		get
		{
			return this._onWander;
		}
	}

	[SerializeField]
	[Tooltip("The methods invoked when this rests")]
	private UnityEvent _onRest;

	public UnityEvent OnRest
	{
		get
		{
			return this._onRest;
		}
	}

	/** The point around which this wanders */
	public Vector2 CenterPoint {get; private set;}

	/** The point this is currently wandering to */
	public Vector2 WanderPoint {get; private set;}

	/** The speed at which this is currently wandering */
	public float WanderSpeed {get; private set;}

	/** Whether this is currently resting */
	public bool IsResting {get; private set;} = false;

	/** Set the next wander point and speed */
	public void Wander()
	{
		this.IsResting = false;
		this.CenterPoint += Vector2.ClampMagnitude(this.Master.StartPoint - this.CenterPoint, this.CenterDriftSpeed * Time.deltaTime);
		this.WanderPoint = this.CenterPoint + Random.insideUnitCircle 
			* Random.Range(this.MinimumRadius, this.MaximumRadius);
		this.WanderSpeed = Random.Range(this.MinimumSpeed, this.MaximumSpeed);
		this.CancelInvoke("Wander");
		this.OnWander.Invoke();
	}

	/** Stop wandering */
	public void Rest()
	{
		this.IsResting = true;
		this.Rb.velocity = Vector3.zero;
		this.WanderPoint = this.Rb.position;
		this.CancelInvoke("Wander");
		this.Invoke("Wander", Random.Range(this.MinimumTime, this.MaximumTime));
		this.OnRest.Invoke();
	}

	public override void Initialise()
	{
		this.CenterPoint = this.Master.StartPoint;
	}

	private void OnEnable()
	{
		this.CenterPoint = this.Rb.position;
		this.Rest();
	}

	private void OnDisable()
	{
		this.Rest();
	}

	private void Update()
	{
		if (!this.IsResting && (this.Rb.position - this.WanderPoint).sqrMagnitude < this.DistanceTolerance)
		{
			this.Rest();
		}
	}

	private void OnTriggerEnter2D(Collider2D coll)
	{
		this.Rest();
	}

	private void OnCollisionEnter2D(Collision2D coll)
	{
		this.Rest();
	}

	private void FixedUpdate()
	{
		if (this.IsResting)
		{
			this.Rb.velocity = Vector3.zero;
		}
		else
		{
			this.Rb.AddForce(Vector2.ClampMagnitude(this.WanderPoint - this.Rb.position, 
				this.WanderSpeed * Time.fixedDeltaTime) - this.Rb.velocity);
		}
	}

	#if UNITY_EDITOR
		/** Log en error if the given maximum is lesser than the given minimum
		\param label The data label logged in the error
		\param min The minimum value to validate
		\param max The maximum value to validate
		*/
		private void ValidateMinMax(string label, float min, float max)
		{
			if (max < min)
			{
				Debug.LogError("Maximum " + label + " cannot be lesser than minimum " + label);
			}
		}
		
		private void OnValidate()
		{
			this.ValidateMinMax("wander radius", this.MinimumRadius, this.MaximumRadius);
			this.ValidateMinMax("wander time", this.MinimumTime, this.MaximumTime);
			this.ValidateMinMax("wander speed", this.MinimumSpeed, this.MaximumSpeed);
		}
		
		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.white;
			Gizmos.DrawWireSphere(UnityEditor.EditorApplication.isPlaying ? 
				this.CenterPoint : (Vector2)this.transform.position, this.MaximumRadius);
			if (UnityEditor.EditorApplication.isPlaying)
			{
				Gizmos.color = this.IsResting ? Color.green : Color.red;
				Gizmos.DrawSphere(this.WanderPoint, 1);
			}
		}
	#endif
}