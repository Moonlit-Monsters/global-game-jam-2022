using UnityEngine;
using System.Collections.Generic;
using System.Collections.ObjectModel;

[System.Serializable]
public class CipherGroup
{
	/** The letters used in passwords and keys */
	public static readonly ReadOnlyCollection<char> LETTERS = new ReadOnlyCollection<char>(new char[]{
		'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'
	});

	[SerializeField]
	[Tooltip("The object holding the encrypted password")]
	private CipherPassword _passwordItem;

	public CipherPassword PasswordItem
	{
		get
		{
			return this._passwordItem;
		}
	}

	[SerializeField]
	[Tooltip("The object holding the key")]
	private CipherKey _keyItem;

	public CipherKey KeyItem
	{
		get
		{
			return this._keyItem;
		}
	}

	[SerializeField]
	[Tooltip("The gates opened with the decrypted password")]
	private List<CipherGate> _gates;

	public ReadOnlyCollection<CipherGate> Gates
	{
		get
		{
			return this._gates.AsReadOnly();
		}
	}

	/** The decrypted password used to unlock gates */
	public string Plaintext {get; private set;}

	/** The key used to decyper the ciphertext password */
	public string Key {get; private set;}

	/** The encrypted password found in the password item */
	public string Ciphertext {get; private set;}

	/** Assign the password and key for this group
	\param password The decrypted password used to unlock gates in this group
	\param key The key word contained in the key item of this group
	*/
	public void AssignPassword(string password, string key)
	{
		this.Plaintext = password;
		Debug.Log("Password: " + this.Plaintext);
		this.Key = key;
		Debug.Log("Key: " + this.Key);
		List<int> plaintext_indices = this.StringToIndices(password);
		List<int> key_indices = this.StringToIndices(key);
		List<int> ciphertext_indices = new List<int>(plaintext_indices.Count);
		for (int i = 0; i < plaintext_indices.Count; ++i)
		{
			ciphertext_indices.Add((plaintext_indices[i] 
				+ (key_indices[i % key_indices.Count])) % CipherGroup.LETTERS.Count);
		}
		this.Ciphertext = this.IndicesToString(ciphertext_indices);
		Debug.Log("Ciphertext: " + this.Ciphertext);
	}

	/** Covert given string to list of indices corresponding to LETTERS
	\param str The string to convert
	\return The index of each letter from the given string in LETTERS
	*/
	private List<int> StringToIndices(string str)
	{
		List<int> indices = new List<int>(str.Length);
		foreach (char c in str)
		{
			indices.Add(CipherGroup.LETTERS.IndexOf(c));
		}
		return indices;
	}

	/** Convert the given list of indices to a string
	\param indicies The indices of letters from LETTERS
	\return The string created from the given index list using LETTERS
	*/
	private string IndicesToString(IList<int> indices)
	{
		char[] str = new char[indices.Count];
		for (int i = 0; i < indices.Count; ++i)
		{
			str[i] = indices[i] >= 0 && indices[i] < CipherGroup.LETTERS.Count 
				? CipherGroup.LETTERS[indices[i]] : '_';
		}
		return new string(str);
	}

	/** Check if this group contains the given cipher piece
	\param piece The cipher piece to search for
	*/
	public bool DoesContain(CipherPieceBase piece)
	{
		return this.PasswordItem == piece
			|| this.KeyItem == piece
			|| this.Gates.Contains(piece as CipherGate);
	}
}