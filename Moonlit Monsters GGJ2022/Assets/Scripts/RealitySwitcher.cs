using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Collections.ObjectModel;

public class RealitySwitcher : MonoBehaviour
{
	[SerializeField]
	[Tooltip("The delusion tracker to listen from changes to")]
	private Delusion _sourceDelusion;

	[Header("Controlled Tags")]

	[SerializeField]
	[Tooltip("The tags to only enable when in reality")]
	private List<string> _realityTags;

	public ReadOnlyCollection<string> RealityTags
	{
		get
		{
			return this._realityTags.AsReadOnly();
		}
	}

	[SerializeField]
	[Tooltip("The tags to only enable when in delusion")]
	private List<string> _delusionTags;

	public ReadOnlyCollection<string> DelusionTags
	{
		get
		{
			return this._delusionTags.AsReadOnly();
		}
	}

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

	private List<GameObject> _realityObjects;
	/** The game objects that are a part of reality */
	public ReadOnlyCollection<GameObject> RealityObjects
	{
		get
		{
			return this._realityObjects.AsReadOnly();
		}
	}

	private List<GameObject> _delusionObjects;
	/** The game objects that are part of delusion */
	public ReadOnlyCollection<GameObject> DelusionObjects
	{
		get
		{
			return this._delusionObjects.AsReadOnly();
		}
	}

	/** Transition to reality if not there already */
	public void SwitchToReality()
	{
		if (!this.IsReality)
		{
			Debug.Log("Entering reality");
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
			Debug.Log("Entering delusion");
			this.IsReality = false;
			this.Transition();
			this.OnEnterDelusion.Invoke();
		}
	}

	/** Transition between reality and delusion */
	private void Transition()
	{
		this.ControlObjects(this.RealityObjects, this.IsReality);
		this.ControlObjects(this.DelusionObjects, !this.IsReality);
	}

	/** Set the active status of gameobjects with at least one of the given tags to the given status
	\param objects The game objects to control
	\param activate Whether the found game objects should be activated
	*/
	private void ControlObjects(ICollection<GameObject> objects, bool activate)
	{
		foreach (GameObject obj in objects)
		{
			Debug.Log((activate ? "Activating: " : "Deactivating: ") + obj.name);
			obj.SetActive(activate);
		}
	}

	/** Find all the game objects with one of the given tags
	\param tags The tags to search for
	*/
	private List<GameObject> FindObjectsWithTags(ICollection<string> tags)
	{
		List<GameObject> objects = new List<GameObject>();
		foreach (string tag in tags)
		{
			objects.AddRange(GameObject.FindGameObjectsWithTag(tag));
		}
		return objects;
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
		this._realityObjects = this.FindObjectsWithTags(this.RealityTags);
		this._delusionObjects = this.FindObjectsWithTags(this.DelusionTags);
		this.Transition();
	}

	private void OnDestroy()
	{
		this.UnsetSingleton();
	}
}