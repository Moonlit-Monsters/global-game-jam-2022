using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    [SerializeField]
	[Tooltip("The audio clips used for footsteps")]
	private AudioClip[] _footstepSounds;

	public AudioClip[] FootstepSounds
	{
		get
		{
			return this._footstepSounds;
		}
	}

    [SerializeField]
	[Tooltip("The audiosource used for playing clips")]
	private AudioSource _playerAudioSource;

	public AudioSource PlayerAudioSource
	{
		get
		{
			return this._playerAudioSource;
		}
	}

    private void Awake()
    {
        _playerAudioSource ??= this.GetComponent<AudioSource>();
    }

    private int PickClip(AudioClip[] clips)
    {
        return Random.Range(0, clips.Length - 1);
    }

    public void PlayFootsteps()
    {
        PlayerAudioSource.PlayOneShot(FootstepSounds[PickClip(FootstepSounds)], 1f);
    }
}
