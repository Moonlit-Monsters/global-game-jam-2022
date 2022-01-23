using UnityEngine;
using UnityEngine.UI;
using Michsky.UI.ModernUIPack;

public class HealthMeter : MonoBehaviour
{
	[SerializeField]
	[Tooltip("THe slider to control")]
	private ProgressBar _slider;

	[SerializeField]
	[Tooltip("The health tracker to pull values from")]
	private Health _source;

	private void Awake()
	{
		this._slider.currentPercent = this._source.Maximum;
		this._slider.currentPercent = this._source.Maximum;
		this._source.OnChange.AddListener(this.UpdateMeter);
	}

	/** Update the value of the controlled slider with the given value
	\param value The value to pass to the slider
	*/
	private void UpdateMeter(float value)
	{
		this._slider.currentPercent = value;
	}
}