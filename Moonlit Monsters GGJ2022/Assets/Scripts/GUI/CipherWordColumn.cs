using System.Collections.Generic;
using UnityEngine;

public class CipherWordColumn : MonoBehaviour
{
	[Header("Components")]

	[SerializeField]
	[Tooltip("Where the ciphertext should be built")]
	private WordBuilder _ciphertextBuilder;

	public WordBuilder CiphertextBuilder
	{
		get
		{
			return this._ciphertextBuilder;
		}
	}

	[SerializeField]
	[Tooltip("Where the key should be built")]
	private WordBuilder _keyBuilder;

	public WordBuilder KeyBuilder
	{
		get
		{
			return this._keyBuilder;
		}
	}

	[SerializeField]
	[Tooltip("Where the plaintext should be built each step")]
	private WordBuilder _plaintextBuilder;

	public WordBuilder PlaintextBuilder
	{
		get
		{
			return this._plaintextBuilder;
		}
	}

	[SerializeField]
	[Tooltip("The transform to move over the selected column")]
	private RectTransform _selector;

	public RectTransform Selector
	{
		get
		{
			return this._selector;
		}
	}

	[SerializeField]
	[Tooltip("The decipher machine to pull information from and respond to")]
	private DecipherMachine _decipherer;

	public DecipherMachine Decipherer
	{
		get
		{
			return this._decipherer;
		}
	}

	[Header("Dimension Settings")]

	[SerializeField]
	[Tooltip("The size of the letter cells")]
	[Min(0)]
	private float _cellSize;

	public float CellSize
	{
		get
		{
			return this._cellSize;
		}
	}

	/** Build the ciphertext from the cipher group currently being deciphered */
	public void BuildCiphertext()
	{
		this.CiphertextBuilder.SetWord(this.Decipherer.CurrentGroup.Ciphertext);
	}

	/** Build the key from the cipher group currently being deciphered */
	public void BuildKey()
	{
		this.KeyBuilder.SetWord(this.Decipherer.CurrentGroup.Key);
	}

	/** Build the plaintext from the cipher group currently begin deciphered but only up to the current decipher letter */
	public void BuildPlaintext()
	{
		char[] plaintext = this.Decipherer.CurrentGroup.Plaintext.ToCharArray();
		for (int i = this.Decipherer.CurrentLetterIndex + 1; i < plaintext.Length; ++i)
		{
			plaintext[i] = '_';
		}
		this.PlaintextBuilder.SetWord(new string(plaintext));
	}

	/** Update the position of the selector based on the current decipher index */
	public void UpdateSelector()
	{
		this.Selector.rect.Set(this.Decipherer.CurrentLetterIndex * this.CellSize, 
			this.Selector.rect.y, 
			this.Selector.rect.width, 
			this.Selector.rect.height);
	}

	private void Awake()
	{
		this.Decipherer.OnStart.AddListener(this.BuildCiphertext);
		this.Decipherer.OnStart.AddListener(this.BuildKey);
		this.Decipherer.OnStep.AddListener(this.BuildPlaintext);
		this.Decipherer.OnStep.AddListener(this.UpdateSelector);
	}
}
