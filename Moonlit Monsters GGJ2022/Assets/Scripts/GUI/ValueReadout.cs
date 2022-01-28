using UnityEngine;
using UnityEngine.UI;

public class ValueReadout : MonoBehaviour
{
	[SerializeField]
	[Tooltip("The text to control")]
	private Text _text;

	[SerializeField]
	[Tooltip("The prefix to prepend")]
	private string _prefix = "";

	[SerializeField]
	[Tooltip("The suffix to append")]
	private string _suffix = "";

	/** Update the controlled text with the given value
	\param value The value to read out
	*/
	public void UpdateText(object value)
	{
		this._text.text = this._prefix + value.ToString() + this._suffix;
	}

	public void UpdateFloatText(float value)
	{
		this.UpdateFloatText(value, 0);
	}

	/** Update the controlled text with the given float
	\param value The float to read out
	*/
	public void UpdateFloatText(float value, int decimals)
	{
		float factor = Mathf.Pow(10, decimals);
		this.UpdateText(System.Math.Truncate(value * factor) / factor);
	}
}