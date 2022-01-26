using UnityEngine;
using UnityEngine.Events;

public class DelusionEffects : MonoBehaviour
{
    [SerializeField]
	[Tooltip("The delusion script to get relevant variables from")]
	private Delusion _delusion;

    [SerializeField]
	[Tooltip("The blackout script to invoke certain methods from")]
	private BlackoutTransition _blackoutTransition;

    [SerializeField]
	[Tooltip("The blackout script to invoke certain methods from")]
	private AudioSource _countdownTrack;

    private void Awake()
    {
        _delusion.OnChange.AddListener(OnDelusionChange);
    }

    public void OnDelusionChange(float value)
    {
        if (!_countdownTrack.isPlaying && value > 0.01f && (value / _delusion.DecayRate) < _countdownTrack.clip.length)
        {
            _countdownTrack.Play();
        }

        if ((value / _delusion.DecayRate) < _blackoutTransition.FadeOutRate && value > 0.01f)
        {
            _blackoutTransition.FadeOut();
        }
        else
        {
            _blackoutTransition.FadeIn();
        }
    }

    public void StopCountdownTrack()
    {
        _countdownTrack.Stop();
    }
}
