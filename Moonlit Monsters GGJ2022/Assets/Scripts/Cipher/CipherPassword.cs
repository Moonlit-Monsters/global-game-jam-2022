using UnityEngine;
using UnityEngine.Events;

public class CipherPassword : CipherPieceBase
{
	[SerializeField]
	[Tooltip("The methods invoked when this is collected")]
	private UnityEvent _onCollect;

	public UnityEvent OnCollect
	{
		get
		{
			return this._onCollect;
		}
	}

	/** Whether this has been collected */
	public bool IsCollected {get; private set;} = false;

	/** Collect this password */
	public void Collect()
	{
		if (!this.IsCollected)
		{
			this.IsCollected = true;
			this.gameObject.SetActive(false);
			this.OnCollect.Invoke();
		}
	}

	private void OnTriggerEnter2D(Collider2D coll)
	{
		this.Collect();
	}
}