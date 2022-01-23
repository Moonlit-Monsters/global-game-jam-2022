using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using System.Collections.Generic;

[RequireComponent(typeof(Collider2D))]
public class PursueAI : MonoBehaviour
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

	[SerializeField]
	[Tooltip("The circle trigger to use for detecting targets")]
	private CircleCollider2D _detectTrigger;

	public CircleCollider2D DetectTrigger
	{
		get
		{
			return this._detectTrigger;
		}
	}

	[SerializeField]
	[Tooltip("The nav mesh agent used to path find")]
	private NavMeshAgent _nav;

	public NavMeshAgent Nav
	{
		get
		{
			return this._nav;
		}
	}

	[Header("Persue Settings")]

	[SerializeField]
	[Tooltip("The layer mask used to find targets")]
	private LayerMask _targetMask;

	public LayerMask TargetMask
	{
		get
		{
			return this._targetMask;
		}
	}

	[SerializeField]
	[Tooltip("The layer mask used to determine line of sight")]
	private LayerMask _sightMask;

	public LayerMask SightMask
	{
		get
		{
			return this._sightMask;
		}
	}

	[SerializeField]
	[Tooltip("The minimum number of seconds this will wait before returning to its start position after losing its target")]
	[Min(0)]
	private float _minimumWait;

	public float MinimumWait
	{
		get
		{
			return this._minimumWait;
		}
	}

	[SerializeField]
	[Tooltip("The maximum number of seconds this will wait before returning to its start position after losing its target")]
	[Min(0)]
	private float _maximumWait;

	public float MaximumWait
	{
		get
		{
			return this._maximumWait;
		}
	}

	[Header("Events")]

	[SerializeField]
	[Tooltip("The methods invoked when this persues a new target")]
	private UnityEvent _onPersue;

	public UnityEvent OnPersue
	{
		get
		{
			return this._onPersue;
		}
	}

	[SerializeField]
	[Tooltip("The methods invoked when this loses a target")]
	private UnityEvent _onWait;

	public UnityEvent OnWait
	{
		get
		{
			return this._onWait;
		}
	}

	[SerializeField]
	[Tooltip("The methods invoked when this returns its start position")]
	private UnityEvent _onReturn;

	public UnityEvent OnReturn
	{
		get
		{
			return this._onReturn;
		}
	}

	/** The position at which this started */
	public Vector2 StartPosition {get; private set;}

	/** The target this is currently persuing */
	public Transform CurrentTarget {get; private set;}

	public enum Status
	{
		Returning,
		Waiting,
		Persuing
	}

	/** The current state of the AI */
	public Status State {get; private set;}

	/** Cast out for a new target
	\return The found target
	*/
	private Transform FindNewTarget()
	{
		if (this.CurrentTarget == null)
		{
			ContactFilter2D filter = new ContactFilter2D();
			filter.SetLayerMask(this.TargetMask);
			List<Collider2D> hits = new List<Collider2D>();
			this.DetectTrigger.OverlapCollider(filter, hits);
			foreach (Collider2D col in hits)
			{
				if (this.IsTargetInSight(col.transform))
				{
					return col.transform;
				}
			}
		}
		return null;
	}

	/** Check if the given target is in sight 
	\param target The target to sight check
	\return True when the given target is in line of sight
	*/
	private bool IsTargetInSight(Transform target)
	{
		if (target == null)
		{
			return false;
		}
		Vector2 diff = (Vector2)target.position - this.Rb.position;
		if (diff.sqrMagnitude > this.DetectTrigger.radius * this.DetectTrigger.radius)
		{
			return false;
		}
		RaycastHit2D hit = Physics2D.Raycast(this.Rb.position, diff, diff.magnitude, this.SightMask);
		Debug.DrawLine(this.Rb.position, hit.point);
		return hit.transform == target;
	}

	/** Wait in its current position for a new target before returning to its start position */
	public void Wait()
	{
		this.CurrentTarget = null;
		this.State = Status.Waiting;
		this.Nav.ResetPath();
		this.Rb.velocity = Vector2.zero;
		this.CancelInvoke("Return");
		this.Invoke("Return", Random.Range(this.MinimumWait, this.MaximumWait));
		this.OnWait.Invoke();
	}

	/** Return to its starting position */
	public void Return()
	{
		this.CurrentTarget = null;
		this.State = Status.Returning;
		this.Nav.SetDestination(this.StartPosition);
		this.CancelInvoke("Return");
		this.OnReturn.Invoke();
	}

	/** Persue the given target
	\param target The transform to persue
	*/
	public void Persue(Transform target)
	{
		if (target != null)
		{
			this.CurrentTarget = target;
			this.State = Status.Persuing;
			this.Nav.SetDestination(target.position);
			this.CancelInvoke("Return");
			this.OnPersue.Invoke();
		}
	}

	private void Awake()
	{
		this.Nav.updateRotation = false;
		this.Nav.updateUpAxis = false;
		this.StartPosition = this.Rb.position;
	}

	private void OnTriggerEnter2D(Collider2D coll)
	{
		if (this.CurrentTarget == null && (coll.gameObject.layer & this.TargetMask) != 0)
		{
			this.Persue(coll.transform);
		}
	}

	private void OnTriggerExit2D(Collider2D coll)
	{
		if (coll.transform == this.CurrentTarget)
		{
			this.Wait();
		}
	}

	private void FixedUpdate()
	{
		if (this.State == Status.Persuing)
		{
			if (this.IsTargetInSight(this.CurrentTarget))
			{
				this.Nav.SetDestination(this.CurrentTarget.position);
			}
			else
			{
				this.Wait();
			}
		}
		else
		{
			this.Persue(this.FindNewTarget());
		}
	}

	private void OnEnable()
	{
		this.Return();
	}

	private void OnDisable()
	{
		this.Wait();
		this.CancelInvoke();
	}

	#if UNITY_EDITOR
		private void OnValidate()
		{
			if (this.MaximumWait < this.MinimumWait)
			{
				Debug.LogError("Maximum wait time cannot be lesser than minimum wait time");
			}
			if (this.DetectTrigger != null)
			{
				if (this.DetectTrigger.gameObject != this.gameObject)
				{
					Debug.LogError("Detect trigger must be on same game object");
				}
				if (!this.DetectTrigger.isTrigger)
				{
					Debug.LogError("Detect trigger must be a trigger");
				}
				if (this.Nav != null && this.DetectTrigger.radius < this.Nav.stoppingDistance)
				{
					Debug.LogError("Detect trigger radius cannot be lesser than stop radius");
				}
			}
		}

		/** Draw the detection radius gizmo */
		private void DrawDetectGizmo()
		{
			if (this.Rb != null && this.DetectTrigger != null)
			{
				Gizmos.color = Color.white;
				Gizmos.DrawWireSphere(this.Rb.position, this.DetectTrigger.radius);
			}
		}

		/** Draw the stopping distance gizmo */
		private void DrawStopGizmo()
		{
			if (this.Rb != null && this.Nav != null)
			{
				Gizmos.color = Color.red;
				Gizmos.DrawWireSphere(this.Rb.position, this.Nav.stoppingDistance);
			}
		}

		/** Draw the start position gizmo */
		private void DrawReturnGizmo()
		{
			if (UnityEditor.EditorApplication.isPlaying && this.State == Status.Returning)
			{
				Gizmos.color = Color.green;
				Gizmos.DrawSphere(this.StartPosition, 1f);
			}
		}

		/** Draw the target gizmo */
		private void DrawTargetGizmo()
		{
			if (this.CurrentTarget != null)
			{
				Gizmos.color = Color.red;
				Gizmos.DrawWireSphere(this.CurrentTarget.position, 1f);
			}
		}

		private void OnDrawGizmosSelected()
		{
			this.DrawReturnGizmo();
			this.DrawDetectGizmo();
			this.DrawStopGizmo();
			this.DrawTargetGizmo();
		}
	#endif
}