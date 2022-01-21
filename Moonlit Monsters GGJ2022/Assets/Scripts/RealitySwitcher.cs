using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class RealitySwitcher : MonoBehaviour
{
	[SerializeField]
	[Tooltip("The delusion tracker to listen from changes to")]
	private Delusion _sourceDelusion;

	[Header("Controlled Tags")]

	[SerializeField]
	[Tooltip("The tags to only enable when in reality")]
	private List<string> _realityTags;

	[SerializeField]
	[Tooltip("The tags to only enable when in delusion")]
	private List<string> _delusionTags;

	[Header("Events")]

	[SerializeField]
	[Tooltip("The events invoked when switching from delusion to reality")]
	public UnityEvent OnEnterReality;

	[SerializeField]
	[Tooltip("The events invoked when switching from reality to delusion")]
	public UnityEvent OnEnterDelusion;

	/** The singleton instance that should be used globally */
	public static RealitySwitcher Instance {get; private set;} = null;

	/** Whether the player is in reality */
	public bool IsReality {get; private set;} = true;

	/** Transition to reality if not there already */
	public void SwitchToReality()
	{
		if (!this.IsReality)
		{
			this.IsReality = true;
			this.Transition();
			this.OnEnterReality.Invoke();
		}
	}

	/** Transition to delusion if not there already */
	public void SwitchToDelusion()
	{
		if (this.IsReality)
		{
			this.IsReality = false;
			this.Transition();
			this.OnEnterDelusion.Invoke();
		}
	}

	/** Transition between reality and delusion */
	private void Transition()
	{
		this.ControlObjectsWithTags(this._realityTags, this.IsReality);
		this.ControlObjectsWithTags(this._delusionTags, !this.IsReality);
	}

	/** Set the active status of gameobjects with at least one of the given tags to the given status
	\param tags The tags to search for on game objects
	\param activate Whether the found game objects should be activated
	*/
	private void ControlObjectsWithTags(List<string> tags, bool activate)
	{
		List<GameObject> objects = new List<GameObject>();
		foreach (string tag in tags)
		{
			objects.AddRange(GameObject.FindGameObjectsWithTag(tag));
		}
		foreach (GameObject obj in objects)
		{
			obj.SetActive(activate);
		}
	}

	/** Set this as the singleton instance or destroy it if instance is already set */
	private void SetSinglton()
	{
		RealitySwitcher.Instance ??= this;
		if (RealitySwitcher.Instance != this)
		{
			Object.Destroy(this);
		}
		else
		{
			Object.DontDestroyOnLoad(this);
		}
	}

	/** Unset the singleton instance if this is it */
	private void UnsetSingleton()
	{
		if (RealitySwitcher.Instance == this)
		{
			RealitySwitcher.Instance = null;
		}
	}

	/** Switch to delusion if given value is greater than 0, oherwise switch to reality
	\param value The value to check whether to switch to reality or delusion
	*/
	private void OnSourceChange(float value)
	{
		if (value > 0)
		{
			this.SwitchToDelusion();
		}
		else
		{
			this.SwitchToReality();
		}
	}

	private void Awake()
	{
		this.SetSinglton();
		this._sourceDelusion.OnChange.AddListener(this.OnSourceChange);
	}

	private void OnDestroy()
	{
		this.UnsetSingleton();
	}
}