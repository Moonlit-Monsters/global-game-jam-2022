using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Collider2D))]
public class WanderAI : MonoBehaviour
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
	[Tooltip("The minimum radius this can wander from its start point")]
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
	[Tooltip("The maximum radius this can wander from its start point")]
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

	/** The point at which this started */
	public Vector2 StartPoint {get; private set;}

	/** The point this is currently wandering to */
	public Vector2 WanderPoint {get; private set;}

	/** The speed at which this is currently wandering */
	public float WanderSpeed {get; private set;}

	/** Whether this is currently resting */
	public bool IsResting {get; private set;} = false;

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
	#endif

	/** Schedule the next wander phase */
	public void StartWander()
	{
		this.Invoke("NextWander", Random.Range(this.MinimumTime, this.MaximumTime));
	}

	/** Set the next wander point and speed */
	public void NextWander()
	{
		Debug.Log("Wandering");
		this.IsResting = false;
		this.WanderPoint = this.StartPoint + Random.insideUnitCircle 
			* Random.Range(this.MinimumRadius, this.MaximumRadius);
		this.WanderSpeed = Random.Range(this.MinimumSpeed, this.MaximumSpeed);
	}

	/** Stop wandering */
	public void ForceRest()
	{
		Debug.Log("Resting");
		this.IsResting = true;
		this.Rb.velocity = Vector3.zero;
		this.WanderPoint = this.Rb.position;
		this.CancelInvoke("NextWander");
	}

	#if UNITY_EDITOR
		private void OnValidate()
		{
			this.ValidateMinMax("wander radius", this.MinimumRadius, this.MaximumRadius);
			this.ValidateMinMax("wander time", this.MinimumTime, this.MaximumTime);
			this.ValidateMinMax("wander speed", this.MinimumSpeed, this.MaximumSpeed);
		}
	#endif

	private void Awake()
	{
		this.StartPoint = this.Rb.position;
	}

	private void OnEnable()
	{
		this.StartWander();
	}

	private void OnDisable()
	{
		this.ForceRest();
	}

	private void Update()
	{
		if (!this.IsResting && (this.Rb.position - this.WanderPoint).sqrMagnitude < this.DistanceTolerance)
		{
			this.ForceRest();
			this.StartWander();
		}
	}

	private void OnTriggerEnter2D(Collider2D coll)
	{
		this.ForceRest();
		this.StartWander();
	}

	private void OnCollisionEnter2D(Collision2D coll)
	{
		this.ForceRest();
		this.StartWander();
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
		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.white;
			Gizmos.DrawWireSphere(UnityEditor.EditorApplication.isPlaying ? 
				this.StartPoint : (Vector2)this.transform.position, this.MaximumRadius);
			if (UnityEditor.EditorApplication.isPlaying)
			{
				Gizmos.color = Color.red;
				Gizmos.DrawSphere(this.WanderPoint, 1);
			}
		}
	#endif
}