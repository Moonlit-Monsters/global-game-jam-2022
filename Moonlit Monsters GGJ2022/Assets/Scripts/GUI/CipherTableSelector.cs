using System.Collections.Generic;
using UnityEngine;

public class CipherTableSelector : MonoBehaviour
{
	[Header("Components")]

	[SerializeField]
	[Tooltip("The rect transform this controls")]
	private RectTransform _rect;

	public RectTransform Rect
	{
		get
		{
			return this._rect;
		}
	}

	[SerializeField]
	[Tooltip("The decipher machine that this responds to")]
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
	[Tooltip("The size of the table cells")]
	[Min(0)]
	private float _cellSize;

	public float CellSize
	{
		get
		{
			return this._cellSize;
		}
	}

	/** Update the size of the rect transform to encompass the current letters */
	public void UpdateSize()
	{
		int plaintext_letter_index = CipherGroup.LETTERS.IndexOf(
			this.Decipherer.CurrentGroup.Plaintext[this.Decipherer.CurrentLetterIndex]);
		int ciphertext_letter_index = CipherGroup.LETTERS.IndexOf(
			this.Decipherer.CurrentGroup.Ciphertext[this.Decipherer.CurrentLetterIndex]);
		this.Rect.rect.Set(0, 0, 
			(plaintext_letter_index + 2) * this.CellSize,
			(ciphertext_letter_index + 2) * this.CellSize);
	}


	/** Reset the size of the rect transform to its default size */
	public void ResetSize()
	{
		this.Rect.rect.Set(0, 0, this.CellSize, this.CellSize);
	}

	private void Awake()
	{
		this._rect ??= this.GetComponent<RectTransform>();
		this.Decipherer.OnStart.AddListener(this.ResetSize);
		this.Decipherer.OnStep.AddListener(this.UpdateSize);
	}
}
