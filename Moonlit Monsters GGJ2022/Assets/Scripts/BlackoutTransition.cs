using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class BlackoutTransition : MonoBehaviour
{
    [SerializeField]
	[Tooltip("The image component on this Game Object")]
	private Image _blackoutImage;

	public Image BlackoutImage
	{
		get
		{
			return this._blackoutImage;
		}
	}

    [SerializeField]
	[Tooltip("The speed at which the screen fades to black")]
    private float _fadeOutRate = 5f;

    public float FadeOutRate
    {
        get
        {
            return this._fadeOutRate;
        }
    }

    [SerializeField]
	[Tooltip("The speed at which the screen fades from black")]
    private float _fadeInRate = 2f;

    public float FadeInRate
    {
        get
        {
            return this._fadeInRate;
        }
    }
	[Header("Events")]

	[SerializeField]
	[Tooltip("The methods invoked when this is done fading out")]
	private UnityEvent _onFadeOutDone;

	public UnityEvent OnFadeOutDone
	{
		get
		{
			return this._onFadeOutDone;
		}
	}

	[SerializeField]
	[Tooltip("The methods invoked when this is done fading in")]
	private UnityEvent _onFadeInDone;

	public UnityEvent OnFadeInDone
	{
		get
		{
			return this._onFadeInDone;
		}
	}

	public enum TransitionState
	{
		Inactive,
		FadeOut,
		FadeIn
	}

	public TransitionState CurrentState {get; private set;}

    private float _targetAlpha;


    private void Awake()
    {
        _blackoutImage ??= this.GetComponent<Image>();
        _targetAlpha = BlackoutImage.color.a;
    }

    private void Update()
    {
        float alpha = Mathf.Lerp(this.BlackoutImage.color.a, this._targetAlpha, 
			(this.CurrentState == TransitionState.FadeIn 
				? (1f / this.FadeInRate) / Mathf.Abs(this.BlackoutImage.color.a - this._targetAlpha) 
				: (1f / this.FadeOutRate) / Mathf.Abs(this.BlackoutImage.color.a - this._targetAlpha)) 
			* Time.deltaTime);
        alpha = (Mathf.Abs(alpha - this._targetAlpha) < 0.005) ? this._targetAlpha : alpha;
        BlackoutImage.color = new Color(0, 0, 0, alpha);
		this.CheckEvents();
    }

	/** Invoke any required events */
	private void CheckEvents()
	{
		if (this._targetAlpha == this.BlackoutImage.color.a)
		{
			this.CurrentState = TransitionState.Inactive;
			switch (this.CurrentState)
			{
				case TransitionState.FadeIn:
					this.OnFadeInDone.Invoke();
					break;
				case TransitionState.FadeOut:
					this.OnFadeOutDone.Invoke();
					break;
			}
		}
	}

    public void FadeIn()
    {
		this.CurrentState = TransitionState.FadeIn;
        _targetAlpha = 0f;
    }

    public void FadeOut()
    {
		this.CurrentState = TransitionState.FadeOut;
        _targetAlpha = 1f;
    }
}
