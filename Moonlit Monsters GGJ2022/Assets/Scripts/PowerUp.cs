using UnityEngine;
using UnityEngine.Events;

public class PowerUp : MonoBehaviour
{
	[Header("Delusion Settings")]

	[SerializeField]
	[Tooltip("The amount to increase delusion by")]
	[Min(0)]
	private float _value = 50;

	public float Value
	{
		get
		{
			return this._value;
		}
	}

	[SerializeField]
	[Tooltip("The time before Power-up respawns")]
	[Min(5f)]
	private float _respawnTime = 60f;

	public float RespawnTime
	{
		get
		{
			return this._respawnTime;
		}
	}

	[Header("Events")]

	[SerializeField]
	[Tooltip("The events invoked when this is collected")]
	public UnityEvent OnCollect;

	/** Collect this power up with the given collector
	\param collector The delusion tracker to modify
	*/
	public void Collect(Delusion collector)
	{
		if (collector != null)
		{
			collector.Value += this.Value;
		}
		this.OnCollect.Invoke();
		this.gameObject.SetActive(false);
		Invoke("Reactivate", RespawnTime);
	}

	private void Reactivate()
	{
		this.gameObject.SetActive(true);
	}

	private void OnTriggerEnter2D(Collider2D coll)
	{
		this.Collect(coll.gameObject.GetComponent<Delusion>());
		coll.gameObject.GetComponent<DelusionEffects>()?.StopCountdownTrack();
	}
}