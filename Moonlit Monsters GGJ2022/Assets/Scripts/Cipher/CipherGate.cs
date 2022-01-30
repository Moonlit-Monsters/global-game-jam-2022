using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class CipherGate : CipherPieceBase
{
	[Header("Components")]

	[SerializeField]
	[Tooltip("The collider to disable when this gate opens")]
	private Collider2D _gateCollider;

	public Collider2D GateCollider
	{
		get
		{
			return this._gateCollider;
		}
	}

	[SerializeField]
	[Tooltip("The trigger that opens the gate")]
	private Collider2D _openTrigger;

	public Collider2D OpenTrigger
	{
		get
		{
			return this._openTrigger;
		}
	}

	[SerializeField]
	private Animator _animator;

	[Header("Animation Settings")]

	[SerializeField]
	[Tooltip("The animation boolean to set to true when this opens")]
	private string _boolName;

	[Header("Events")]

	[SerializeField]
	[Tooltip("The methods invoked when this is opened")]
	private UnityEvent _onOpen;

	public UnityEvent OnOpen
	{
		get
		{
			return this._onOpen;
		}
	}

	/** Whether this gate is open */
	public bool IsOpen {get; private set;} = false;

	/** Open this gate */
	public void Open()
	{
		if (!this.IsOpen && this.Group.IsDeciphered)
		{
			this.IsOpen = true;
			this.GateCollider.enabled = false;
			this._animator.SetBool(this._boolName, true);
			this.OnOpen.Invoke();
		}
	}

	private void OnTriggerEnter2D(Collider2D coll)
	{
		this.Open();
	}

	private void Update()
	{
		this._animator.SetBool(this._boolName, this.IsOpen);
	}

	#if UNITY_EDITOR
	private void OnValidate()
	{
		if (this.OpenTrigger != null)
		{
			if (!this.OpenTrigger.isTrigger)
			{
				Debug.LogError("Open trigger must be a trigger");
			}
			if (this.OpenTrigger.gameObject != this.gameObject)
			{
				Debug.LogError("Open trigger must be on same game object as cipher gate script");
			}
		}
	}
	#endif
}