using UnityEngine;
using UnityEngine.UI;

public class SettingSlider : MonoBehaviour
{
	[SerializeField]
	[Tooltip("The linked slider")]
	private Slider _slider;

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
		this._slider.value = PlayerPrefs.GetFloat(this._settingName, this._slider.value);
		this._slider.onValueChanged.AddListener(this.UpdateSetting);
	}

	private void OnDestroy()
	{
		PlayerPrefs.Save();
	}
}