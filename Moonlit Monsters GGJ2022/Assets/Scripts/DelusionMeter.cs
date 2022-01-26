using UnityEngine;
using UnityEngine.UI;
using Michsky.UI.ModernUIPack;

public class DelusionMeter : MonoBehaviour
{
	[SerializeField]
	[Tooltip("The silder to control")]
	private ProgressBar _slider;

	[SerializeField]
	[Tooltip("The delusion tracker to source the value from")]
	private Delusion _source;

	private void Awake()
	{
		this._slider.currentPercent = this._source.Maximum;
		this._slider.currentPercent = this._source.Maximum;
		this._source.OnChange.AddListener(this.UpdateMeter);
	}

	/** Update the attached slider with the given value
	\param value The value to pass to the slider
	*/
	private void UpdateMeter(float value)
	{
		this._slider.currentPercent = value / _source.Maximum * 100f;
	}
}