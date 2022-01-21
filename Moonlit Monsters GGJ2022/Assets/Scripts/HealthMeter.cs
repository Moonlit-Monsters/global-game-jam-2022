using UnityEngine;
using UnityEngine.UI;

public class HealthMeter : MonoBehaviour
{
	[SerializeField]
	[Tooltip("THe slider to control")]
	private Slider _slider;

	[SerializeField]
	[Tooltip("The health tracker to pull values from")]
	private Health _source;

	private void Awake()
	{
		this._slider.minValue = 0;
		this._slider.maxValue = this._source.Maximum;
		this._source.OnChange.AddListener(this.UpdateMeter);
	}

	/** Update the value of the controlled slider with the given value
	\param value The value to pass to the slider
	*/
	private void UpdateMeter(float value)
	{
		this._slider.value = value;
	}
}