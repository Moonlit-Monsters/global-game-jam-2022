using UnityEngine;
using UnityEngine.Events;

public class CipherGate : CipherPieceBase
{
	[SerializeField]
	[Tooltip("The collider to disable when this gate opens")]
	private Collider2D _collider;

	public Collider2D Collider
	{
		get
		{
			return this._collider;
		}
	}

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
		if (!this.IsOpen)
		{
			this.IsOpen = true;
			this.Collider.enabled = false;
			this.OnOpen.Invoke();
		}
	}
}