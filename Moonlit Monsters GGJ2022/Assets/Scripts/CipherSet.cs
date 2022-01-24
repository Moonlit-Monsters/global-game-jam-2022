using System.Collections.ObjectModel;
using System.Collections.Generic;

public class CipherSet
{
	public static readonly ReadOnlyCollection<char> LETTERS = new ReadOnlyCollection<char>(new char[]{
		'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'
	});

	/** The decrypted password */
	public string Plaintext {get; private set;}

	/** The key used to decyper the ciphertext password */
	public string Key {get; private set;}

	/** The encrypted password */
	public string Ciphertext {get; private set;}

	public CipherSet(string password, string key)
	{
		this.Plaintext = password;
		this.Key = key;
		List<int> plaintext_indices = this.StringToIndices(password);
		List<int> key_indices = this.StringToIndices(key);
		List<int> ciphertext_indices = new List<int>(plaintext_indices.Count);
		for (int i = 0; i < plaintext_indices.Count; ++i)
		{
			ciphertext_indices[i] = (plaintext_indices[i] 
				+ (key_indices[i % key_indices.Count])) % CipherSet.LETTERS.Count;
		}
		this.Ciphertext = this.IndicesToString(ciphertext_indices);
	}

	/** Covert given string to list of indices corresponding to LETTERS
	\param str The string to convert
	\return The index of each letter from the given string in LETTERS
	*/
	private List<int> StringToIndices(string str)
	{
		List<int> indices = new List<int>(str.Length);
		for (int i = 0; i < str.Length; ++i)
		{
			indices[i] = CipherSet.LETTERS.IndexOf(str[i]);
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
			str[i] = indices[i] >= 0 && indices[i] < CipherSet.LETTERS.Count 
				? CipherSet.LETTERS[indices[i]] : '_';
		}
		return new string(str);
	}
}