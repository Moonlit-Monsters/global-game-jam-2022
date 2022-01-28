using UnityEngine;

public class PaneSwitcher : MonoBehaviour
{
	[Header("Panes")]

	[SerializeField]
	[Tooltip("The pane that will be disabled")]
	private GameObject _toDisable;

	public GameObject ToDisable
	{
		get
		{
			return this._toDisable;
		}
	}

	[SerializeField]
	[Tooltip("The pane that will be enabled")]
	private GameObject _toEnable;

	public GameObject ToEnable
	{
		get
		{
			return this._toEnable;
		}
	}

	/** Disable the specified pane and enable the specified pane */
	public void SwitchPanes()
	{
		this.ToDisable?.SetActive(false);
		this.ToEnable?.SetActive(true);
	}
}