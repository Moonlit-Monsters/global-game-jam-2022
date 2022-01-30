using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class SoundRandomiser : MonoBehaviour
{
	[SerializeField]
	[Tooltip("The audiosource used for playing clips")]
	private AudioSource _audioSource;

	public AudioSource AudioSource
	{
		get
		{
			return this._audioSource;
		}
	}

	[SerializeField]
	[Tooltip("The audio clips used for footsteps")]
	private List<AudioClip> _clips;

	public ReadOnlyCollection<AudioClip> Clips
	{
		get
		{
			return this._clips.AsReadOnly();
		}
	}

	private void Awake()
	{
		this._audioSource ??= this.GetComponent<AudioSource>();
	}

	private int PickClip(ICollection<AudioClip> clips)
	{
		return Random.Range(0, clips.Count - 1);
	}

	public void PlayClip()
	{
		if (this.isActiveAndEnabled & !this._audioSource.isPlaying)
		{
			this.AudioSource.PlayOneShot(this.Clips[this.PickClip(this.Clips)], 1f);
		}
	}
}
