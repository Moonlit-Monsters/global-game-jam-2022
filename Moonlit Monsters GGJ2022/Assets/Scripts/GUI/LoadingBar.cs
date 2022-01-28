using UnityEngine;
using UnityEngine.UI;

public class LoadingBar : MonoBehaviour
{
	[SerializeField]
	[Tooltip("The slider to control")]
	private Slider _slider;

	private void Awake()
	{
		this._slider.minValue = 0;
		this._slider.maxValue = 1;
	}

	private void Update()
	{
		this._slider.value = SceneLoader.LoadingOperation.progress;
	}
}