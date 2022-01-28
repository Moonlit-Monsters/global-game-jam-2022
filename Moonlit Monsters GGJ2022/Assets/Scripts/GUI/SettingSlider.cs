using UnityEngine;
using UnityEngine.UI;
using Michsky.UI.ModernUIPack;

public class SettingSlider : MonoBehaviour
{
	[SerializeField]
	[Tooltip("The linked slider")]
	private RadialSlider _slider;

	[SerializeField]
	[Tooltip("The name of the linked setting")]
	private string _settingName;

	/** Update the setting with the given float
	\param value 
	*/
	public void UpdateSetting(float value)
	{
		PlayerPrefs.SetFloat(this._settingName, value);
	}

	private void Awake()
	{
		this._slider.currentValue = PlayerPrefs.GetFloat(this._settingName, this._slider.currentValue);
	}

	private void OnDestroy()
	{
		PlayerPrefs.Save();
	}
}