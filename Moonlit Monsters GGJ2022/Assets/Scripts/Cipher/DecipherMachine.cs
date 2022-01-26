using UnityEngine;
using UnityEngine.Events;

public class DecipherMachine : MonoBehaviour
{
	[SerializeField]
	[Tooltip("The number of seconds between decipher steps")]
	[Min(0)]
	private float _stepDelay;

	public float StepDelay
	{
		get
		{
			return this._stepDelay;
		}
	}

	[Header("Events")]

	[SerializeField]
	[Tooltip("The methods invoked when this cannot decipher anything")]
	private UnityEvent _onNothingToDecipher;

	public UnityEvent OnNothingToDecipher
	{
		get
		{
			return this._onNothingToDecipher;
		}
	}

	[SerializeField]
	[Tooltip("The methods invoked when this successfully starts deciphering a password")]
	private UnityEvent _onStart;

	public UnityEvent OnStart
	{
		get
		{
			return this._onStart;
		}
	}

	[SerializeField]
	[Tooltip("The methods invoked each step of the deciphering process")]
	private UnityEvent _onStep;

	public UnityEvent OnStep
	{
		get
		{
			return this._onStep;
		}
	}

	[SerializeField]
	[Tooltip("The methods invoked when this successfully completes deciphering a password")]
	private UnityEvent _onComplete;

	public UnityEvent OnComplete
	{
		get
		{
			return this._onComplete;
		}
	}

	/** The cipher group this is currently deciphering */
	public CipherGroup CurrentGroup {get; private set;} = null;

	/** The index of the letter that was just processed */
	public int CurrentLetterIndex {get; private set;} = -1;

	/** Return a cipher group that is ready to be deciphered
	\return A cipher group that can be deciphered
	*/
	private CipherGroup FindValidGroup()
	{
		foreach (CipherGroup group in CipherMaster.Instance.Groups)
		{
			if (!group.IsDeciphered && group.PasswordItem.IsCollected && group.KeyItem.IsCollected)
			{
				return group;
			}
		}
		return null;
	}

	/** Start deciphering a password */
	public void StartDecipher()
	{
		if (this.CurrentGroup == null)
		{
			this.CurrentGroup = this.FindValidGroup();
			if (this.CurrentGroup == null)
			{
				this.OnNothingToDecipher.Invoke();
			}
			else
			{
				this.CurrentLetterIndex = -1;
				this.Invoke("NextStep", this.StepDelay);
				this.OnStart.Invoke();
			}
		}
	}

	/** Perform the next deciphering step for the current cipher group */
	private void NextStep()
	{
		++this.CurrentLetterIndex;
		if (this.CurrentLetterIndex < this.CurrentGroup.Plaintext.Length)
		{
			this.Invoke("NextStep", this.StepDelay);
			this.OnStep.Invoke();
		}
		else
		{
			this.CompleteDecipher();
		}
	}

	/** Complete the deciphering process for the current cipher group */
	public void CompleteDecipher()
	{
		if (this.CurrentGroup != null)
		{
			this.CurrentGroup.IsDeciphered = true;
			this.CurrentGroup = null;
			this.OnComplete.Invoke();
		}
	}
}