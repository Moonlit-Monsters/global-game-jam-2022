using UnityEngine;
using UnityEngine.UI;
using Michsky.UI.ModernUIPack;

public class LoadingBar : MonoBehaviour
{
	[SerializeField]
	[Tooltip("THe slider to control")]
	private ProgressBar _slider;

	private void Awake()
	{

	}

	private void Update()
	{
		this._slider.currentPercent = SceneLoader.LoadingOperation.progress;
	}
}