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

	private void Update()
	{
		this._slider.value = this._source.Value / this._source.Maximum;
	}
}