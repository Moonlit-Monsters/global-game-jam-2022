using UnityEngine;
using UnityEngine.UI;

public class SettingToggle : MonoBehaviour
{
	[SerializeField]
	[Tooltip("The linked toggle")]
	private Toggle _toggle;

	[SerializeField]
	[Tooltip("The linked setting name")]
	private string _settingName;
	
	/** Update the setting with the given bool
	\param value the boolean to use
	*/
	public void UpdateSetting(bool value)
	{
		PlayerPrefs.SetInt(this._settingName, value ? 1 : 0);
	}

	private void Awake()
	{
		this._toggle.isOn = PlayerPrefs.GetInt(this._settingName, this._toggle.isOn ? 1 : 0) != 0;
		this._toggle.onValueChanged.AddListener(this.UpdateSetting);
	}

	private void OnDestroy()
	{
		PlayerPrefs.Save();
	}
}