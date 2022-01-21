using UnityEngine;
using UnityEngine.UI;

public class DelusionMeter : MonoBehaviour
{
	[SerializeField]
	[Tooltip("The silder to control")]
	private Slider _slider;

	[SerializeField]
	[Tooltip("The delusion tracker to source the value from")]
	private Delusion _source;

	private void Awake()
	{
		this._slider.minValue = 0;
		this._slider.maxValue = this._source.Maximum;
		this._source.OnChange.AddListener(this.UpdateMeter);
	}

	/** Update the attached slider with the given value
	\param value The value to pass to the slider
	*/
	private void UpdateMeter(float value)
	{
		Debug.Log("Delusioin Meter: " + value.ToString());
		this._slider.value = value;
	}
}