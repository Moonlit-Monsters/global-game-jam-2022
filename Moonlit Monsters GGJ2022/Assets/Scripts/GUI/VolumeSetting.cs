using UnityEngine;
using UnityEngine.Audio;

public class VolumeSetting : MonoBehaviour
{
	[SerializeField]
	[Tooltip("The name of the linked volume float percentage setting")]
	private string _settingName;

	[SerializeField]
	[Tooltip("The audio mixer to control")]
	private AudioMixer _mixer;

	[SerializeField]
	[Tooltip("The name of the mixer parameter to control")]
	private string _mixerName;

	public void UpdateMixer()
	{
		float percent = PlayerPrefs.GetFloat(this._settingName, 100);
		float db = percent <= 0.01 ? -80f : Mathf.Log10(percent / 100f) * 20f;
		this._mixer.SetFloat(this._mixerName, db);
	}

	private void Awake()
	{
		this.UpdateMixer();
	}
}