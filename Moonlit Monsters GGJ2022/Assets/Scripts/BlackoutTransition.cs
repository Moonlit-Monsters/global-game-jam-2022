using UnityEngine;
using UnityEngine.UI;

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

    private float _targetAlpha;

    private void Awake()
    {
        _blackoutImage ??= this.GetComponent<Image>();
        _targetAlpha = BlackoutImage.color.a;
    }

    private void Update()
    {
        //float alpha = (_targetAlpha == 0f ? FadeInRate : FadeOutRate) * Time.deltaTime * ((_targetAlpha - BlackoutImage.color.a) > 0f ? 1f : -1f);
        float alpha = Mathf.Lerp(BlackoutImage.color.a, _targetAlpha, (_targetAlpha == 0f ? (1f / FadeInRate) / Mathf.Abs(BlackoutImage.color.a - _targetAlpha) : (1f / FadeOutRate) / Mathf.Abs(BlackoutImage.color.a - _targetAlpha)) * Time.deltaTime);
        alpha = ((alpha - _targetAlpha) > -0.005 && (alpha - _targetAlpha) < 0.005) ? _targetAlpha : alpha;
        BlackoutImage.color = new Color(0, 0, 0, alpha);
    }

    public void FadeIn()
    {
        _targetAlpha = 0f;
    }

    public void FadeOut()
    {
        _targetAlpha = 1f;
    }
}
